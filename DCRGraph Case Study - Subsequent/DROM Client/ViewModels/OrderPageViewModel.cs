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
using DROM_Client.Models.ObjectsOptimizedForUI;
using System.Collections.Specialized;
using Windows.UI.Popups;

namespace DROM_Client.ViewModels
{
    /// <summary>
    /// OrderPage ViewModel.
    /// </summary>
    public class OrderPageViewModel : INotifyPropertyChanged
    {
        private APICaller _APICaller { get; set; }
        
        public bool Chef
        {
            get { return _chef; }
            set { Set(ref _chef, value); FilterViewAcordingToRoles(); }
        }
        private bool _chef;

        public bool Delivery
        {
            get { return _delivery; }
            set { Set(ref _delivery, value); FilterViewAcordingToRoles(); }
        }
        private bool _delivery;

        public bool Manager
        {
            get { return _manager; }
            set { Set(ref _manager, value); FilterViewAcordingToRoles(); }
        }
        private bool _manager;

        public bool Waiter
        {
            get { return _waiter; }
            set{ Set(ref _waiter, value); FilterViewAcordingToRoles(); }
        }
        private bool _waiter;

        public bool ShowOnlyPendingOrders
        {
            get { return _ShowOnlyPendingOrders; }
            set { Set(ref _ShowOnlyPendingOrders, value); FilterViewAcordingToRoles(); }
        }
        private bool _ShowOnlyPendingOrders;

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

        public ObservableCollection<Order> OrderList { get { return _OrderList; } }
        private readonly ObservableCollection<Order> _OrderList = new ObservableCollection<Order>();

        public List<Order> OrdersFromWebAPI { get; set; }

        /// <summary>
        /// Constructor for OrderPageViewModel. Gets data that view can bind to.
        /// </summary>
        public OrderPageViewModel()
        {
            _APICaller = new APICaller();
            setupData();
        }

        /// <summary>
        /// Gets data from web api and sets up collections.
        /// </summary>
        /// <returns>Tuple of bool and string, Item1 == true if success, Item2 == false if not success and Item2 contains errormessage.</returns>
        public Tuple<bool, string, List<Order>> setupData()
        {
            Tuple<bool, string, List<Order>> answerFromWebApi = _APICaller.GetOrders();
            if(answerFromWebApi.Item1 == false) CreateAndShowMessageDialog(answerFromWebApi.Item2); //Show fail message if not success
            OrdersFromWebAPI = answerFromWebApi.Item3;
            FilterViewAcordingToRoles();
            return answerFromWebApi;
        }

        /// <summary>
        /// Call to execute event.
        /// </summary>
        /// <param name="eventToExecute">Event to execute</param>
        /// <returns>Tuple of bool and string, Item1 == true if success, Item2 == false if not success and Item2 contains errormessage.</returns>
        public Tuple<bool, string> ExecuteEvent(Event eventToExecute)
        {
            return _APICaller.PutExecuteEvent(eventToExecute);
        }

        /// <summary>
        /// Archives order on Web API.
        /// </summary>
        /// <param name="orderToBeArchived">Order to archive</param>
        internal void ArchiveOrder(Order orderToBeArchived)
        {
            var archiveOrderOnWebAPI = _APICaller.PutArchiveOrder(orderToBeArchived);
            if (archiveOrderOnWebAPI.Item1 == false) CreateAndShowMessageDialog(archiveOrderOnWebAPI.Item2); //Shows error message if someting went wrong
        }

        /// <summary>
        /// Deletes order on Web API.
        /// </summary>
        /// <param name="orderToBeDeleted"></param>
        internal void DeleteOrder(Order orderToBeDeleted)
        {
            var deleteOrderOnWebAPI = _APICaller.PutDeleteOrder(orderToBeDeleted);
            if (deleteOrderOnWebAPI.Item1 == false) CreateAndShowMessageDialog(deleteOrderOnWebAPI.Item2); //Shows error message if someting went wrong
        }

        /// <summary>
        /// Method for filtering view.
        /// </summary>
        public void FilterViewAcordingToRoles()
        {
            OrderList.Clear();
            foreach (Order o in OrdersFromWebAPI)
            {
                var newOrder = CopyOrderExceptEvents(o);
                foreach (Event e in o.DCRGraph.Events)
                {
                    if (!e.Groups.Exists(ev => ev.Name == "Edit events") && e.Groups.Exists(ev => ev.Name == "only pending")) //Filter out "Edit events" and be sure the event shows for "only pending"
                    {
                        foreach (Role r in e.Roles)
                        {
                            if (Manager) //If manager is checked off, we just add all events.
                            {
                                if (!newOrder.DCRGraph.Events.Contains(e))
                                {
                                    newOrder.DCRGraph.Events.Add(e);
                                    continue;
                                }
                            }
                            if (r.Name == "Waiter" && Waiter)
                            {
                                if (!newOrder.DCRGraph.Events.Contains(e))
                                {
                                    newOrder.DCRGraph.Events.Add(e);
                                    continue;
                                }
                            }
                            if (r.Name == "Chef" && Chef)
                            {
                                if (!newOrder.DCRGraph.Events.Contains(e))
                                {
                                    newOrder.DCRGraph.Events.Add(e);
                                    continue;
                                }
                            }
                            if (r.Name == "Delivery" && Delivery)
                            {
                                if (!newOrder.DCRGraph.Events.Contains(e))
                                {
                                    newOrder.DCRGraph.Events.Add(e);
                                    continue;
                                }
                            }
                        }
                    }
                }
                if(_ShowOnlyPendingOrders && newOrder.DCRGraph.Events.Count == 0) //If no event are to be shown in order, don't add it. It is not relevant.
                {
                    continue;
                }
                OrderList.Add(newOrder);
            }
        }

        /// <summary>
        /// Copies order into new order object but omits the Events. Used when filtering to only add events relevant.
        /// </summary>
        /// <param name="orderToBeCoppied">Order to be coppied</param>
        /// <returns>Coppied order without events.</returns>
        private Order CopyOrderExceptEvents(Order orderToBeCoppied)
        {
            var newOrder = new Order()
            {
                Id = orderToBeCoppied.Id,
                ItemsAndQuantity = new List<ItemQuantity>(),
                Customer = orderToBeCoppied.Customer,
                OrderDate = orderToBeCoppied.OrderDate,
                Notes = orderToBeCoppied.Notes,
                DCRGraph = new DCRGraph() { Id = orderToBeCoppied.DCRGraph.Id, Events = new List<Event>()}, //Empty events list.
                Table = orderToBeCoppied.Table,
                OrderType = orderToBeCoppied.OrderType,
                AcceptingState = orderToBeCoppied.AcceptingState
            };
            foreach (ItemQuantity iq in orderToBeCoppied.ItemsAndQuantity) newOrder.ItemsAndQuantity.Add(iq);

            return newOrder;
        }

        /// <summary>
        /// Creates and shows a message dialog.
        /// </summary>
        /// <param name="message">Message for message log</param>
        private async void CreateAndShowMessageDialog(string message)
        {
            var messageDialog = new MessageDialog(message);
            messageDialog.CancelCommandIndex = 0;
            await messageDialog.ShowAsync();
        }
    }
}
