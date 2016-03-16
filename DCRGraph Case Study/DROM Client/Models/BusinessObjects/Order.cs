using System;
using System.Collections.Generic;
using System.Linq;

namespace DROM_Client.Models.BusinessObjects
{
    //HEY! Please talk to Peter/Johan before changing!
    public class Order
    {
        public int Id { get; set; }
        public Dictionary<Item,int> ItemsAndQuantity { get; set; }
        public Customer Customer { get; set; }
        public DateTime OrderDate { get; set; }
        public string Notes { get; set; }
        public DCRGraph DCRGraph { get; set; }
        public int Table { get; set; }
    }
}