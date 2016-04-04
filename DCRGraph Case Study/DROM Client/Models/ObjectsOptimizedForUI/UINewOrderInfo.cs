using DROM_Client.Models.BusinessObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DROM_Client.Models.ObjectsOptimizedForUI
{
    public class UINewOrderInfo : INotifyPropertyChanged
    {
        private Dictionary<Item, int> _ItemsAndQuantity;

        public Dictionary<Item, int> ItemsAndQuantity
        {
            get { return _ItemsAndQuantity; }
            set { Set(ref _ItemsAndQuantity, value); }
        }

        public string OrderType { get; set; } //Either "For Serving", "For Delivery", "For Pickup"
        public Customer Customer { get; set; } //Null if Serving, Phone if pickup, All if delivery
        public DateTime OrderDate { get; set; } //Client side sets this
        public string Notes { get; set; }
        public string Table { get; set; } // -1 if order is for pickup or delivery
        

        public event PropertyChangedEventHandler PropertyChanged;

        public bool Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
                return false;
            storage = value;
            RaisePropertyChanged(propertyName);
            return true;
        }

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
