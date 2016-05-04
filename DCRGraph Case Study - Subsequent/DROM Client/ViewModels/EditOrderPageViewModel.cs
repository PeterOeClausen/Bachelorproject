using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DROM_Client.Models.BusinessObjects;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Data;
using System.Collections.ObjectModel;
using DROM_Client.Models.ObjectsOptimizedForUI;
using DROM_Client.Services;
using Windows.UI.Popups;

namespace DROM_Client.ViewModels
{
    public class EditOrderPageViewModel : INotifyPropertyChanged
    {
        public UIOrder OrderBeingEdited { get; set; } = new UIOrder()
        {
            Id = 2,
                ItemsAndQuantity = new ObservableCollection<ItemQuantity>() {
                    
                        new ItemQuantity()
                        {
                            Item = new Item()
                            {
                                Id = 3,
                                Name = "Sprite",
                                Category = "Drink",
                                Price = 30.0,
                                Description = "Soda"
                            },
                            Quantity = 2
                        }
                        
                    
                },
                Customer = new Customer()
                {
                    Id = 4,
                    FirstAndMiddleNames = "Fjong",
                    LastName = "Fjongson",
                    Email = "Fjong@Fjongson.com",
                    Phone = 22222222,
                    StreetAndNumber = "Qwerty Road 66",
                    ZipCode = 1234,
                    City = "Amsterdam"
                },
                OrderDate = DateTime.Now,
                Notes = "With extra ice please",
                DCRGraph = new UIDCRGraph
                {
                    Events = new ObservableCollection<Event>() {
                        new Event() {
                            Id = 1,
                            Label = "Cook order for serving",
                            Description = "Execute to confirm cooking",
                            Included = true, Pending = true, Executed = false,
                            Roles = new List<Role> {
                                new Role() {
                                    Id = 1,
                                    Name = "Chef"
                                }
                            },
                            Groups = new List<Group>
                            {
                                new Group()
                                {
                                    Id = 1,
                                    Name = "only pending"
                                }
                            }
                        },
                        new Event()
                        {
                            Id = 2,
                            Label = "Change to takeaway",
                            Included = true, Pending = true, Executed = false,
                            Roles = new List<Role>
                            {
                                new Role
                                {
                                    Id = 1,
                                    Name = "Waiter"
                                }
                            },
                            Groups = new List<Group>
                            {
                                new Group
                                {
                                    Id = 2,
                                    Name = "Edit events"
                                }
                            }
                        }
                    }
                },
                Table = 1,
                OrderType = "To be served"
            };

        public Event ItemsOnOrderHasBeenChangedEvent;
        public bool ItemsOnOrderHasBeenChanged;
        public List<Event> EditEventsToExecute = new List<Event>();
        private APICaller _APICaller;
        public ObservableCollection<Item> ItemCollection { get; set; } = new ObservableCollection<Item>();
        public ObservableCollection<Event> EditEvents;

        public EditOrderPageViewModel() {
            _APICaller = new APICaller();
            Tuple<bool, string, List<Item>> answerFromWebAPI = _APICaller.GetItems();
            if (answerFromWebAPI.Item1 == false) CreateAndShowMessageDialog(answerFromWebAPI.Item2); //Show message popup if API call fails
            foreach (Item item in answerFromWebAPI.Item3) ItemCollection.Add(item);
        }

        internal void AddItemQuantity(Item item, int quantity)
        {
            OrderBeingEdited.ItemsAndQuantity.Add(new ItemQuantity { Item = item, Quantity = quantity });
        }

        internal void RemoveItemQuantity(ItemQuantity itemQuantity)
        {
            OrderBeingEdited.ItemsAndQuantity.Remove(itemQuantity);
        }

        /// <summary>
        /// Saves edited order.
        /// </summary>
        /// <returns>Tuple with bool and string, Item1 == true if success, Item2 == false if not success and Item2 contains errormessage.</returns>
        internal Tuple<bool, string> SaveOrder()
        {
            Order ChangedOrder = new Order
            {
                Id = OrderBeingEdited.Id,
                ItemsAndQuantity = new List<ItemQuantity>(),
                Customer = OrderBeingEdited.Customer,
                OrderDate = OrderBeingEdited.OrderDate,
                Notes = OrderBeingEdited.Notes,
                DCRGraph = new DCRGraph() { Id = OrderBeingEdited.Id, Events = new List<Event>() },
                Table = OrderBeingEdited.Table,
                OrderType = OrderBeingEdited.OrderType
            };
            foreach (ItemQuantity iq in OrderBeingEdited.ItemsAndQuantity) ChangedOrder.ItemsAndQuantity.Add(iq);
            foreach (Event e in OrderBeingEdited.DCRGraph.Events) ChangedOrder.DCRGraph.Events.Add(e);
            if(ItemsOnOrderHasBeenChanged)
            {
                EditEventsToExecute.Add(ItemsOnOrderHasBeenChangedEvent);
            }
            var EventIdsToExecute = new List<int>();
            foreach (Event e in EditEventsToExecute) EventIdsToExecute.Add(e.Id);
            return _APICaller.PutUpdateOrder(ChangedOrder, EventIdsToExecute);
        }

        //Code is not in use:
        //public void FilterEvents()
        //{
        //    IEnumerable<Event> eventsToRemove = OrderBeingEdited.DCRGraph.Events.Where(ev => ev.Groups.Exists(g => g.Name == "only pending" || g.Name == "Hidden edit events"));
        //    foreach(Event e in eventsToRemove)
        //    {
        //        OrderBeingEdited.DCRGraph.Events.Remove(e);
        //    }
        //}

        #region Property changed implementation
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

        private async void CreateAndShowMessageDialog(string message)
        {
            var messageDialog = new MessageDialog(message);
            messageDialog.CancelCommandIndex = 0;
            await messageDialog.ShowAsync();
        }
    }
}
