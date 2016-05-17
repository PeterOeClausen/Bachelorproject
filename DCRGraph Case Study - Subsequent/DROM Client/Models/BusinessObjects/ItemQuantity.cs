using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DROM_Client.Models.BusinessObjects
{
    /// <summary>
    /// Container for Item (food on order) and Quantity data. Also used as a transferobject between client and Web API.
    /// </summary>
    public class ItemQuantity
    {
        public Item Item { get; set; }
        public int Quantity { get; set; }
    }
}
