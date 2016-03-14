using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using DcrWebAPI.XMLParser;

namespace DcrWebAPI.XMLParser
{
    public class EventAndRolesContainer
    {
        public int GraphId { get; set; }
        public List<DCREvent> Events { get; set; }
        public HashSet<string> Roles { get; set; }
        public List<EventRole> EventRoles  { get; set; }
        public List<Constraint> Conditions { get; set; }
        public List<Constraint> Responses { get; set; }
        public List<Constraint> Exclusions { get; set; }
        public List<Constraint> Inclusions { get; set; }
        public List<Constraint> Milestones { get; set; }




    }


    public class EventRole
    {
        public EventRole(string role, string eventId)
        {
            Role = role;
            EventId = eventId;
        }
        public string Role { get; set; }
        public string EventId { get; set; }
    }

}


