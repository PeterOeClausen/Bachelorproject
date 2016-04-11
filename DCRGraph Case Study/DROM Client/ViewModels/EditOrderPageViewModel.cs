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

        public Event EditEventToExecute;

        private APICaller _APICaller;

        internal void RemoveItem(Item key)
        {
            var replacementDictionary = new ObservableCollection<ItemQuantity>();
            foreach (var entry in OrderBeingEdited.ItemsAndQuantity)
            {
                if (entry.Item.Equals(key)) continue;
                else replacementDictionary.Add(new ItemQuantity()
                {
                    Item = entry.Item,
                    Quantity = entry.Quantity
                });
            }
            OrderBeingEdited.ItemsAndQuantity = replacementDictionary;
        }

        public ObservableCollection<Item> ItemCollection { get; set; } = new ObservableCollection<Item>();

        public EditOrderPageViewModel() {

            _APICaller = new APICaller();
            foreach (Item item in _APICaller.GetItems()) ItemCollection.Add(item);
        }

        internal void AddQuantityAndItem(int quantity, Item item)
        {
            var replacementDictionary = new ObservableCollection<ItemQuantity>();
            foreach (var entry in OrderBeingEdited.ItemsAndQuantity)
            {
                replacementDictionary.Add(new ItemQuantity()
                {
                    Item = entry.Item,
                    Quantity = entry.Quantity
                });
            }
            replacementDictionary.Add(new ItemQuantity()
            {
                Item = item,
                Quantity = quantity
            });
            OrderBeingEdited.ItemsAndQuantity = replacementDictionary;
        }

        public ObservableCollection<Event> EditEvents;

        internal void SaveOrder()
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
            
            _APICaller.PutUpdateOrder(ChangedOrder, new List<int>() { EditEvents.First().Id});
           // _APICaller.PutUpdateOrder(ChangedOrder);
        }

        public void FilterEvents()
        {
            IEnumerable<Event> eventsToRemove = OrderBeingEdited.DCRGraph.Events.Where(ev => ev.Groups.Exists(g => g.Name == "only pending"));
            foreach(Event e in eventsToRemove)
            {
                OrderBeingEdited.DCRGraph.Events.Remove(e);
            }
        }

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
    }
}
