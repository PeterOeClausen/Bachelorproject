using System;
using System.Collections.Generic;
using System.Linq;

namespace DROM_Client.Models.BusinessObjects
{
    //HEY! Please talk to Peter/Johan before changing!
    public class Event
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public bool Included { get; set; }
        public bool Pending { get; set; }
        public bool Executed { get; set; }
        public List<Role> Roles { get; set; }
        public List<Group> Groups { get; set; }
    }
}