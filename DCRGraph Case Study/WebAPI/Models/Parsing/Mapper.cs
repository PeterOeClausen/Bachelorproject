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
                            OrderDetails = new List<OrderDetail>(),
                            OrderType = orderInfo.OrderType
                            
                            };
                        foreach (var iq in orderInfo.ItemsAndQuantity)
                        {
                            
                            var item =
                                db.Items
                                    .FirstOrDefaultAsync(i => i.Name == iq.Key.Name).Result;
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
                                    .FirstOrDefaultAsync(c => c.Phone == orderInfo.Customer.Phone).Result;



                            if (customer == null)
                            {
                                if(orderInfo.Customer.Phone == 0) throw new Exception("Missing phone number on customer - Mapper.cs");
                                customer = new Customer()
                                {
                                    City = orderInfo.Customer.City ?? "n/a",
                                    Email = orderInfo.Customer.Email ?? "n/a",
                                    FirstName = orderInfo.Customer.FirstAndMiddleNames ?? "n/a",
                                    LastName = orderInfo.Customer.LastName ?? "n/a",
                                    Phone = orderInfo.Customer.Phone,
                                    StreetAndNumber = orderInfo.Customer.StreetAndNumber ?? "n/a",
                                    Zipcode = orderInfo.Customer.ZipCode
                                    

                                };
                                
                                customer.Orders = new HashSet<Order>();
                                customer.Orders.Add(order);
                            }
                            order.Customer = customer;

                        }
                        db.Orders.Add(order);
                        db.SaveChanges();




                        /*
                        var adAgency = context.Companies.Single(c => c.AgencyType == AgencyType.Advertising);
                        var adClients = context.Companies.Where(c => c.City.StartsWith("B") && c.AgencyType == AgencyType.NotSet).ToList();
                        adAgency.Clients = adClients;

                        var digitalAgency = context.Companies.Single(c => c.AgencyType == AgencyType.Digital);
                        var digiClients = context.Companies.Where(c => c.City.StartsWith("L") && c.AgencyType == AgencyType.NotSet).ToList();
                        digitalAgency.Clients = digiClients;

                        var prAgency = context.Companies.Single(c => c.AgencyType == AgencyType.PR);
                        var client = context.Companies.Single(c => c.CompanyName == "Black");
                        prAgency.Clients.Add(client);
                        */

                        //put groups on events
                        foreach (var i in container.EventGroups)
                        {


                            var group = db.Groups.FirstOrDefaultAsync(x => x.Name.Equals(i.GroupName)).Result;
                            container.Events.Find(x => x.EventId.Equals(i.EventId)).Groups.Add(group);


                        }

                        
                        //put roles on events
                        foreach (var i in container.EventRoles)
                        {

                            var role = db.Roles.FirstOrDefaultAsync(x => x.Name.Equals(i.RoleName)).Result;
                            container.Events.Find(x => x.EventId.Equals(i.EventId)).Roles.Add(role);


                        }
                        
                        
                        //put inclusions on events
                        foreach (var i in container.Inclusions)
                        {

                            var fromEvent = container.Events.Find(x => x.EventId.Equals(i.fromNodeId));
                            var toEvent = container.Events.Find(x => x.EventId.Equals(i.toNodeId));

                            container.Events.Find(x => x.EventId.Equals(i.fromNodeId)).IncludeFrom.Add(toEvent);
                            container.Events.Find(x => x.EventId.Equals(i.fromNodeId)).IncludeTo.Add(fromEvent);

                        }
                        
                        //put exclusions on events
                        foreach (var i in container.Exclusions)
                        {

                            var fromEvent = container.Events.Find(x => x.EventId.Equals(i.fromNodeId));
                            var toEvent = container.Events.Find(x => x.EventId.Equals(i.toNodeId));

                            container.Events.Find(x => x.EventId.Equals(i.fromNodeId)).ExcludeFrom.Add(toEvent);
                            container.Events.Find(x => x.EventId.Equals(i.fromNodeId)).ExcludeTo.Add(fromEvent);

                        }

                        //put responses on events
                        foreach (var i in container.Responses)
                        {

                            var fromEvent = container.Events.Find(x => x.EventId.Equals(i.fromNodeId));
                            var toEvent = container.Events.Find(x => x.EventId.Equals(i.toNodeId));

                            container.Events.Find(x => x.EventId.Equals(i.fromNodeId)).ResponseFrom.Add(toEvent);
                            container.Events.Find(x => x.EventId.Equals(i.fromNodeId)).ResponseTo.Add(fromEvent);

                        }

                        //put conditions on events
                        foreach (var i in container.Conditions)
                        {

                            var fromEvent = container.Events.Find(x => x.EventId.Equals(i.fromNodeId));
                            var toEvent = container.Events.Find(x => x.EventId.Equals(i.toNodeId));

                            container.Events.Find(x => x.EventId.Equals(i.fromNodeId)).ConditionReverseFrom.Add(toEvent);
                            container.Events.Find(x => x.EventId.Equals(i.fromNodeId)).ConditionReverseTo.Add(fromEvent);

                        }




                        //put milestones on events
                        foreach (var i in container.Milestones)
                        {

                            var fromEvent = container.Events.Find(x => x.EventId.Equals(i.fromNodeId));
                            var toEvent = container.Events.Find(x => x.EventId.Equals(i.toNodeId));

                            container.Events.Find(x => x.EventId.Equals(i.fromNodeId)).MilestoneReverseFrom.Add(toEvent);
                            container.Events.Find(x => x.EventId.Equals(i.fromNodeId)).MilestoneReverseTo.Add(fromEvent);

                        }
                        

                        foreach (var e in container.Events)
                        {
                            db.Entry(e).State = EntityState.Modified;
                        }
                        db.SaveChanges();
                        

                        scope.Complete();
                    }
                    catch (Exception ex)
                    {

                        
                        throw;
                    }
                    
                }

            }
        }
    
    }
    
}
