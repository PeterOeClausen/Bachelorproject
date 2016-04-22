using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WebAPI.XMLParser;
using DROM_Client.Models.NewOrderData;
using WebAPI.Models.DBObjects;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Net;
using WebAPI.Models.DBMethods;


namespace WebAPI.Models.Parsing
{
    class Mapper
    {

        public async Task<HttpStatusCode> mapper(EventAndRolesContainer container, NewOrderInfo orderInfo)
        {
            using (var db = new WebAPI.Models.DBObjects.Database())
            {


                //using (TransactionScope scope = new TransactionScope())
                //{
                try
                {
                    var graph = new DCRGraph()
                    {
                        AcceptingState = false,
                    };
                    graph.DCREvents = container.Events;

                    var order = new Order()
                    {

                        DCRGraph = graph,
                        OrderDate = orderInfo.OrderDate,
                        Notes = orderInfo.Notes,
                        Table = orderInfo.Table,
                        OrderDetails = new List<OrderDetail>(),
                        OrderType = orderInfo.OrderType,
                        
                        

                    };
                    foreach (var iq in orderInfo.ItemsAndQuantity)
                    {

                        var item =
                            db.Items
                                .FirstOrDefaultAsync(i => i.Name == iq.Key.Name).Result;
                        if (!iq.Key.Name.ToLower().Equals(item.Name.ToLower()))
                        {
                            throw new Exception("Item '" + iq.Key.Name + "' did not exist in the database");
                        }

                        order.OrderDetails.Add(
                            new OrderDetail()
                            {
                                ItemId = item.Id,
                                Item = item,
                                Order = order,
                                Quantity = iq.Value
                            });

                    }




                    //Determine if there should be a customer on the order

                    if (orderInfo.OrderType != "For serving")
                    {
                        var customer = 
                            await db.Customers
                                            .FirstOrDefaultAsync(c => c.Phone == orderInfo.Customer.Phone);



                        if (customer == null)
                        {
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
                        container.Events.Find(e => e.Id == fromEvent.Id).Includes.Add(toEvent);
                        //InsertBySqlQuery(fromEvent.Id, toEvent.Id, "Includes");

                    }

                    //put exclusions on events
                    foreach (var i in container.Exclusions)
                    {
                        var fromEvent = container.Events.Find(x => x.EventId.Equals(i.fromNodeId));
                        var toEvent = container.Events.Find(x => x.EventId.Equals(i.toNodeId));
                        container.Events.Find(e => e.Id == fromEvent.Id).Excludes.Add(toEvent);
                        //InsertBySqlQuery(fromEvent.Id, toEvent.Id, "Excludes");


                    }

                    //put responses on events
                    foreach (var i in container.Responses)
                    {
                        var fromEvent = container.Events.Find(x => x.EventId.Equals(i.fromNodeId));
                        var toEvent = container.Events.Find(x => x.EventId.Equals(i.toNodeId));
                        container.Events.Find(e => e.Id == fromEvent.Id).Responses.Add(toEvent);
                        //InsertBySqlQuery(fromEvent.Id, toEvent.Id, "Milestones");
                    }

                    //put conditions on events
                    foreach (var i in container.Conditions)
                    {
                        var fromEvent = container.Events.Find(x => x.EventId.Equals(i.fromNodeId));
                        var toEvent = container.Events.Find(x => x.EventId.Equals(i.toNodeId));
                        container.Events.Find(e => e.Id == fromEvent.Id).Conditions.Add(toEvent);
                        //InsertBySqlQuery(fromEvent.Id, toEvent.Id, "Conditions");
                    }




                    //put milestones on events
                    foreach (var i in container.Milestones)
                    {
                        var fromEvent = container.Events.Find(x => x.EventId.Equals(i.fromNodeId));
                        var toEvent = container.Events.Find(x => x.EventId.Equals(i.toNodeId));
                        container.Events.Find(e => e.Id == fromEvent.Id).Milestones.Add(toEvent);
                        //InsertBySqlQuery(fromEvent.Id, toEvent.Id, "Milestones");


                    }



                    foreach (var e in container.Events)
                    {
                        db.Entry(e).State = EntityState.Modified;
                    }
                    db.SaveChanges();

                    //needs statuscode exception handling
                    switch (orderInfo.OrderType)
                    {
                        case "For serving":
                            var na = await new DbInteractions().ExecuteEvent(
                                    order.DCRGraph.DCREvents.FirstOrDefault(e => e.Label == "Setup graph serving").Id);
                            var stopwe = 5;
                            break;
                        case "For takeaway":
                            var na1 = await new DbInteractions().ExecuteEvent(
                                    order.DCRGraph.DCREvents.FirstOrDefault(e => e.Label == "Setup graph takeaway").Id);
                            var stop1 = 1;
                            break;
                        case "For delivery":
                            var na2 = await new DbInteractions().ExecuteEvent(
                                    order.DCRGraph.DCREvents.FirstOrDefault(e => e.Label.Contains("Setup graph delivery")).Id);
                            var stop = 5;
                            break;
                        default:
                            throw new Exception("ordertype id not match - " + orderInfo.OrderType);
                    }




                    //scope.Complete();
                    return HttpStatusCode.OK;

                }
                catch (Exception ex)
                {


                    throw;
                }

                //}

            }


        }
    }

}
