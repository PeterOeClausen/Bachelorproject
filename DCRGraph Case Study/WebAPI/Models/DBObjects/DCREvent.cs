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
            this.IncludeFrom = new HashSet<DCREvent>();
            this.IncludeTo = new HashSet<DCREvent>();
            this.ExcludeFrom = new HashSet<DCREvent>();
            this.ExcludeTo = new HashSet<DCREvent>();
            this.ResponseFrom = new HashSet<DCREvent>();
            this.ResponseTo = new HashSet<DCREvent>();
            this.MilestoneReverseFrom = new HashSet<DCREvent>();
            this.MilestoneReverseTo = new HashSet<DCREvent>();
            this.ConditionReverseFrom = new HashSet<DCREvent>();
            this.ConditionReverseTo = new HashSet<DCREvent>();
            /*
            DCREvents3 = new HashSet<DCREvent>();
            DCRGraphs = new HashSet<DCRGraph>();
            DCREvents13 = new HashSet<DCREvent>();
            DCREvents4 = new HashSet<DCREvent>();
            DCREvents14 = new HashSet<DCREvent>();
            DCREvents5 = new HashSet<DCREvent>();
            DCREvents15 = new HashSet<DCREvent>();
            DCREvents6 = new HashSet<DCREvent>();
            */
        }

        public ICollection<DCREvent> ConditionReverseTo { get; set; }
               
        public ICollection<DCREvent> ConditionReverseFrom { get; set; }
               
        public ICollection<DCREvent> MilestoneReverseTo { get; set; }
               
        public ICollection<DCREvent> MilestoneReverseFrom { get; set; }
               
        public ICollection<DCREvent> ResponseTo { get; set; }
               
        public ICollection<DCREvent> ResponseFrom { get; set; }
               
        public ICollection<DCREvent> ExcludeTo { get; set; }
               
        public ICollection<DCREvent> ExcludeFrom { get; set; }

        public ICollection<DCREvent> IncludeFrom { get; set; }
         
        public ICollection<DCREvent> IncludeTo { get; set; }

        public int Id { get; set; }

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
    }
}
