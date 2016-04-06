using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using DROM_Client.Models.BusinessObjects;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DROM_Client.Services;

namespace DROM_Client.ViewModels
{
    public class OrderPageViewModel : INotifyPropertyChanged
    {
        private APICaller _APICaller { get; set; }

        public bool Chef
        {
            get { return _chef; }
            set { Set(ref _chef, value); }
        }
        public bool Delivery
        {
            get { return _delivery; }
            set { Set(ref _delivery, value); }
        }
        public bool Manager
        {
            get { return _manager; }
            set { Set(ref _manager, value); }
        }
        public bool Waiter
        {
            get { return _waiter; }
            set { Set(ref _waiter, value); }
        }

        private bool _chef;
        private bool _delivery;
        private bool _manager;
        private bool _waiter;

        #region Property changed implementation from video (06:48): https://mva.microsoft.com/en-US/training-courses/windows-10-data-binding-14579?l=O5mda3EsB_1405632527
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //If property is updated with, raise property changed, else don't
        public bool Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
                return false;
            storage = value;
            RaisePropertyChanged(propertyName);
            return true;
        }
        #endregion

        public ObservableCollection<Order> Orders { get; set; } = new ObservableCollection<Order>();
        public List<Order> OrdersFromWebAPI { get; set; }

        public OrderPageViewModel()
        {
            _APICaller = new APICaller();
            _getOrders();

        }

        private async void _getOrders()
        {
            OrdersFromWebAPI = await _APICaller.GetOrders();
        }

        private void _filterData()
        {
            //var query = from Order o in OrdersFromWebAPI
            //            where from Event e in o.DCRGraph.Events
            //                  where from Group g in e.Groups
            //                        where g.Name == "only Pending"
            //                        select o;


            //foreach (Order o in OrdersFromWebAPI)
            //{
            //    foreach(Event e in o.DCRGraph.Events)
            //    {
                    
            //    }
            //}
        }

        public async void ExecuteEvent(Event eventToExecute)
        {
            await _APICaller.PostExecuteEvent(eventToExecute);
        }
    }
}
