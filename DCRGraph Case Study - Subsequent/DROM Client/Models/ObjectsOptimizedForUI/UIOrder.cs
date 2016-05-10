using DROM_Client.Models.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DROM_Client.Models.ObjectsOptimizedForUI
{
    public class UIOrder : INotifyPropertyChanged
    {
        private ObservableCollection<ItemQuantity> _ItemsAndQuantity;

        public ObservableCollection<ItemQuantity> ItemsAndQuantity
        {
            get { return _ItemsAndQuantity; }
            set { Set(ref _ItemsAndQuantity, value); }
        }

        public double TotalPrice
        {
            get { return _totalPrice; }
            set { Set(ref _totalPrice, value); }
        }
        private double _totalPrice;

        public int Id { get; set; }
        public Customer Customer { get; set; }
        public DateTime OrderDate { get; set; }
        public string Notes { get; set; }
        public UIDCRGraph DCRGraph { get; set; }
        public int Table { get; set; }
        public string OrderType {
            get { return _OrderType; }
            set { Set(ref _OrderType, value); }
        }
        private string _OrderType;

        #region PropertyChangedEvent stuff
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
        #endregion
    }
}
