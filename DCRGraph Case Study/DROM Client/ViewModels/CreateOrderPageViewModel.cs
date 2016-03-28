using DROM_Client.Models.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DROM_Client.ViewModels
{
    public class CreateOrderPageViewModel
    {
        public Order OrderBeingCreated { get; set; } = new Order()
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
                            Parent = false
                        }
                    }
            },
            Table = 1,
            OrderType = "To be served"
        };
    }
}
