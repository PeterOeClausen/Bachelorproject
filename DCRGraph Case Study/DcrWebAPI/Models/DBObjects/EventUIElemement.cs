namespace DcrWebAPI.Models.DBObjects
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EventUIElemement
    {
        public int Id { get; set; }

        public int IntegerSpecifyingUIElementId { get; set; }

        public int DCREventId { get; set; }

        public virtual DCREvent DCREvent { get; set; }

        public virtual IntegerSpecifyingUIElement IntegerSpecifyingUIElement { get; set; }
    }
}
