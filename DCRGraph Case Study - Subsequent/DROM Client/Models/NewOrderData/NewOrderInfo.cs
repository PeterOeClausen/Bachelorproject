using DROM_Client.Models.BusinessObjects;
using System;
using System.Collections.Generic;

namespace DROM_Client.Models.NewOrderData
{
    /// <summary>
    /// Container class for a newly created order (Has no Order ID for example). Also used as transfer object between Client and Web API.
    /// </summary>
    public class NewOrderInfo
    {
        public Dictionary<Item, int> ItemsAndQuantity { get; set; }
        public string OrderType { get; set; } //Either "Serving", "Delivery", "Pickup"
        public Customer Customer { get; set; }
        public DateTime OrderDate { get; set; } //DateTime.now in client
        public string Notes { get; set; }
        public int Table { get; set; }
        public int GraphType { get; set; } //we do not use this yet, but it is for determining which dcrgraph is used for the order, if system has more than one type of DCRGraph
        public int Restaurant { get; set; }
    }
}