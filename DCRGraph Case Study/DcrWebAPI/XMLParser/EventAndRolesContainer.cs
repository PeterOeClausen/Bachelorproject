using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using DcrWebAPI.Models.DBObjects;
using DcrWebAPI.XMLParser;

namespace DcrWebAPI.XMLParser
{
    public class EventAndRolesContainer
    {
        public int GraphId { get; set; }
        public List<DCREvent> Events { get; set; }
        public HashSet<string> Roles { get; set; }
        public HashSet<string> Groups { get; set; }
        public List<EventRole> EventRoles { get; set; }
        public List<EventGroup> EventGroups { get; set; }
        public List<Constraint> Conditions { get; set; }
        public List<Constraint> Responses { get; set; }
        public List<Constraint> Exclusions { get; set; }
        public List<Constraint> Inclusions { get; set; }
        public List<Constraint> Milestones { get; set; }

        public EventAndRolesContainer()
        {
            Events = new List<DCREvent>();
            Roles = new HashSet<string>();
            Groups = new HashSet<string>();
            EventRoles = new List<EventRole>();
            EventGroups = new List<EventGroup>();
            Conditions = new List<Constraint>();
            Responses = new List<Constraint>();
            Exclusions = new List<Constraint>();
            Inclusions = new List<Constraint>();
            Milestones = new List<Constraint>();
    }






    }


    public class EventRole
    {
        public string RoleName { get; set; }
        public string EventId { get; set; }

        public EventRole(string roleName, string eventId)
        {
            RoleName = roleName;
            EventId = eventId;
        }

    }

    public class EventGroup
    {
        public string EventId { get; set; }
        public string GroupName { get; set; }

        public EventGroup (string groupName, string eventId)
        {
            GroupName = groupName;
            EventId = eventId;
        }
    }

}


