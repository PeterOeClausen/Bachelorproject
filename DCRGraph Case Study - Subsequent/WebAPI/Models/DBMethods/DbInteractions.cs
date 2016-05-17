using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DROM_Client.Models.BusinessObjects;
using Database = WebAPI.Models.DBObjects.Database; //ease of reference
using WebAPI.Models.DBObjects;
using Group = DROM_Client.Models.BusinessObjects.Group;  //ease of reference
using Role = DROM_Client.Models.BusinessObjects.Role;  //ease of reference
using DBO = WebAPI.Models.DBObjects;

namespace WebAPI.Models.DBMethods
{
    public class DbInteractions
    {

        /// <summary>
        /// Medhod to get items from the database.
        /// </summary>
        /// <returns></returns>
        public async Task<Tuple<List<DROM_Client.Models.BusinessObjects.Item>, string, HttpStatusCode>> GetItems()
        {
            try
            {


                using (var db = new Database())
                {
                    var items = await db.Items
                            .Include(i => i.Category).ToListAsync();
                    
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

        /// <summary>
        /// Method to find orders in the database, while filtering events so that only pending and edit events are loaded.
        /// </summary>
        /// <param name="restaurant"></param>
        /// <returns></returns>
        public async Task<Tuple<List<DROM_Client.Models.BusinessObjects.Order>, string, HttpStatusCode>> GetOrdersWithSortedEvents(int restaurant)
        {
            try
            {


                using (var db = new Database())
                {
                    //get non arcihved orders from database, with only relevant events.
                    //Projection is used to get the data, since filtering on child collections is not availeble in eager loading https://msdn.microsoft.com/en-us/magazine/hh205756.aspx

                    var query = await (from o in db.Orders
                                 where o.Archived == false
                                 where o.RestaurantId == restaurant
                                 select
                                     new
                                     {
                                         Order = o,
                                         //child collections aren't loaded, and have to be selected seperately

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
                                     }).ToListAsync();
                    
                    
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
                            AcceptingState = queryOrder.Graph.AcceptingState,
                            Restaurant = restaurant
                        };
                        
                        var events = new List<Event>();

                        //Assemble events - meaning put the disjointed information together in one object.
                        foreach (var e in queryOrder.Events)
                        {
                            var assemblyEvent = new Event
                            {
                                Description = e.Event.Description,
                                Executed = e.Event.Executed,
                                Id = e.Event.Id,
                                Included = e.Event.Included,
                                Label = e.Event.Label,
                                Pending = e.Event.Pending,
                                Groups = new List<Group>()
                            };
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

                        //make the DCRGraph, to be put onto the order, with all the newly assembled events in it
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
                        if (queryOrder.Customer != null)
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

        /// <summary>
        /// Method to update an order in the database. 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<Tuple<string, HttpStatusCode>> UpdateOrder(Tuple<DROM_Client.Models.BusinessObjects.Order, List<int>> data)
        {
            try
            {


                using (var db = new Database())
                {
                    
                    

                    //try to lock the order
                    var guid = Guid.NewGuid();
                    var tryLock = await this.LockGraph(guid, data.Item1.DCRGraph.Id, db);
                    if (tryLock.Item1 == false)
                        return new Tuple<string, HttpStatusCode>(tryLock.Item2, HttpStatusCode.InternalServerError);

                    //Execute edit events
                    foreach (var i in data.Item2)
                    {
                        var status = await this.ExecuteEvent(i, true);
                        if (status.Item2 != HttpStatusCode.OK)
                            return status; //Preconditions were not meet
                    }

                    var orderToBeUpdated = await db.Orders
                        .Include(o => o.OrderDetails.Select(od => od.Item))
                        .Include(o => o.Customer)
                        .FirstOrDefaultAsync(o => o.Id == data.Item1.Id);


                    //update related customer - it is not possible to have the phone number 0.

                    if (data.Item1.Customer.Phone != 0)
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

                    //Check if we have a lock before saving to db
                    var checkLock = await this.CheckLock(guid, data.Item1.DCRGraph.Id, db);
                    if (checkLock.Item1 == false)
                        return new Tuple<string, HttpStatusCode>(tryLock.Item2, HttpStatusCode.InternalServerError);

                    //we have a lock! Saving to db
                    db.Entry(orderToBeUpdated).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    //unlock after are done saving
                    var unlock = await this.Unlock(guid, data.Item1.DCRGraph.Id, db);
                    if (unlock.Item1 == false)
                        return new Tuple<string, HttpStatusCode>(tryLock.Item2, HttpStatusCode.InternalServerError);

                    return new Tuple<string, HttpStatusCode>("Success", HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                return new Tuple<string, HttpStatusCode>(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Medthod to execute an event in the database. Will only execute the event if its relations allow it.
        /// If the execution results in the DCRGraph which the event belongs to, entered or exiting acceting state, the state will be updated accordingly.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Tuple<string, HttpStatusCode>> ExecuteEvent(int id, bool preLocked)
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

                    //Better see if we can lock before we do anything else.
                    //If we were called by an update order, the order is already locked for us.
                    var guid = Guid.NewGuid();
                    Tuple<bool, string> tryLock = null;
                    if (!preLocked)
                    {
                        tryLock = await this.LockGraph(guid, eventToBeExecuted.DCRGraphId, db);
                        if (tryLock.Item1 == false)
                            return new Tuple<string, HttpStatusCode>(tryLock.Item2, HttpStatusCode.InternalServerError);
                    }

                    
                   


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

                    if (!preLocked)
                    {
                        var checkLock = await this.CheckLock(guid, eventToBeExecuted.DCRGraphId, db);
                        if (checkLock.Item1 == false)
                            return new Tuple<string, HttpStatusCode>(tryLock.Item2, HttpStatusCode.InternalServerError);
                    }
                    

                    await db.SaveChangesAsync();

                    

                    //get the modified order from the database to check whether it has gone into accepting state
                    var order = await (from o in db.Orders
                                       .Include(o => o.DCRGraph.DCREvents)
                                       where o.DCRGraph.Id == eventToBeExecuted.DCRGraphId
                                       select o).FirstOrDefaultAsync();

                    //remove accepting state if we have left it.
                    if (order.DCRGraph.DCREvents.Any(dcrEvent => dcrEvent.Included && dcrEvent.Pending))
                    {

                        if (order.DCRGraph.AcceptingState)
                        {
                            order.DCRGraph.AcceptingState = false;
                            db.Entry(order.DCRGraph).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                        }

                        if (!preLocked)
                        {
                            var unlock1 = await this.Unlock(guid, eventToBeExecuted.DCRGraphId, db);
                            if (unlock1.Item1 == false)
                                return new Tuple<string, HttpStatusCode>(tryLock.Item2, HttpStatusCode.InternalServerError);
                        }
                        
                        return new Tuple<string, HttpStatusCode>("Success but not accepting state", HttpStatusCode.OK);
                    }

                    //if already accepting, leave it as is. 
                    if (order.DCRGraph.AcceptingState)
                    {
                        if (!preLocked)
                        {
                            var unlock2 = await this.Unlock(guid, eventToBeExecuted.DCRGraphId, db);
                            if (unlock2.Item1 == false)
                                return new Tuple<string, HttpStatusCode>(tryLock.Item2, HttpStatusCode.InternalServerError);
                        }

                        return new Tuple<string, HttpStatusCode>("Success and accepting state", HttpStatusCode.OK);
                    }

                    //if not already accepting, set accepting
                    order.DCRGraph.AcceptingState = true;
                    db.Entry(order.DCRGraph).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    if (!preLocked)
                    {
                        var unlock2 = await this.Unlock(guid, eventToBeExecuted.DCRGraphId, db);
                        if (unlock2.Item1 == false)
                            return new Tuple<string, HttpStatusCode>(tryLock.Item2, HttpStatusCode.InternalServerError);
                    }
                    
                    return new Tuple<string, HttpStatusCode>("Success and accepting state", HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                return new Tuple<string, HttpStatusCode>(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Method to get delivery types from the database.
        /// </summary>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public async Task<Tuple<List<string>, string, HttpStatusCode>> DeliveryTypes(int orderType)
        {
            try
            {
                using (var db = new Database())
                {
                    var deliveryTypes = await db.DeliveryTypes.Where(dt => dt.OrderType == orderType).ToListAsync();

                    //Get the delivery type names from the objects.
                    var result = deliveryTypes.Select(dt => dt.Type).ToList();

                    return new Tuple<List<string>, string, HttpStatusCode>(result, "Success", HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                return new Tuple<List<string>, string, HttpStatusCode>(null,
                        ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Method to archive an order in the database.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public async Task<Tuple<string, HttpStatusCode>> AchiveOrder(int order)
        {
            try
            {
                using (var db = new Database())
                {

                    var orderToBeArchived = await db.Orders
                        .Include(o => o.DCRGraph)
                        .FirstOrDefaultAsync(o => o.Id == order);

                    //this order did not exist
                    if (orderToBeArchived == null)
                        return new Tuple<string, HttpStatusCode>("The order did not exist in the Database", HttpStatusCode.InternalServerError);

                    //the order exists, better lock before we change anything.
                    var guid = Guid.NewGuid();
                    var tryLock = await this.LockGraph(guid, orderToBeArchived.DCRGraph.Id, db);
                    if (tryLock.Item1 == false)
                        return new Tuple<string, HttpStatusCode>(tryLock.Item2, HttpStatusCode.InternalServerError);

                    //change to archived
                    orderToBeArchived.Archived = true;
                    db.Entry(orderToBeArchived).State = EntityState.Modified;

                    //lets be absolutely sure we have the lock before we change the db
                    var checkLock = await this.CheckLock(guid, orderToBeArchived.DCRGraph.Id, db);
                    if (checkLock.Item1 == false)
                        return new Tuple<string, HttpStatusCode>(tryLock.Item2, HttpStatusCode.InternalServerError);

                    //save to db
                    await db.SaveChangesAsync();

                    //we're done, better unlock
                    var unlock = await this.Unlock(guid, orderToBeArchived.DCRGraph.Id, db);
                    if (unlock.Item1 == false)
                        return new Tuple<string, HttpStatusCode>(tryLock.Item2, HttpStatusCode.InternalServerError);

                    return new Tuple<string, HttpStatusCode>("Success", HttpStatusCode.OK);

                }
            }
            catch (Exception ex)
            {
                return new Tuple<string, HttpStatusCode>(
                        ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Method to lock a DCRGraph in the database. This is a helper method which is used by all methods which change data in the database.
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="graphId"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public async Task<Tuple<bool, string>> LockGraph(Guid guid, int graphId, DBO.Database db)
        {
            var graph = await db.DCRGraphs.FindAsync(graphId);
            //check if the graph exist
            if(graph == null) return new Tuple<bool, string>(false,"The graph did not exist");

            //Check that the graph is not locked, and if it is, check whether it's an old lock. Locks older than 1minute we do no care about.
            if(graph.Lock && DateTime.Now.Subtract(graph.LockTime).Minutes < 1 ) return new Tuple<bool, string>(false, "The graph is already locked");

            //lock this 
            graph.Lock = true;
            graph.LockTime = DateTime.Now;
            graph.Guid = guid;

            db.Entry(graph).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return new Tuple<bool, string>(true, "");


            
        }

        /// <summary>
        /// Method to check if a DCRGraph in the database is locked. This is a helper method which is used by all methods which change data in the database.
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="graphId"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public async Task<Tuple<bool, string>> CheckLock(Guid guid, int graphId, DBO.Database db)
        {
            
            var graph = await db.DCRGraphs.FindAsync(graphId);

            if (graph == null) return new Tuple<bool, string>(false, "The graph did not exist");

            if (graph.Lock == false)
                return new Tuple<bool, string>(false, "The graph was not locked when reaching the check lock phase");

            if (graph.Guid != guid)
                return new Tuple<bool, string>(false, "someone else has locked the graph.");

            return new Tuple<bool, string>(true, "");
            
        }

        /// <summary>
        /// Method to unlock a DCRGraph in the database. This is a helper method which is used by all methods which change data in the database.
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="graphId"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public async Task<Tuple<bool, string>> Unlock(Guid guid, int graphId, DBO.Database db)
        {
            
            var graph = await db.DCRGraphs.FindAsync(graphId);

            if (graph == null) return new Tuple<bool, string>(false, "The graph did not exist");

            if (graph.Lock == false)
                return new Tuple<bool, string>(false, "The graph was not locked when reaching the unlock phase");

            if (graph.Guid != guid)
                return new Tuple<bool, string>(false, "someone else has locked the graph. Should not be possible.");

            //unlock and save to db.
            graph.Lock = false;

            db.Entry(graph).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return new Tuple<bool, string>(true, "");
            
        }

    }
}
