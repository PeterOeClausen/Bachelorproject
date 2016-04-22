using DROM_Client.Models.NewOrderData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DROM_Client.Models.BusinessObjects;

namespace DROM_Client.Services
{
    public class APICaller
    {

        public Uri BaseAddress { get; set; }

        public APICaller()
        {
            BaseAddress = new Uri("http://localhost:57815/"); //set the address of the api here
        }

        /// <summary>
        /// Save order on web api.
        /// </summary>
        /// <param name="newOrder">New Order to be saved.</param>
        /// <returns>Tuple of bool and string, bool == true when API succeded, bool == false when API did not succeed, string == fail message.</returns>
        public Tuple<bool,string> PostOrderAsync(NewOrderInfo newOrder) //Rename to: PostNewOrder
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = BaseAddress;
                    var response = client.PostAsXmlAsync("api/parse", newOrder, new CancellationToken()).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var answer = new Tuple<bool, string>(true, response.StatusCode.ToString());
                        return answer;
                    }
                    else //do failure thing
                    {
                        var answer = new Tuple<bool, string>(false, "Error from Web api: " + response.StatusCode.ToString() + ": " + response.ReasonPhrase);
                        return answer;
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Calls API to update order.
        /// </summary>
        /// <param name="updatedOrder">Updated order to be saved</param>
        /// <param name="editEvents">List of id's of edit events to be executed</param>
        /// <returns></returns>
        public Tuple<bool, string> PutUpdateOrder(Order updatedOrder, List<int> editEvents)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var dto = new Tuple<Order, List<int>>(updatedOrder, editEvents);
                    client.BaseAddress = BaseAddress;
                    var response = client.PutAsXmlAsync("api/order/updateorder", dto).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return new Tuple<bool, string>(true, response.StatusCode.ToString());
                    }
                    else //do failure thing
                    {
                        return new Tuple<bool, string>(false, "Error from Web api: " + response.StatusCode.ToString() + ": " + response.ReasonPhrase);
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Execute event on API
        /// </summary>
        /// <param name="eventToExecute"></param>
        /// <returns></returns>
        public string PutExecuteEvent(Event eventToExecute)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                var response = client.PutAsXmlAsync("api/order/executeevent", eventToExecute).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.StatusCode.ToString();
                }
                else //Do faillure thing
                {
                    return response.StatusCode.ToString();
                }
            }
        }

        /// <summary>
        /// Receive all orders
        /// </summary>
        /// <returns></returns>
        public List<Order> GetOrders()
        {
            #region testdata:
            new List<Order>()
                {
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
                },

                new Order()
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
                },

                    new Order()
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
                }
            };
            #endregion

            //using (var client = new HttpClient())
            //{
            //    return null; //Not implemented yet
            //}


            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = BaseAddress;
                    var response = client.GetAsync("api/order/orderswithsortedevents", new CancellationToken()).Result;
                    var ordersReceived = response.Content.ReadAsAsync<List<Order>>().Result;

                   // var settings = new JsonSerializerSettings { Converters = new JsonConverter[] { new DictionaryConverter() } };
                   // var ordersReceived = JsonConvert.DeserializeObject<List<Order>>(jsonOrdersReceived, settings);
                    response.EnsureSuccessStatusCode();
                    return ordersReceived;
                }
                catch (Exception ex)
                {
                    return new List<Order>();
                }
            }
        }

        public List<Item> GetItems() //Needs to be called only one time.
        {
            //var items = new List<Item> {
            //    new Item
            //    {
            //        Name = "Cola"
            //    },
            //    new Item
            //    {
            //        Name = "Sprite"
            //    },
            //    new Item
            //    {
            //        Name = "Pizza"
            //    },
            //    new Item
            //    {
            //        Name = "Burger"
            //    }
            //};
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = BaseAddress;
                    var response = client.GetAsync("api/order/items", new CancellationToken()).Result;
                    var itemsReceived = response.Content.ReadAsAsync<List<Item>>().Result;
                    response.EnsureSuccessStatusCode();
                    return itemsReceived;

                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public List<string> GetDeliveryTypes()
        {
            var orderGraphType = 0; //this is not really used atm., but will be used in case more than one type of dcrgraphs is in the system
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = BaseAddress;
                    var response = client.GetAsync("api/order/deliveryTypes/" + orderGraphType, new CancellationToken()).Result;
                    var deliveryTypesReceived = response.Content.ReadAsAsync<List<string>>().Result;
                    response.EnsureSuccessStatusCode();
                    return deliveryTypesReceived;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public string PutArchiveOrder(Order order)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = BaseAddress;
                    var response = client.PutAsXmlAsync("api/order/archive", order).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        //do succes thing
                        return response.StatusCode.ToString();
                    }
                    else
                    {
                        //do failure thing
                        return response.StatusCode.ToString();
                    }
                    
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
