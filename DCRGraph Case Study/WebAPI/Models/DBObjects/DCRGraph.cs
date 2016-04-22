namespace WebAPI.Models.DBObjects
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DCRGraph
    {
        public DCRGraph()
        {
            DCREvents = new HashSet<DCREvent>();
        }

        public int Id { get; set; }

        public bool AcceptingState { get; set; }
        
        public virtual ICollection<DCREvent> DCREvents { get; set; }
    }
}
