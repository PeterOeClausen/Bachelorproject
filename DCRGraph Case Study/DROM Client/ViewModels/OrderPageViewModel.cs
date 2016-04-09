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

namespace DROM_Client.ViewModels
{
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

        public OrderPageViewModel()
        {
            _APICaller = new APICaller();
            setupData();
            //setupDesignerData();
            FilterViewAcordingToRoles();
        }

        private void setupData()
        {
            OrdersFromWebAPI = _APICaller.GetOrders();
            
            foreach(Order o in OrdersFromWebAPI)
            {
                OrderList.Add(o);
            }
            //FilterView();
            #region old code (to be deleted)
            //var query = from Order o in OrdersFromWebAPI
            //            where from Event e in o.DCRGraph.Events
            //                  where from Group g in e.Groups
            //                        where g.Name == "only Pending"
            //                        select o;

            //Orders = new ObservableCollection<UIOrder>();
            //foreach(Order o in OrdersFromWebAPI)
            //{
            //    var newUIOrder = new UIOrder
            //    {
            //        Id = o.Id,
            //        Customer = o.Customer,
            //        OrderDate = o.OrderDate,
            //        Notes = o.Notes,
            //        DCRGraph = new UIDCRGraph(),
            //        Table = o.Table,
            //        OrderType = o.OrderType,
            //        ItemsAndQuantity = new Dictionary<Item, int>()
            //    };
            //}
            #endregion
        }

        /// <summary>
        /// This method sets up collections for designing view using XAML, only run when designing.
        /// </summary>
        public void setupDesignerData()
        {
            OrdersFromWebAPI = new List<Order>();

            #region Test data
            OrdersFromWebAPI.Add(
                new Order()
                {
                    Id = 1,
                    ItemsAndQuantity = new List<ItemQuantity>() {
                        new ItemQuantity() {
                            Item = new Item() {
                                    Id = 1,
                                    Name = "Chiliburger",
                                    Price = 69.9,
                                    Category = "Burger",
                                    Description = "Chiliburger med pommes frites"
                                },
                            Quantity = 1
                        }
                    },
                    Customer = new Customer()
                    {
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
                    DCRGraph = new DCRGraph
                    {
                        Id = 1,
                        Events = new List<Event>() {
                            new Event() {
                                Id = 1,
                                Label = "Confirm web order",
                                Description = "Execute to confirm",
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
                                    },
                                    new Role
                                    {
                                        Id = 2,
                                        Name = "Manager"
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
                });

            OrdersFromWebAPI.Add(new Order()
            {
                Id = 2,
                ItemsAndQuantity = new List<ItemQuantity>() {
                        new ItemQuantity()
                        {
                            Item = new Item(){
                            Id = 2,
                            Name = "Cola",
                            Price = 69.9,
                            Category = "Drink",
                            Description = "Cola"
                        }, Quantity = 2}
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
                                Label = "Cook order to eat in restaurant",
                                Description = "Execute and begin cooking order for eating in restaurant",
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
                            new Event() {
                                Id = 2,
                                Label = "Pay",
                                Description = "Execute after customer has paid",
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
                                }
                            }
                        }
                },
                Table = 1,
                OrderType = "To be served"
            });

            OrdersFromWebAPI.Add(new Order()
            {
                Id = 2,
                ItemsAndQuantity = new List<ItemQuantity>() {
                        new ItemQuantity()
                        { Item= new Item(){
                            Id = 2,
                            Name = "Cola",
                            Price = 69.9,
                            Category = "Drink",
                            Description = "Cola"
                        },Quantity = 2}
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
                            }
                        }
                },
                Table = 1,
                OrderType = "To be served"
            });

            #endregion

            foreach(Order o in OrdersFromWebAPI)
            {
                OrderList.Add(o);
            }
        }

        public async void ExecuteEvent(Event eventToExecute)
        {
            await _APICaller.PutExecuteEvent(eventToExecute);
        }

        public void FilterViewAcordingToRoles()
        {
            OrderList.Clear();
            foreach (Order o in OrdersFromWebAPI)
            {
                var newOrder = CopyOrderExceptEvents(o);
                foreach (Event e in o.DCRGraph.Events)
                {
                    if (Manager) //If manager is checked off, we just add all events. (Talk about this with Johan)
                    {
                        if (!newOrder.DCRGraph.Events.Contains(e))
                        {
                            newOrder.DCRGraph.Events.Add(e);
                            continue;
                        }
                    }
                    if (!e.Groups.Exists(ev => ev.Name == "Edit events") && e.Groups.Exists(ev => ev.Name == "only pending")) //Filter out "Edit events" and be sure the event shows for "only pending"
                    {
                        foreach (Role r in e.Roles)
                        {
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
                OrderList.Add(newOrder);
            }
        }

        private Order CopyOrderExceptEvents(Order orderToBeCoppied)
        {
            var newOrder = new Order()
            {
                Id = orderToBeCoppied.Id,
                ItemsAndQuantity = new List<ItemQuantity>(),
                Customer = orderToBeCoppied.Customer,
                OrderDate = orderToBeCoppied.OrderDate,
                Notes = orderToBeCoppied.Notes,
                DCRGraph = new DCRGraph() { Id = orderToBeCoppied.Id, Events = new List<Event>()},
                Table = orderToBeCoppied.Table,
                OrderType = orderToBeCoppied.OrderType
            };
            foreach (ItemQuantity iq in orderToBeCoppied.ItemsAndQuantity) newOrder.ItemsAndQuantity.Add(iq);

            return newOrder;
        }
    }
}
