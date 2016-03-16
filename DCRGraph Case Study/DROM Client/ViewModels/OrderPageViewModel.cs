using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using DROM_Client.Models.BusinessObjects;
using System.Collections.ObjectModel;
using DROM_Client.Models.BusinessObjects;

namespace DROM_Client.ViewModels
{
    public class OrderPageViewModel
    {
        public ObservableCollection<Order> Orders { get; set; } = new ObservableCollection<Order>();

        public OrderPageViewModel()
        {
            Item tempItem = new Item() {
                Id = 1,
                Name = "Chiliburger",
                Price = 69.9,
                Category = "Burger",
                Description = "Chiliburger med pommes frites"
            };

            Item tempItem2 = new Item()
            {
                Id = 1,
                Name = "Chiliburger",
                Price = 69.9,
                Category = "Drink",
                Description = "Cola"
            };

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
                        }
                    }
                },
                Table = 0
            };

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
                Table = 1
            };

            Orders.Add(order);
            Orders.Add(order2);
        }
    }
}
