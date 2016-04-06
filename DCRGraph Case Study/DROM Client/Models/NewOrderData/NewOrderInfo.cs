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
        public string OrderType { get; set; } //Either "Serving", "Delivery", "Pickup"
        public Customer Customer { get; set; } //Null if Serving, otherwise all info.
        public DateTime OrderDate { get; set; } //DateTime.now in client
        public string Notes { get; set; }
        public int Table { get; set; } 
    }
}