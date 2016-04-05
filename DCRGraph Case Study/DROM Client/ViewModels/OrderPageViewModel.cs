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

        public OrderPageViewModel()
        {
            _APICaller = new APICaller();

            #region items
            Item tempItem = new Item() {
                Id = 1,
                Name = "Chiliburger",
                Price = 69.9,
                Category = "Burger",
                Description = "Chiliburger med pommes frites"
            };

            Item tempItem2 = new Item()
            {
                Id = 2,
                Name = "Cola",
                Price = 69.9,
                Category = "Drink",
                Description = "Cola"
            };
            #endregion items

            #region Order1
            Order order = new Order()
            {
                Id = 1,
                ItemsAndQuantity = new Dictionary<Item, int>() {
                    { tempItem,1}
                },
                Customer = new Customer() {
                    Id = 1,
                    FirstAndMiddleNames = "John",
                    LastName = "Doe",
                    Email = "John@Doe.com",
                    Phone = 88888888,
                    StreetAndNumber = "Rued Langaardsvej 7",
                    ZipCode = 2300,
                    City = "København S"
                },
                OrderDate = DateTime.Now,
                Notes = "Minus tomatoes please",
                DCRGraph = new DCRGraph {
                    Id = 1,
                    Events = new List<Event>() {
                        new Event() {
                            Id = 1,
                            EventId = "Activity 1",
                            Label = "Confirm web order",
                            Description = "Execute to confirm",
                            StatusMessageAfterExecution = "Web order confirmed",
                            Included = true, Pending = true, Executed = false,
                            Roles = new List<Role> {
                                new Role() {
                                    Id = 1,
                                    Name = "Waiter"
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
                            Parent = false,
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
                Table = 0,
                OrderType = "To be delivered"
            };
            #endregion Order1

            #region Order2
            Order order2 = new Order()
            {
                Id = 2,
                ItemsAndQuantity = new Dictionary<Item, int>() {
                    { tempItem2,2}
                },
                Customer = new Customer()
                {
                    Id = 2,
                    FirstAndMiddleNames = "Alice",
                    LastName = "Allen",
                    Email = "Alice@Allen.com",
                    Phone = 77777777,
                    StreetAndNumber = "Langaardsvej Rued 7",
                    ZipCode = 2300,
                    City = "København S"
                },
                OrderDate = DateTime.Now,
                Notes = "Minus ice please",
                DCRGraph = new DCRGraph
                {
                    Id = 2,
                    Events = new List<Event>() {
                        new Event() {
                            Id = 1,
                            EventId = "Activity 1",
                            Label = "Cook order to eat in restaurant",
                            Description = "Execute and begin cooking order for eating in restaurant",
                            StatusMessageAfterExecution = "Order is being cooked",
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
                        new Event() {
                            Id = 2,
                            EventId = "Activity 2",
                            Label = "Pay",
                            Description = "Execute after customer has paid",
                            StatusMessageAfterExecution = "Order has been paid",
                            Included = true, Pending = true, Executed = false,
                            Roles = new List<Role> {
                                new Role() {
                                    Id = 1,
                                    Name = "Waiter"
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
                        }
                    }
                },
                Table = 1,
                OrderType = "To be served"
            };
            #endregion Order2

            #region Order3
            Order order3 = new Order()
            {
                Id = 2,
                ItemsAndQuantity = new Dictionary<Item, int>() {
                    { tempItem2,2}
                },
                Customer = new Customer()
                {
                    Id = 2,
                    FirstAndMiddleNames = "Peter Øvergård",
                    LastName = "Clausen",
                    Email = "PeterOeClausen@gmail.com",
                    Phone = 77777777,
                    StreetAndNumber = "Langaardsvej Rued 7",
                    ZipCode = 2300,
                    City = "København S"
                },
                OrderDate = DateTime.Now,
                Notes = "Blablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablabla",
                DCRGraph = new DCRGraph
                {
                    Id = 2,
                    Events = new List<Event>() {
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
                            Parent = false,
                        }
                    }
                },
                Table = 1,
                OrderType = "To be served"
            };
            #endregion Order3

            Orders.Add(order);
            Orders.Add(order2);
            Orders.Add(order3);
        }

        public async void ExecuteEvent(Event eventToExecute)
        {
            await _APICaller.PostExecuteEvent(eventToExecute);
        }
    }
}
