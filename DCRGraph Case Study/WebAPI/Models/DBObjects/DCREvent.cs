namespace WebAPI.Models.DBObjects
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DCREvent
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DCREvent()
        {
            EventUIElemements = new HashSet<EventUIElemement>();
            Includes = new HashSet<DCREvent>();
            Excludes = new HashSet<DCREvent>();
            Responses = new HashSet<DCREvent>();
            MilestonesToMe = new HashSet<DCREvent>();
            Groups = new HashSet<Group>();
            Roles = new HashSet<Role>();
            ConditionsToMe = new HashSet<DCREvent>();
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

        public int Id { get; set; }

        [Required]
        public string EventId { get; set; }

        [Required]
        public string Label { get; set; }

        public string Description { get; set; }

        public string StatusMessageAfterExecution { get; set; }

        [Required]
        public bool Included { get; set; }

        [Required]
        public bool Pending { get; set; }

        [Required]
        public bool Executed { get; set; }

        [Required]
        public bool Parent { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EventUIElemement> EventUIElemements { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DCREvent> Includes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DCREvent> ConditionsToMe { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DCREvent> MilestonesToMe { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DCREvent> Responses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Group> Groups { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Role> Roles { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DCREvent> Excludes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DCREvent> Children { get; set; }
        /*
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DCRGraph> DCRGraphs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DCREvent> DCREvents13 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DCREvent> DCREvents4 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DCREvent> DCREvents14 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DCREvent> DCREvents5 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DCREvent> DCREvents15 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DCREvent> DCREvents6 { get; set; }
        */
    }
}
