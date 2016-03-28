namespace WebAPI.Models.DBObjects
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrderDetail
    {
        public int Id { get; set; }

        public int ItemId { get; set; }

        public int OrderId { get; set; }

        public virtual Item Item { get; set; }

        public virtual Order Order { get; set; }
    }
}
