using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.XMLParser;
using DROM_Client.Models.NewOrderData;
using WebAPI.Models.DBObjects;
using System.Data.Entity;
using System.Net;
using WebAPI.Models.DBMethods;


namespace WebAPI.Models.Parsing
{
    class Mapper
    {

        /// <summary>
        /// Method to create new orders in the database.
        /// It will create a new order with the information it receives and with the DCRGraph which is currently in the xml file which the API reads from.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="orderInfo"></param>
        /// <returns></returns>
        public async Task<Tuple<string, HttpStatusCode>> CreateOrder(EventAndRolesContainer container, NewOrderInfo orderInfo)
        {
            using (var db = new WebAPI.Models.DBObjects.Database())
            {
                try
                {
                    //setup a new dcrgraph
                    var graph = new DCRGraph
                    {
                        AcceptingState = false,
                        Lock = false,
                        LockTime = DateTime.Now,
                        DCREvents = container.Events,
                    };

                    //setup a new order
                    var order = new Order()
                    {
                        DCRGraph = graph,
                        OrderDate = orderInfo.OrderDate,
                        Notes = orderInfo.Notes,
                        Table = orderInfo.Table,
                        OrderDetails = new List<OrderDetail>(),
                        OrderType = orderInfo.OrderType,
                        RestaurantId = orderInfo.Restaurant
                    };

                    //put items on the order. Can only put items that exists in the database on.
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



                    //Determine if there should be a customer on the order. We do not want cutstomers with the phone number 0.
                    if(orderInfo.Customer.Phone != 0)
                    {
                        //see if we already have customer with the same phone number, otherwise create a new one.
                        var customer =
                            await db.Customers
                                .FirstOrDefaultAsync(c => c.Phone == orderInfo.Customer.Phone) ?? new Customer
                                {
                                    City = orderInfo.Customer.City ?? "n/a",
                                    Email = orderInfo.Customer.Email ?? "n/a",
                                    FirstName = orderInfo.Customer.FirstAndMiddleNames ?? "n/a",
                                    LastName = orderInfo.Customer.LastName ?? "n/a",
                                    Phone = orderInfo.Customer.Phone,
                                    StreetAndNumber = orderInfo.Customer.StreetAndNumber ?? "n/a",
                                    Zipcode = orderInfo.Customer.ZipCode,
                                    Orders = new HashSet<Order> {order}
                                };

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
                    await db.SaveChangesAsync();

                    //needs statuscode exception handling
                    DCREvent dcrEvent;
                    switch (orderInfo.OrderType)
                    {
                        case "For serving":
                            dcrEvent = order.DCRGraph.DCREvents.FirstOrDefault(e => e.Label == "Setup graph serving");
                            if (dcrEvent != null)
                            {
                                await new DbInteractions().ExecuteEvent(dcrEvent.Id, false);
                                break;
                            }
                            return new Tuple<string, HttpStatusCode>("The DCRGraph does not contain the relvant setup event",
                            HttpStatusCode.InternalServerError);
                        case "For takeaway":
                            dcrEvent = order.DCRGraph.DCREvents.FirstOrDefault(e => e.Label == "Setup graph takeaway");
                            if (dcrEvent != null)
                            {
                                await new DbInteractions().ExecuteEvent(dcrEvent.Id, false);
                                break;
                            }

                            return new Tuple<string, HttpStatusCode>("The DCRGraph does not contain the relvant setup event",
                            HttpStatusCode.InternalServerError);
                        case "For delivery":
                            dcrEvent = order.DCRGraph.DCREvents.FirstOrDefault(e => e.Label.Contains("Setup graph delivery"));
                            if (dcrEvent != null)
                            {
                                await new DbInteractions().ExecuteEvent(dcrEvent.Id, false);
                                break;
                            }
                            return new Tuple<string, HttpStatusCode>("The DCRGraph does not contain the relvant setup event",
                            HttpStatusCode.InternalServerError);

                        case "Bulk order":
                            dcrEvent = order.DCRGraph.DCREvents.FirstOrDefault(e => e.Label.Contains("Setup bulk order"));
                            if (dcrEvent != null)
                            {
                                await new DbInteractions().ExecuteEvent(dcrEvent.Id, false);
                                break;
                            }
                            return new Tuple<string, HttpStatusCode>("The DCRGraph does not contain the relvant setup event",
                            HttpStatusCode.InternalServerError);

                        default:
                            return new Tuple<string, HttpStatusCode>("ordertype id not match - " + orderInfo.OrderType,
                                HttpStatusCode.InternalServerError);
                    }




                    //scope.Complete();
                    return new Tuple<string, HttpStatusCode>("success", HttpStatusCode.OK);

                }
                catch (Exception ex)
                {


                    return new Tuple<string, HttpStatusCode>(ex.Message, HttpStatusCode.InternalServerError);
                }

                //}

            }


        }
    }

}
