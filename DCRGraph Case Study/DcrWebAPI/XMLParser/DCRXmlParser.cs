using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WebAPI;
using WebAPI.XMLParser;

namespace DcrWebAPI.XMLParser
{
    /// <summary>
    /// The DCRXmlParser class contains one public method for parsing, and a lot of private methods for parsing internally
    /// </summary>
    public class DCRXmlParser
    {
        /// <summary>
        /// Parses a given XML string that repressents a process model extracted from DCRGraphs.net.
        /// Returns an XmlWorkflowData object with all the data that is needed to map and create a process model.
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns>XmlWorkFlowData containing parsed data</returns>
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

        private string ParseWorkflowTitle(XDocument doc)
        {
            return doc.Descendants("dcrgraph").First().FirstAttribute.Value;
        }

        private EventAndRolesContainer ParseNodes(XDocument doc)
        {
            //Parsing constraints:
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
                                    where labelMapping.Attribute("eventId").Value.Equals(Event.Id)
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

        

        private List<Constraint> ParseConditions(XDocument doc)
        {
            var ConditionList = new List<Constraint>();

            foreach (var condition in doc.Descendants("conditions").Elements())
            {
                ConditionList.Add(new Constraint()
                {
                    fromNodeId = condition.Attribute("sourceId").Value,
                    toNodeId = condition.Attribute("targetId").Value
                });
            }

            return ConditionList;
        }

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

        private List<Constraint> ParseMilestones(XDocument doc)
        {
            var MilestonesList = new List<Constraint>();
            foreach (var milestone in doc.Descendants("milestones").Elements())
            {
                MilestonesList.Add(new Constraint()
                {
                    fromNodeId = milestone.Attribute("sourceId").Value,
                    toNodeId = milestone.Attribute("targetId").Value
                });
            }
            return MilestonesList;
        }

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

        private List<Constraint> ParseResponsesReversed(XDocument doc)
        {
            var ResponseList = new List<Constraint>();

            foreach (var response in doc.Descendants("responses").Elements())
            {
                ResponseList.Add(new Constraint()
                {
                    fromNodeId = response.Attribute("targetId").Value,
                    toNodeId = response.Attribute("sourceId").Value
                });
            }

            return ResponseList;
        }

        private List<Constraint> ParseExclusionsReversed(XDocument doc)
        {
            var ExcludesList = new List<Constraint>();
            foreach (var exclude in doc.Descendants("excludes").Elements())
            {
                ExcludesList.Add(new Constraint()
                {
                    fromNodeId = exclude.Attribute("targetId").Value,
                    toNodeId = exclude.Attribute("sourceId").Value
                });
            }
            return ExcludesList;
        }

        private List<Constraint> ParseIncludesReversed(XDocument doc)
        {
            var IncludesList = new List<Constraint>();
            foreach (var include in doc.Descendants("includes").Elements())
            {
                IncludesList.Add(new Constraint()
                {
                    fromNodeId = include.Attribute("targetId").Value,
                    toNodeId = include.Attribute("sourceId").Value
                });
            }
            return IncludesList;
        }

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


        //should no longer be nececary
        private List<string> ParseRoles(XDocument doc)
        {
            var RolesList = new List<string>();

            foreach (var role in doc.Descendants("roles").Elements().Where(element => element.Parent.Parent.Parent.Name != "event"))
            {
                if (role.Value != "") RolesList.Add(role.Value);
            }

            return RolesList;
        }
    }
}
