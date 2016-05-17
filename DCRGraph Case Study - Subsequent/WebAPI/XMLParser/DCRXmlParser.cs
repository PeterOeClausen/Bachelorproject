using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using WebAPI.Models.DBObjects;

namespace WebAPI.XMLParser
{
    /// <summary>
    /// The DCRXmlParser class contains one public method for parsing, and a lot of private methods for parsing internally
    /// </summary>
    public class DCRXmlParser
    {
        /// <summary>
        /// Parses a given xml string that repressents a DCRGraph from DCRGraphs.net.
        /// Returns all the informaton needed to create a DCRGraph.
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public EventAndRolesContainer Parse(string xmlString)
        {
            XDocument doc = XDocument.Parse(xmlString);

            //Workflow title
            string workflowTitle = ParseWorkflowTitle(doc);

            //Nodes
            EventAndRolesContainer container = ParseNodes(doc);

            //Constraints:
            container.Conditions = ParseConditionsReversed(doc);
            container.Responses = ParseResponses(doc);
            container.Exclusions = ParseExclusions(doc);
            container.Inclusions = ParseIncludes(doc);
            container.Milestones = ParseMilestonesReversed(doc);
            //List<string> RolesList = ParseRoles(doc);

            //Return data:
            return container;
        }

        /// <summary>
        /// Method to get the workflow title from an xml file
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private string ParseWorkflowTitle(XDocument doc)
        {
            return doc.Descendants("dcrgraph").First().FirstAttribute.Value;
        }

        /// <summary>
        /// Method to parse a DCRGraph from an xml file. It will read all the informaion and put into lists.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private EventAndRolesContainer ParseNodes(XDocument doc)
        {
            //Mapper constraints:
            var idOfIncludedEvents = (from includedEvent in doc.Descendants("included").Elements()
                                      select includedEvent.FirstAttribute.Value); //Think about checking for ID.

            var idOfPendingEvents = (from pendingEvent in doc.Descendants("pendingResponses").Elements()
                                     select pendingEvent.FirstAttribute.Value); //Think about checking for ID.

            var idOfExecutedEvents = (from executedEvent in doc.Descendants("executed").Elements()
                                      select executedEvent.FirstAttribute.Value); //Think about checking for ID.

            IEnumerable<XElement> events = doc.Descendants("event").Where(element => element.HasElements); //Only takes event elements in events!

            //Creating result list:
            EventAndRolesContainer Container = new EventAndRolesContainer();
            //Creating and adding data to nodes:
            foreach (var _event in events)
            {
                DCREvent Event = new DCREvent(); //I create one NodeData for each _event

                //Assigning Id
                Event.EventId = _event.Attribute("id").Value;

                //Assigning Name:
                Event.Label = (from labelMapping in doc.Descendants("labelMapping")
                                    where labelMapping.Attribute("eventId").Value.Equals(Event.EventId)
                                    select labelMapping.Attribute("labelId").Value).FirstOrDefault();

                //Assigning Roles:
                var roles = _event.Descendants("role");
                foreach (var role in roles)
                {
                    if (role.Value != "")
                    {
                        Container.Roles.Add(role.Value);
                        Container.EventRoles.Add(new WebAPI.XMLParser.EventRole(role.Value, Event.EventId));
                    }
                }

                //Assigning Groups:
                var groups = _event.Descendants("group");
                foreach (var group in groups)
                {
                    if (group.Value != "")
                    {
                        Container.Groups.Add(group.Value);
                        Container.EventGroups.Add(new WebAPI.XMLParser.EventGroup(group.Value, Event.EventId));
                    }
                }





                //Mark Included
                if (idOfIncludedEvents.Contains(Event.EventId)) Event.Included = true;

                //Mark Pending:
                if (idOfPendingEvents.Contains(Event.EventId)) Event.Pending = true;

                //Mark Executed:
                if (idOfExecutedEvents.Contains(Event.EventId)) Event.Executed = true;

                //Add Created Node to collection
                Container.Events.Add(Event);
            }

            return Container;
        }

        /// <summary>
        /// Method to parse responses from an xml file.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private List<Constraint> ParseResponses(XDocument doc)
        {
            var ResponseList = new List<Constraint>();

            foreach (var response in doc.Descendants("responses").Elements())
            {
                ResponseList.Add(new Constraint()
                {
                    fromNodeId = response.Attribute("sourceId").Value,
                    toNodeId = response.Attribute("targetId").Value
                });
            }

            return ResponseList;
        }

        /// <summary>
        /// Method to parse exclusions from an xml file.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private List<Constraint> ParseExclusions(XDocument doc)
        {
            var ExcludesList = new List<Constraint>();
            foreach (var exclude in doc.Descendants("excludes").Elements())
            {
                ExcludesList.Add(new Constraint()
                {
                    fromNodeId = exclude.Attribute("sourceId").Value,
                    toNodeId = exclude.Attribute("targetId").Value
                });
            }
            return ExcludesList;
        }

        /// <summary>
        /// Method to parse includes from an xml file.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private List<Constraint> ParseIncludes(XDocument doc)
        {
            var IncludesList = new List<Constraint>();
            foreach (var include in doc.Descendants("includes").Elements())
            {
                IncludesList.Add(new Constraint()
                {
                    fromNodeId = include.Attribute("sourceId").Value,
                    toNodeId = include.Attribute("targetId").Value
                });
            }
            return IncludesList;
        }

        /// <summary>
        /// Method to parse conditions from an xml file.
        /// Conditions are parsed in reverse, because it is easier to work with if an event which is being executed,
        /// knows which other events have conditions to it, instead of having to go through all events to see if there
        /// are any with conditions to an event.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private List<Constraint> ParseConditionsReversed(XDocument doc)
        {
            var ConditionList = new List<Constraint>();

            foreach (var condition in doc.Descendants("conditions").Elements())
            {
                ConditionList.Add(new Constraint()
                {
                    fromNodeId = condition.Attribute("targetId").Value,
                    toNodeId = condition.Attribute("sourceId").Value
                });
            }

            return ConditionList;
        }

        /// <summary>
        /// Method to parse milestones from an xml file.
        /// Milestones are parsed in reverse, because it is easier to work with if an event which is being executed,
        /// knows which other events have milestones to it, instead of having to go through all events to see if there
        /// are any with milestones to an event.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private List<Constraint> ParseMilestonesReversed(XDocument doc)
        {
            var MilestonesList = new List<Constraint>();
            foreach (var milestone in doc.Descendants("milestones").Elements())
            {
                MilestonesList.Add(new Constraint()
                {
                    fromNodeId = milestone.Attribute("targetId").Value,
                    toNodeId = milestone.Attribute("sourceId").Value
                });
            }
            return MilestonesList;
        }

    }
}
