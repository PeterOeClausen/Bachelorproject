namespace WebAPI.Models.DBObjects
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class DCREvent
    {
        
        public DCREvent()
        {
            EventUIElemements = new HashSet<EventUIElemement>();
            Groups = new HashSet<Group>();
            Roles = new HashSet<Role>();
            this.Includes = new HashSet<DCREvent>();
            this.Excludes = new HashSet<DCREvent>();
            this.Responses = new HashSet<DCREvent>();
            this.Milestones = new HashSet<DCREvent>();
            this.Conditions = new HashSet<DCREvent>();
        }

        public int Id { get; set; }

        public int DCRGraphId { get; set; }

        [Required]
        public string EventId { get; set; }

        [Required]
        public string Label { get; set; }

        public string Description { get; set; }

        [Required]
        public bool Included { get; set; }

        [Required]
        public bool Pending { get; set; }

        [Required]
        public bool Executed { get; set; }

        [Required]
        public bool Parent { get; set; }

      
        public virtual ICollection<EventUIElemement> EventUIElemements { get; set; }

        public virtual ICollection<Group> Groups { get; set; }

        public virtual ICollection<Role> Roles { get; set; }

        public virtual ICollection<DCREvent> Conditions { get; set; }

        public virtual ICollection<DCREvent> Milestones { get; set; }

        public virtual ICollection<DCREvent> Responses { get; set; }

        public virtual ICollection<DCREvent> Excludes { get; set; }

        public virtual ICollection<DCREvent> Includes { get; set; }
    }
}
