using DROM_Client.Models.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DROM_Client.Models.NewOrderData
{
    public class NewOrderInfo
    {
        public Dictionary<Item, int> ItemsAndQuantity { get; set; }
        public string OrderType { get; set; } //Either "For Serving", "For Delivery", "For Pickup"
        public Customer Customer { get; set; } //Null if Serving, Phone if pickup, All if delivery
        public DateTime OrderDate { get; set; } //Client side sets this
        public string Notes { get; set; }
        public int Table { get; set; } // -1 if order is for pickup or delivery
    }
}