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
using WebAPI.Models.DBObjects;
using Group = DROM_Client.Models.BusinessObjects.Group;
using Role = DROM_Client.Models.BusinessObjects.Role;

namespace WebAPI.Models.DBMethods
{
    public class DbInteractions
    {

        public async Task<List<DROM_Client.Models.BusinessObjects.Item>> GetItems()
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

                    return itemList;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<List<DROM_Client.Models.BusinessObjects.Order>> GetOrdersWithSortedEvents()
        {
            try
            {


                using (var db = new Database())
                {
                    //get non arcihved orders from database, with only relevant events.
                    //Projection is used to get the data, since filtering on child collections is not availeble in lazy and eager loading https://msdn.microsoft.com/en-us/magazine/hh205756.aspx
                    var query = from o in db.Orders
                               where o.Archived == false
                               select
                                   new
                                   {
                                       Order = o,
                                       //child collections aren't loaded, and have to be slected seperately
                                       Graph = o.DCRGraph,
                                       PendingEvents = o.DCRGraph.DCREvents.Where(e => e.Pending && e.Included),
                                       PendingEventsGroups = o.DCRGraph.DCREvents.Where(e => e.Pending && e.Included).Select(e => e.Groups),
                                       PendingEventsRoles = o.DCRGraph.DCREvents.Where(e => e.Pending && e.Included).Select(e => e.Roles),
                                       EditEvents = o.DCRGraph.DCREvents.Where(e => e.Groups.Any(g => g.Name == "Edit events")).Select(e => e),
                                       EditEventsGroups = o.DCRGraph.DCREvents.Where(e => e.Groups.Any(g => g.Name == "Edit events")).Select(e => e.Groups),
                                       EditEventsRoles = o.DCRGraph.DCREvents.Where(e => e.Groups.Any(g => g.Name == "Edit events")).Select(e => e.Roles),
                                       Customer = o.Customer,
                                       OrderDetails = o.OrderDetails,
                                       Items = o.OrderDetails.Select(od => od.Item),
                                       Categories = o.OrderDetails.Select(od => od.Item).Select(i => i.Category)
                                   };

                    var orders = new List<DROM_Client.Models.BusinessObjects.Order>();

                    //go through all the orders loaded form the database
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

                        //put edit events back together with groups and roles, and add them to the order
                        for (int c = 0; c < queryOrder.EditEvents.Count(); c++)
                        {
                            //create the event and set all non collection type properties
                            var eventToBeAdded = new Event()
                            {
                                Description = queryOrder.EditEvents.ElementAt(c).Description,
                                Executed = queryOrder.EditEvents.ElementAt(c).Executed,
                                Included = queryOrder.EditEvents.ElementAt(c).Included,
                                Pending = queryOrder.EditEvents.ElementAt(c).Pending,
                                Label = queryOrder.EditEvents.ElementAt(c).Label,
                                Id = queryOrder.EditEvents.ElementAt(c).Id
                            };

                            eventToBeAdded.Groups = new List<Group>();
                            eventToBeAdded.Roles = new List<Role>();

                            foreach (var group in queryOrder.EditEventsGroups.ElementAt(c))
                            {
                                eventToBeAdded.Groups.Add(new Group()
                                {
                                    Id = group.Id,
                                    Name = group.Name
                                });
                            }

                            foreach (var role in queryOrder.EditEventsRoles.ElementAt(c))
                            {
                                eventToBeAdded.Roles.Add(new Role()
                                {
                                    Id = role.Id,
                                    Name = role.Name
                                });
                            }

                            ////Remove duplicate groups.
                            //var groupsWithNoDuplicates = new Dictionary<int, string>();
                            //foreach (var group in queryOrder.EditEventsGroups)
                            //{
                            //    if(!groupsWithNoDuplicates.ContainsKey(group.ElementAt(0).Id))
                            //        groupsWithNoDuplicates.Add(group.ElementAt(0).Id, group.ElementAt(0).Name);
                            //}

                            //put groups on event
                            //for (int i = 0; i < queryOrder.EditEventsGroups.Count(); i++)
                            //{
                            //    foreach (var group in queryOrder.EditEventsGroups.ElementAt(i))
                            //    {
                            //        eventToBeAdded.Groups.Add(new Group()
                            //        {
                            //            Id = group.Id,
                            //            Name = group.Name
                            //        });
                            //    }


                            //}

                            ////put groups on the event
                            //foreach (var group in groupsWithNoDuplicates)
                            //{

                            //    eventToBeAdded.Groups.Add(new Group()
                            //    {
                            //        Id = group.Key,
                            //        Name = group.Value
                            //    });
                            //}

                            //Remove duplicate roles.
                            //var rolesWithNoDuplicates = new Dictionary<int, string>();
                            //foreach (var role in queryOrder.EditEventsGroups)
                            //{
                            //    if (!rolesWithNoDuplicates.ContainsKey(role.ElementAt(0).Id))
                            //        rolesWithNoDuplicates.Add(role.ElementAt(0).Id, role.ElementAt(0).Name);
                            //}

                            //put roles on the event
                            //for (int i = 0; i < queryOrder.EditEventsRoles.Count(); i++)
                            //{
                            //    foreach (var role in queryOrder.EditEventsRoles.ElementAt(i))
                            //    {
                            //        eventToBeAdded.Groups.Add(new Group()
                            //        {
                            //            Id = role.Id,
                            //            Name = role.Name
                            //        });
                            //    }


                            //}
                            //put the assembled event in the list to be added to the order
                            events.Add(eventToBeAdded);
                        }

                        //put pending events back together with groups and roles, and add them to the order
                        for (int c = 0; c < queryOrder.PendingEvents.Count(); c++)
                        {
                            //create the event and set all non collection type properties
                            var eventToBeAdded = new Event()
                            {
                                Description = queryOrder.PendingEvents.ElementAt(c).Description,
                                Executed = queryOrder.PendingEvents.ElementAt(c).Executed,
                                Included = queryOrder.PendingEvents.ElementAt(c).Included,
                                Pending = queryOrder.PendingEvents.ElementAt(c).Pending,
                                Label = queryOrder.PendingEvents.ElementAt(c).Label,
                                Id = queryOrder.PendingEvents.ElementAt(c).Id
                            };
                            eventToBeAdded.Groups = new List<Group>();
                            eventToBeAdded.Roles = new List<Role>();

                            //Remove duplicate groups.
                            //var groupsWithNoDuplicates = new Dictionary<int, string>();
                            //foreach (var group in queryOrder.PendingEventsGroups)
                            //{
                            //    if (!groupsWithNoDuplicates.ContainsKey(group.ElementAt(0).Id))
                            //        groupsWithNoDuplicates.Add(group.ElementAt(0).Id, group.ElementAt(0).Name);
                            //}

                            ////put groups on the event
                            //foreach (var group in groupsWithNoDuplicates)
                            //{

                            //    eventToBeAdded.Groups.Add(new Group()
                            //    {
                            //        Id = group.Key,
                            //        Name = group.Value
                            //    });
                            //}

                            foreach (var group in queryOrder.PendingEventsGroups.ElementAt(c))
                            {
                                eventToBeAdded.Groups.Add(new Group()
                                {
                                    Id = group.Id,
                                    Name = group.Name
                                });
                            }

                            //for (int i = 0; i < queryOrder.PendingEventsGroups.Count(); i++)
                            //{
                            //    foreach (var group in queryOrder.PendingEventsGroups.ElementAt(i))
                            //    {
                            //        eventToBeAdded.Groups.Add(new Group()
                            //        {
                            //            Id = group.Id,
                            //            Name = group.Name
                            //        });
                            //    }


                            //}

                            ////Remove duplicate roles.
                            //var rolesWithNoDuplicates = new Dictionary<int, string>();
                            //foreach (var role in queryOrder.EditEventsGroups)
                            //{
                            //    if (!rolesWithNoDuplicates.ContainsKey(role.ElementAt(0).Id))
                            //        rolesWithNoDuplicates.Add(role.ElementAt(0).Id, role.ElementAt(0).Name);
                            //}

                            //put roles on the event
                            foreach (var role in queryOrder.PendingEventsRoles.ElementAt(c))
                            {
                                eventToBeAdded.Roles.Add(new Role()
                                {
                                    Id = role.Id,
                                    Name = role.Name
                                });
                            }
                            //for (int i = 0; i < queryOrder.PendingEventsRoles.Count(); i++)
                            //{
                            //    foreach (var role in queryOrder.PendingEventsRoles.ElementAt(i))
                            //    {
                            //        eventToBeAdded.Groups.Add(new Group()
                            //        {
                            //            Id = role.Id,
                            //            Name = role.Name
                            //        });
                            //    }


                            //}
                            //put the assembled event in the list to be added to the order
                            events.Add(eventToBeAdded);
                        }




                        //make the DCRGraph to be put onto the order, with all the newly assbled events in it
                        order.DCRGraph = new DROM_Client.Models.BusinessObjects.DCRGraph()
                        {
                            Id = queryOrder.Graph.Id,
                            Events = events
                        };

                        
                        order.ItemsAndQuantity = new List<ItemQuantity>();
                        //put item, quantity and category together to form the DTO Item and quantity. 
                        for (int c = 0; c < queryOrder.OrderDetails.Count; c++)
                        {
                            order.ItemsAndQuantity.Add(new ItemQuantity()
                            {
                                Item = new DROM_Client.Models.BusinessObjects.Item()
                                {
                                    Id = queryOrder.Items.ElementAt(c).Id,
                                    Description = queryOrder.Items.ElementAt(c).Description,
                                    Price = queryOrder.Items.ElementAt(c).Price,
                                    Name = queryOrder.Items.ElementAt(c).Name,
                                    Category = queryOrder.Categories.ElementAt(c).Name
                                }, 
                                Quantity = queryOrder.OrderDetails.ElementAt(c).Quantity
                            });
                        }


                        if (order.OrderType != "For serving")
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

                   return orders;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<HttpStatusCode> UpdateOrder(Tuple<DROM_Client.Models.BusinessObjects.Order, List<int>> data)
        {
            try
            {


                using (var db = new Database())
                {
                    foreach (var i in data.Item2)
                    {
                        var status = await this.ExecuteEvent(i);
                        if (status != HttpStatusCode.OK) return status; //Preconditions were not meet
                    }

                    var orderToBeUpdated = await db.Orders
                        .Include(o => o.OrderDetails.Select(od => od.Item))
                        .Include(o => o.Customer)
                        .FirstOrDefaultAsync(o => o.Id == data.Item1.Id);

                    //update related customer
                    if (orderToBeUpdated.Customer != null)
                    {
                        orderToBeUpdated.Customer.City = data.Item1.Customer.City;
                        orderToBeUpdated.Customer.Email = data.Item1.Customer.Email;
                        orderToBeUpdated.Customer.FirstName = data.Item1.Customer.FirstAndMiddleNames;
                        orderToBeUpdated.Customer.LastName = data.Item1.Customer.LastName;
                        orderToBeUpdated.Customer.Phone = data.Item1.Customer.Phone;
                        orderToBeUpdated.Customer.StreetAndNumber = data.Item1.Customer.StreetAndNumber;
                        orderToBeUpdated.Customer.Zipcode = data.Item1.Customer.ZipCode;
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
                    await db.SaveChangesAsync();
                    return HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<HttpStatusCode> ExecuteEvent(int id)
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

                    var loadedEvents = new Dictionary<int, DCREvent>();

                    //preconditions:
                    //the event must be included
                    if (eventToBeExecuted.Included == false) return HttpStatusCode.InternalServerError;

                    //check if conditions are executed
                    //var conditions = this.GetBySqlQuery(id, "Conditions", true);
                    foreach (var condition in eventToBeExecuted.Conditions)
                    {
                        var cEvent = await db.DCREvents.FirstOrDefaultAsync(e => e.Id == condition.Id);

                        if (cEvent.Executed == false && cEvent.Included) return HttpStatusCode.InternalServerError;
                    }

                    //there must not be a pending milestone 
                    //var milestones = this.GetBySqlQuery(id, "Milestones", true);
                    foreach (var milestone in eventToBeExecuted.Milestones)
                    {
                        var mEvent = await db.DCREvents.FirstOrDefaultAsync(e => e.Id == milestone.Id);

                        if (mEvent.Pending && mEvent.Included) return HttpStatusCode.InternalServerError;
                    }

                    //Preconditions have succeded!


                    //Setup postconditions:
                    eventToBeExecuted.Pending = false;
                    eventToBeExecuted.Executed = true;
                    loadedEvents.Add(eventToBeExecuted.Id, eventToBeExecuted);

                    //exclude related events
                    //var excludes = this.GetBySqlQuery(id, "Excludes", true);
                    foreach (var exclude in eventToBeExecuted.Excludes)
                    {
                        if (loadedEvents.ContainsKey(exclude.Id))
                        {
                            if (loadedEvents[exclude.Id].Included)
                            {
                                loadedEvents[exclude.Id].Included = false;
                            }
                        }
                        else
                        {
                            var eEvent = await db.DCREvents.FirstOrDefaultAsync(e => e.Id == exclude.Id);
                            if (eEvent.Included)
                            {
                                eEvent.Included = false;
                                loadedEvents.Add(eEvent.Id, eEvent);
                            }
                        }
                    }

                    //Include related events
                    //var includes = this.GetBySqlQuery(id, "Includes", true);
                    foreach (var include in eventToBeExecuted.Includes)
                    {
                        if (loadedEvents.ContainsKey(include.Id))
                        {
                            if (!loadedEvents[include.Id].Included)
                            {
                                loadedEvents[include.Id].Included = true;
                            }
                        }
                        else
                        {
                            var iEvent = await db.DCREvents.FirstOrDefaultAsync(e => e.Id == include.Id);
                            if (!iEvent.Included)
                            {
                                iEvent.Included = true;
                                loadedEvents.Add(iEvent.Id, iEvent);
                            }
                        }
                    }

                    //set related events pending
                    //var responses = this.GetBySqlQuery(id, "Responses", true);
                    foreach (var response in eventToBeExecuted.Responses)
                    {
                        if (loadedEvents.ContainsKey(response.Id))
                        {
                            if (!loadedEvents[response.Id].Pending)
                            {
                                loadedEvents[response.Id].Pending = true;
                            }
                        }
                        else
                        {
                            var rEvent = await db.DCREvents.FirstOrDefaultAsync(e => e.Id == response.Id);
                            if (!rEvent.Pending)
                            {
                                rEvent.Pending = true;
                                loadedEvents.Add(rEvent.Id, rEvent);
                            }
                        }
                    }

                    //set updated events to be updated in db
                    foreach (var e in loadedEvents)
                    {
                        db.Entry(e.Value).State = EntityState.Modified;
                    }


                    await db.SaveChangesAsync();

                    var order = await (from o in db.Orders.Include(o => o.DCRGraph.DCREvents)
                                       where o.DCRGraph.Id == eventToBeExecuted.DCRGraphId
                                       select o).FirstOrDefaultAsync();

                    if (order.DCRGraph.DCREvents.Any(dcrEvent => dcrEvent.Included && dcrEvent.Pending))
                    {
                        return HttpStatusCode.OK;
                    }

                    order.DCRGraph.AcceptingState = true;
                    db.Entry(order.DCRGraph).State = EntityState.Modified;
                    db.Entry(order).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    return HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<List<string>> DeliveryTypes(int orderType)
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
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<HttpStatusCode> AchiveOrder(int order)
        {
            using (var db = new Database())
            {
                var orderToBeArchived = await db.Orders.FindAsync(order);
                if(orderToBeArchived == null) return HttpStatusCode.InternalServerError;
                orderToBeArchived.Archived = true;
                db.Entry(orderToBeArchived).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return HttpStatusCode.OK;

            }
        }
    }
}
