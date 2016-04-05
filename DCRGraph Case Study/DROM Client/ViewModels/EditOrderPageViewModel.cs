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

namespace DROM_Client.ViewModels
{
    public class EditOrderPageViewModel : INotifyPropertyChanged
    {
        public UIOrder OrderBeingEdited { get; set; } = new UIOrder()
        {
            Id = 2,
                ItemsAndQuantity = new Dictionary<Item, int>() {
                    {
                        new Item() {
                            Id = 3,
                            Name = "Sprite",
                            Category = "Drink",
                            Price = 30.0,
                            Description = "Soda"
                        },2
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
                            EventId = "Activity 1",
                            Label = "Cook order for serving",
                            Description = "Execute to confirm cooking",
                            StatusMessageAfterExecution = "Order is ready to be served",
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
                            },
                            Parent = false
                        },
                        new Event()
                        {
                            Id = 2,
                            EventId = "Activity 2",
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

        internal void RemoveItem(Item key)
        {
            Dictionary<Item, int> replacementDictionary = new Dictionary<Item, int>();
            foreach (KeyValuePair<Item, int> entry in OrderBeingEdited.ItemsAndQuantity)
            {
                if (entry.Key.Equals(key)) continue;
                else replacementDictionary.Add(entry.Key, entry.Value);
            }
            OrderBeingEdited.ItemsAndQuantity = replacementDictionary;
        }

        public ObservableCollection<Item> ItemCollection { get; set; }

        public EditOrderPageViewModel() {
            ItemCollection = new ObservableCollection<Item>()
            {
                new Item
                {
                    Name = "Cola"
                },
                new Item
                {
                    Name = "Sprite"
                },
                new Item
                {
                    Name = "Pizza"
                },
                new Item
                {
                    Name = "Burger"
                }
            };
        }

        internal void AddQuantityAndItem(int quantity, Item item)
        {
            Dictionary<Item, int> replacementDictionary = new Dictionary<Item, int>();
            foreach (KeyValuePair<Item, int> entry in OrderBeingEdited.ItemsAndQuantity)
            {
                replacementDictionary.Add(entry.Key, entry.Value);
            }
            replacementDictionary.Add(item, quantity);
            OrderBeingEdited.ItemsAndQuantity = replacementDictionary;
        }

        public ObservableCollection<Event> EditEvents;

        internal void SaveOrder()
        {
            //Call web api.
        }

        //public List<Event> EditEvents {get; set;}

        public void FilterCollection()
        {
            var eventsToRemove = new List<Event>();
            foreach(Event editEvent in OrderBeingEdited.DCRGraph.Events)
            {
                foreach(Group g in editEvent.Groups)
                {
                    
                }
            }
            //EditEvents.Where(e => e.Groups.);
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
