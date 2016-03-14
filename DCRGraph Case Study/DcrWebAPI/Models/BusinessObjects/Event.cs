using System;
using System.Collections.Generic;
using System.Linq;

namespace DcrWebAPI.Models.BusinessObjects
{
    //HEY! Please talk to Peter/Johan before changing!
    public class Event
    {
        public int Id { get; set; }
        public string EventId { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public string StatusMessageAfterExecution { get; set; }
        public bool Included { get; set; }
        public bool Pending { get; set; }
        public bool Executed { get; set; }
        public List<Event> Includes { get; set; }
        public List<Event> Excludes { get; set; }
        public List<Event> Conditions { get; set; }
        public List<Event> Responses { get; set; }
        public List<Event> Milestones { get; set; }
        public List<Role> Roles { get; set; }
        public List<Group> Groups { get; set; }
        public bool Parent { get; set; }
        public List<Event> Children { get; set; }
        public List<int> RelatedOrderUIElements { get; set; }
    }
}