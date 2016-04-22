using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DROM_Client.Models.BusinessObjects;
using Database = WebAPI.Models.DBObjects.Database;
using System.Data.SqlClient;
using System.Data;
using System.Net.Http;
using WebAPI.Models.DBObjects;
using Group = DROM_Client.Models.BusinessObjects.Group;
using Role = DROM_Client.Models.BusinessObjects.Role;

namespace WebAPI.Models.DBMethods
{
    public class DbInteractions
    {

        public async Task<Tuple<List<DROM_Client.Models.BusinessObjects.Item>, string, HttpStatusCode>> GetItems()
        {
            try
            {


                using (var db = new Database())
                {
                    var items = db.Items
                            .Include(i => i.Category);
                    List<DROM_Client.Models.BusinessObjects.Item> itemList = new List<DROM_Client.Models.BusinessObjects.Item>();
                    foreach (var i in items)
                    {
                        var item = new DROM_Client.Models.BusinessObjects.Item()
                        {
                            Category = i.Category.Name,
                            Description = i.Description,
                            Id = i.Id,
                            Name = i.Name,
                            Price = i.Price
                        };
                        itemList.Add(item);
                    }
                    return new Tuple<List<DROM_Client.Models.BusinessObjects.Item>, string, HttpStatusCode>(itemList,
                        "Success", HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {

                return new Tuple<List<DROM_Client.Models.BusinessObjects.Item>, string, HttpStatusCode>(null,
                        ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Tuple<List<DROM_Client.Models.BusinessObjects.Order>, string, HttpStatusCode>> GetOrdersWithSortedEvents()
        {
            try
            {


                using (var db = new Database())
                {
                    //get non arcihved orders from database, with only relevant events.
                    //Projection is used to get the data, since filtering on child collections is not availeble in lazy and eager loading https://msdn.microsoft.com/en-us/magazine/hh205756.aspx

                    var query = (from o in db.Orders
                                 where o.Archived == false
                                 select
                                     new
                                     {
                                         Order = o,
                                         //child collections aren't loaded, and have to be slected seperately

                                         Graph = o.DCRGraph,
                                         Events = (from e in o.DCRGraph.DCREvents
                                                   where
                                                           (
                                                           e.Pending
                                                           && e.Included
                                                           && !e.Conditions.Any(
                                                               c => c.Included
                                                               && c.Executed == false)
                                                           && !e.Milestones.Any(
                                                               m => m.Included
                                                               && m.Pending)
                                                           )
                                                       ||
                                                           (
                                                           e.Groups.Any(g => g.Name == "Edit events")
                                                           && e.Included
                                                           )
                                                   select new
                                                   {
                                                       Event = e,
                                                       Groups = e.Groups,
                                                       Roles = e.Roles
                                                   }
                                         ),
                                         Customer = o.Customer,
                                         OrderDetails = o.OrderDetails,
                                         Items = (from od in o.OrderDetails
                                                  select new
                                                  {
                                                      Item = od.Item,
                                                      Category = od.Item.Category,
                                                      Quantity = od.Quantity
                                                  }
                                         ),
                                     });





                    //var query = from o in db.Orders
                    //            where o.Archived == false
                    //            select
                    //                new
                    //                {
                    //                    Order = o,
                    //                   //child collections aren't loaded, and have to be slected seperately
                    //                   Graph = o.DCRGraph,
                    //                    PendingEvents = o.DCRGraph.DCREvents.Where(
                    //                        e => e.Pending
                    //                        && e.Included
                    //                        && !e.Conditions.Any(
                    //                            c => c.Included
                    //                            && c.Executed == false)
                    //                        && !e.Milestones.Any(
                    //                            m => m.Included
                    //                            && m.Pending)
                    //                    ),


                    //                    PendingEventsGroups = o.DCRGraph.DCREvents.Where(
                    //                        e => e.Pending
                    //                        && e.Included).Select(e => e.Groups),
                    //                    PendingEventsRoles = o.DCRGraph.DCREvents.Where(e => e.Pending && e.Included).Select(e => e.Roles),
                    //                    EditEvents = o.DCRGraph.DCREvents.Where(e => e.Groups.Any(g => g.Name == "Edit events")).Select(e => e),
                    //                    EditEventsGroups = o.DCRGraph.DCREvents.Where(e => e.Groups.Any(g => g.Name == "Edit events")).Select(e => e.Groups),
                    //                    EditEventsRoles = o.DCRGraph.DCREvents.Where(e => e.Groups.Any(g => g.Name == "Edit events")).Select(e => e.Roles),
                    //                    Customer = o.Customer,
                    //                    OrderDetails = o.OrderDetails,
                    //                    Items = o.OrderDetails.Select(od => od.Item),
                    //                    Categories = o.OrderDetails.Select(od => od.Item).Select(i => i.Category)
                    //                };





                    var orders = new List<DROM_Client.Models.BusinessObjects.Order>();

                    //go through all the orders loaded from the database ad make DTOs
                    foreach (var queryOrder in query)
                    {
                        //Make a DTO order and set all the non collection type properties
                        var order = new DROM_Client.Models.BusinessObjects.Order()
                        {
                            Id = queryOrder.Order.Id,
                            Notes = queryOrder.Order.Notes,
                            OrderDate = queryOrder.Order.OrderDate,
                            OrderType = queryOrder.Order.OrderType,
                            Table = queryOrder.Order.Table,
                            AcceptingState = queryOrder.Graph.AcceptingState
                        };

                        //prepare a list to put reassembled events into, which will later be added to the DCRGraph
                        var events = new List<Event>();

                        foreach (var e in queryOrder.Events)
                        {
                            var assemblyEvent = new Event()
                            {
                                Description = e.Event.Description,
                                Executed = e.Event.Executed,
                                Id = e.Event.Id,
                                Included = e.Event.Included,
                                Label = e.Event.Label,
                                Pending = e.Event.Pending
                            };
                            assemblyEvent.Groups = new List<Group>();
                            foreach (var g in e.Groups)
                            {
                                assemblyEvent.Groups.Add(new Group()
                                {
                                    Id = g.Id,
                                    Name = g.Name
                                });
                            }
                            assemblyEvent.Roles = new List<Role>();
                            foreach (var r in e.Roles)
                            {
                                assemblyEvent.Roles.Add(new Role()
                                {
                                    Id = r.Id,
                                    Name = r.Name
                                });
                            }
                            events.Add(assemblyEvent);
                        }

                        //make the DCRGraph to be put onto the order, with all the newly assembled events in it
                        order.DCRGraph = new DROM_Client.Models.BusinessObjects.DCRGraph()
                        {
                            Id = queryOrder.Graph.Id,
                            Events = events
                        };


                        order.ItemsAndQuantity = new List<ItemQuantity>();
                        //put item, quantity and category together to form the DTO Item and quantity.
                        foreach (var i in queryOrder.Items)
                        {
                            order.ItemsAndQuantity.Add(new ItemQuantity()
                            {
                                Item = new DROM_Client.Models.BusinessObjects.Item()
                                {
                                    Id = i.Item.Id,
                                    Description = i.Item.Description,
                                    Price = i.Item.Price,
                                    Name = i.Item.Name,
                                    Category = i.Category.Name
                                },
                                Quantity = i.Quantity

                            });
                        }

                        //If there is a customer, include it.
                        if (order.Customer != null)
                        {
                            //map customer to DTO Customer
                            order.Customer = new DROM_Client.Models.BusinessObjects.Customer()
                            {
                                Id = queryOrder.Customer.Id,
                                City = queryOrder.Customer.City,
                                Phone = queryOrder.Customer.Phone,
                                ZipCode = queryOrder.Customer.Zipcode,
                                Email = queryOrder.Customer.Email,
                                StreetAndNumber = queryOrder.Customer.StreetAndNumber,
                                LastName = queryOrder.Customer.LastName,
                                FirstAndMiddleNames = queryOrder.Customer.FirstName
                            };
                        }

                        orders.Add(order);
                    }

                    return new Tuple<List<DROM_Client.Models.BusinessObjects.Order>, string, HttpStatusCode>(orders,
                        "Success", HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                return new Tuple<List<DROM_Client.Models.BusinessObjects.Order>, string, HttpStatusCode>(null,
                        ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Tuple<string, HttpStatusCode>> UpdateOrder(Tuple<DROM_Client.Models.BusinessObjects.Order, List<int>> data)
        {
            try
            {


                using (var db = new Database())
                {
                    foreach (var i in data.Item2)
                    {
                        var status = await this.ExecuteEvent(i);
                        if (status.Item2 != HttpStatusCode.OK) return status; //Preconditions were not meet
                    }

                    var orderToBeUpdated = await db.Orders
                        .Include(o => o.OrderDetails.Select(od => od.Item))
                        .Include(o => o.Customer)
                        .FirstOrDefaultAsync(o => o.Id == data.Item1.Id);

                    //update related customer
                    if (data.Item1.OrderType != "For serving")
                    {
                        if (orderToBeUpdated.Customer == null)
                        {
                            orderToBeUpdated.Customer = new DBObjects.Customer()
                            {
                                City = data.Item1.Customer.City,
                                Email = data.Item1.Customer.Email,
                                FirstName = data.Item1.Customer.FirstAndMiddleNames,
                                LastName = data.Item1.Customer.LastName,
                                Phone = data.Item1.Customer.Phone,
                                StreetAndNumber = data.Item1.Customer.StreetAndNumber,
                                Zipcode = data.Item1.Customer.ZipCode
                            };
                            db.Entry(orderToBeUpdated.Customer).State = EntityState.Added;

                        }
                        else
                        {
                            orderToBeUpdated.Customer.City = data.Item1.Customer.City;
                            orderToBeUpdated.Customer.Email = data.Item1.Customer.Email;
                            orderToBeUpdated.Customer.FirstName = data.Item1.Customer.FirstAndMiddleNames;
                            orderToBeUpdated.Customer.LastName = data.Item1.Customer.LastName;
                            orderToBeUpdated.Customer.Phone = data.Item1.Customer.Phone;
                            orderToBeUpdated.Customer.StreetAndNumber = data.Item1.Customer.StreetAndNumber;
                            orderToBeUpdated.Customer.Zipcode = data.Item1.Customer.ZipCode;
                        }
                        
                        
                    }

                    //update the order
                    orderToBeUpdated.Notes = data.Item1.Notes;
                    orderToBeUpdated.Table = data.Item1.Table;
                    orderToBeUpdated.OrderType = data.Item1.OrderType;
                    //data.Item1.ItemsAndQuantity = data.Item1.ItemsAndQuantity;
                    db.OrderDetails.RemoveRange(orderToBeUpdated.OrderDetails);
                    var newOrderDetails = new HashSet<OrderDetail>();
                    foreach (var iq in data.Item1.ItemsAndQuantity)
                    {

                        var item = await db.Items.FirstOrDefaultAsync(i => i.Id == iq.Item.Id);
                        var order = await db.Orders.FirstOrDefaultAsync(o => o.Id == data.Item1.Id);
                        newOrderDetails.Add(new OrderDetail()
                        {
                            Item = item,
                            ItemId = item.Id,
                            Order = order,
                            OrderId = order.Id,
                            Quantity = iq.Quantity
                        });
                    }

                    db.OrderDetails.AddRange(orderToBeUpdated.OrderDetails);

                    orderToBeUpdated.OrderDetails = newOrderDetails;

                    db.Entry(orderToBeUpdated).State = EntityState.Modified;
                    db.Entry(orderToBeUpdated.Customer).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return new Tuple<string, HttpStatusCode>("Success", HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                return new Tuple<string, HttpStatusCode>(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Tuple<string, HttpStatusCode>> ExecuteEvent(int id)
        {
            try
            {
                using (var db = new Database())
                {
                    var eventToBeExecuted = await db.DCREvents
                        .Include(e => e.Groups)
                        .Include(e => e.Roles)
                        .Include(e => e.Conditions)
                        .Include(e => e.Excludes)
                        .Include(e => e.Includes)
                        .Include(e => e.Responses)
                        .Include(e => e.Milestones)
                        .FirstOrDefaultAsync(e => e.Id == id);


                    //preconditions:
                    //the event must be included
                    if (eventToBeExecuted.Included == false) return new Tuple<string, HttpStatusCode>("Trying to execute excluded event", HttpStatusCode.InternalServerError);

                    //check if conditions are executed
                    foreach (var condition in eventToBeExecuted.Conditions)
                    {
                        if (condition.Executed == false && condition.Included) return new Tuple<string, HttpStatusCode>("A condition is not executed", HttpStatusCode.InternalServerError);
                    }

                    //there must not be a pending milestone
                    foreach (var milestone in eventToBeExecuted.Milestones)
                    {
                        var mEvent = await db.DCREvents.FirstOrDefaultAsync(e => e.Id == milestone.Id);

                        if (milestone.Pending && milestone.Included) return new Tuple<string, HttpStatusCode>("There is a pending milestone", HttpStatusCode.InternalServerError);
                    }

                    //Preconditions have succeded!

                    //Setup postconditions:
                    eventToBeExecuted.Pending = false;
                    eventToBeExecuted.Executed = true;

                    //exclude related events
                    foreach (var e in eventToBeExecuted.Excludes)
                    {
                        e.Included = false;
                    }

                    //Include related events
                    foreach (var e in eventToBeExecuted.Includes)
                    {
                        e.Included = true;
                    }

                    //set related events pending
                    foreach (var e in eventToBeExecuted.Responses)
                    {
                        e.Pending = true;
                    }

                    //set state to modified and save.
                    db.Entry(eventToBeExecuted).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    //get the modified order from the database to check whether it has gone into accepting state
                    var order = await (from o in db.Orders
                                       .Include(o => o.DCRGraph.DCREvents)
                                       where o.DCRGraph.Id == eventToBeExecuted.DCRGraphId
                                       select o).FirstOrDefaultAsync();

                    if (order.DCRGraph.DCREvents.Any(dcrEvent => dcrEvent.Included && dcrEvent.Pending))
                    {
                        return new Tuple<string, HttpStatusCode>("Success but not accepting state", HttpStatusCode.OK);
                    }


                    order.DCRGraph.AcceptingState = true;
                    db.Entry(order.DCRGraph).State = EntityState.Modified;
                    db.Entry(order).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    return new Tuple<string, HttpStatusCode>("Success and accepting state", HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {

                return new Tuple<string, HttpStatusCode>(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Tuple<List<string>, string, HttpStatusCode>> DeliveryTypes(int orderType)
        {
            try
            {


                using (var db = new Database())
                {
                    var deliveryTypes = db.DeliveryTypes.Where(dt => dt.OrderType == orderType);
                    var result = new List<string>();
                    foreach (var dt in deliveryTypes)
                    {
                        result.Add(dt.Type);
                    }
                    return new Tuple<List<string>, string, HttpStatusCode>(result, "Success", HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {

                return new Tuple<List<string>, string, HttpStatusCode>(null,
                        ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Tuple<string, HttpStatusCode>> AchiveOrder(int order)
        {
            try
            {
                using (var db = new Database())
                {
                    var orderToBeArchived = await db.Orders.FindAsync(order);
                    if (orderToBeArchived == null) return new Tuple<string, HttpStatusCode>("The order did not exist in the Database", HttpStatusCode.InternalServerError);
                    orderToBeArchived.Archived = true;
                    db.Entry(orderToBeArchived).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return new Tuple<string, HttpStatusCode>("Success", HttpStatusCode.OK);

                }
            }
            catch (Exception ex)
            {
                return new Tuple<string, HttpStatusCode>(
                        ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        ////get orders from db
        //var orders = db.Orders
        //    .Include(o => o.DCRGraph.DCREvents.Select(e => e.Groups))
        //    .Include(o => o.DCRGraph.DCREvents.Select(e => e.Roles))
        //    .Include(o => o.Customer)
        //    .Include(o => o.OrderDetails.Select(od => od.Item.Category))
        //    ;








        ////var orders = db.Orders;
        //List<DROM_Client.Models.BusinessObjects.Order> orderList = new List<DROM_Client.Models.BusinessObjects.Order>();

        ////convert db orders to serializable transfer orders
        //foreach (var o in orders)
        //{

        //    var order = new DROM_Client.Models.BusinessObjects.Order()
        //    {
        //        Id = o.Id,
        //        OrderType = o.OrderType,
        //        Notes = o.Notes ?? "",
        //        OrderDate = o.OrderDate,
        //        Table = o.Table
        //    };

        //    //if db customer not null, put it on the order, otherwise attach an empty customer to the order
        //    if (o.Customer != null)
        //    {
        //        var customer = new DROM_Client.Models.BusinessObjects.Customer()
        //        {
        //            Id = o.Customer.Id,
        //            Phone = o.Customer.Phone,
        //            ZipCode = o.Customer.Zipcode,
        //            StreetAndNumber = o.Customer.StreetAndNumber,
        //            City = o.Customer.City,
        //            Email = o.Customer.Email,
        //            FirstAndMiddleNames = o.Customer.FirstName,
        //            LastName = o.Customer.LastName

        //        };
        //        order.Customer = customer;
        //    }
        //    else order.Customer = new DROM_Client.Models.BusinessObjects.Customer();

        //    //attach DCRGraph to the order
        //    order.DCRGraph = new DROM_Client.Models.BusinessObjects.DCRGraph()
        //    {
        //        Id = o.DCRGraph.Id,
        //        Events = new List<Event>()
        //    };

        //    //convert events and attach them to the graph
        //    foreach (var dcrEvent in o.DCRGraph.DCREvents)
        //    {
        //        if (dcrEvent.Included == true) // only add included events
        //        {
        //            foreach (var g in dcrEvent.Groups)
        //            {
        //                if ((dcrEvent.Pending == true || g.Name == "Edit events")) // we only want to give pending events and edit events
        //                {
        //                    var businessEvent = new Event
        //                    {
        //                        Id = dcrEvent.Id,
        //                        Description = dcrEvent.Description ?? "",
        //                        Executed = dcrEvent.Executed,
        //                        Included = dcrEvent.Included,
        //                        Label = dcrEvent.Label,
        //                        Pending = dcrEvent.Pending,
        //                        Roles = null,
        //                        Groups = new List<DROM_Client.Models.BusinessObjects.Group>()
        //                    };

        //                    //get groups onto the event
        //                    foreach (var groupList in dcrEvent.Groups)
        //                    {
        //                        var group = new DROM_Client.Models.BusinessObjects.Group();
        //                        group.Id = groupList.Id;
        //                        group.Name = groupList.Name;
        //                        businessEvent.Groups.Add(group);
        //                    }

        //                    //get roles onto the event
        //                    businessEvent.Roles = new List<DROM_Client.Models.BusinessObjects.Role>();
        //                    foreach (var r in dcrEvent.Roles)
        //                    {
        //                        var role = new DROM_Client.Models.BusinessObjects.Role();
        //                        role.Id = r.Id;
        //                        role.Name = r.Name;
        //                        businessEvent.Roles.Add(role);
        //                    }

        //                    order.DCRGraph.Events.Add(businessEvent);
        //                }
        //            }
        //        }

        //    }

        //    //put items and quantity on the order
        //    var itemsAndQuantity = new List<DROM_Client.Models.BusinessObjects.ItemQuantity>();
        //    foreach (var od in o.OrderDetails)
        //    {
        //        var itemQuantity = new ItemQuantity()
        //        {
        //            Item = new DROM_Client.Models.BusinessObjects.Item()
        //            {
        //                Id = od.Item.Id,
        //                Category = od.Item.Category.Name,
        //                Name = od.Item.Name,
        //                Description = od.Item.Description,
        //                Price = od.Item.Price

        //            },
        //            Quantity = od.Quantity

        //        };



        //        itemsAndQuantity.Add(itemQuantity);
        //    }
        //    order.ItemsAndQuantity = itemsAndQuantity;
    }
}
