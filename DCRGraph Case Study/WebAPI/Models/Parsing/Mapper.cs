using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WebAPI.XMLParser;
using DROM_Client.Models.NewOrderData;
using WebAPI.Models.DBObjects;
using System.Data.Entity;

namespace WebAPI.Models.Parsing
{
    class Mapper
    {
        
        public Mapper(EventAndRolesContainer container, NewOrderInfo orderInfo)
        {
            using (var db = new WebAPI.Models.DBObjects.Database())
            {

                
                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        var graph = new DCRGraph();
                        graph.DCREvents = container.Events;

                        var order = new Order()
                        {

                            DCRGraph = graph,
                            OrderDate = orderInfo.OrderDate,
                            Notes = orderInfo.Notes,
                            Table = orderInfo.Table,
                            OrderDetails = new List<OrderDetail>()
                            };
                        foreach (var iq in orderInfo.ItemsAndQuantity)
                        {
                            
                            var item =
                                db.Items
                                    .FirstAsync(i => i.Name == iq.Key.Name).Result;
                            if (item == null)
                            {
                                throw new Exception("Item '" + iq.Key.Name + "' did not exist in the database");
                            }
                            
                            order.OrderDetails.Add(
                                new OrderDetail()
                                {
                                    ItemId = item.Id,
                                    Item = item,
                                    Order = order
                                });

                        }
                        
                        


                        //Determine if there should be a customer on the order

                        if (orderInfo.OrderType != "Serving")
                        {
                            var customer =
                                db.Customers
                                    .FirstAsync(c => c.Phone == orderInfo.Customer.Phone).Result;



                            if (customer == null)
                            {
                                customer = new Customer()
                                {
                                    City = orderInfo.Customer.City,
                                    Email = orderInfo.Customer.Email,
                                    FirstName = orderInfo.Customer.FirstAndMiddleNames,
                                    LastName = orderInfo.Customer.LastName,
                                    Phone = orderInfo.Customer.Phone,
                                    StreetAndNumber = orderInfo.Customer.StreetAndNumber,
                                    Zipcode = orderInfo.Customer.ZipCode,
                                    

                                };
                                customer.Orders = new HashSet<Order>();
                                customer.Orders.Add(order);
                            }
                            order.Customer = customer;

                        }
                        

                        

                        //put roles on events
                        foreach (var i in container.EventRoles)
                        {

                            
                            var role = db.Roles.FirstAsync(x => x.Name.Equals(i.RoleName)).Result;
                            container.Events.Find(x => x.EventId.Equals(i.EventId)).Roles.Add(role);

                        }

                        //put groups on events
                        foreach (var i in container.EventGroups)
                        {


                            var group = db.Groups.FirstAsync(x => x.Name.Equals(i.GroupName)).Result;
                            container.Events.Find(x => x.EventId.Equals(i.EventId)).Groups.Add(group);

                        }

                        //put inclusions on events
                        foreach (var i in container.Inclusions)
                        {

                            container.Events.Find(x => x.EventId.Equals(i.fromNodeId)).Includes.Add(
                                container.Events.Find(x => x.EventId.Equals(i.toNodeId)));
                            
                        }

                        //put exclusions on events
                        foreach (var i in container.Exclusions)
                        {

                            container.Events.Find(x => x.EventId.Equals(i.fromNodeId)).Includes.Add(
                                container.Events.Find(x => x.EventId.Equals(i.toNodeId)));

                        }

                        //put responses on events
                        foreach (var i in container.Responses)
                        {

                            container.Events.Find(x => x.EventId.Equals(i.fromNodeId)).Includes.Add(
                                container.Events.Find(x => x.EventId.Equals(i.toNodeId)));

                        }

                        //put conditions on events
                        foreach (var i in container.Conditions)
                        {

                            container.Events.Find(x => x.EventId.Equals(i.fromNodeId)).Includes.Add(
                                container.Events.Find(x => x.EventId.Equals(i.toNodeId)));

                        }

                        //put milestones on events
                        foreach (var i in container.Milestones)
                        {

                            container.Events.Find(x => x.EventId.Equals(i.fromNodeId)).Includes.Add(
                                container.Events.Find(x => x.EventId.Equals(i.toNodeId)));

                        }


                        db.DCREvents.AddRange(container.Events);
                        db.SaveChanges();
                        

                        scope.Complete();
                    }
                    catch (Exception)
                    {

                        
                        throw;
                    }
                    
                }

            }
        }
    
    }
    
}
