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

namespace WebAPI.Models.DBMethods
{
    public class DbInteractions
    {

        public async Task<List<DROM_Client.Models.BusinessObjects.Item>> GetItems()
        {
            using (var db = new Database())
            {
                //db.Configuration.LazyLoadingEnabled = false;
                var items = db.Items;
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

        public async Task<List<DROM_Client.Models.BusinessObjects.Order>> GetOrdersWithSortedEvents()
        {
            using (var db = new Database())
            {
                //get orders from db
                var orders = db.Orders;
                List<DROM_Client.Models.BusinessObjects.Order> orderList = new List<DROM_Client.Models.BusinessObjects.Order>();

                //convert db orders to serializable transfer orders
                foreach (var o in orders)
                {

                    var order = new DROM_Client.Models.BusinessObjects.Order()
                    {
                        Id = o.Id,
                        OrderType = o.OrderType,
                        Notes = o.Notes ?? "",
                        OrderDate = o.OrderDate,
                        Table = o.Table
                    };

                    //if db customer not null, put it on the order, otherwise attach an empty customer to the order
                    if (o.Customer != null)
                    {
                        var customer = new DROM_Client.Models.BusinessObjects.Customer()
                        {
                            Id = o.Customer.Id,
                            Phone = o.Customer.Phone,
                            ZipCode = o.Customer.Zipcode,
                            StreetAndNumber = o.Customer.StreetAndNumber,
                            City = o.Customer.City,
                            Email = o.Customer.Email,
                            FirstAndMiddleNames = o.Customer.FirstName,
                            LastName = o.Customer.LastName

                        };
                        order.Customer = customer;
                    }
                    else order.Customer = new DROM_Client.Models.BusinessObjects.Customer();

                    //attach DCRGraph to the order
                    order.DCRGraph = new DROM_Client.Models.BusinessObjects.DCRGraph()
                    {
                        Id = o.DCRGraph.Id,
                        Events = new List<Event>()
                    };

                    //convert events and attach them to the graph
                    foreach (var dcrEvent in o.DCRGraph.DCREvents)
                    {
                        if (dcrEvent.Included == true) // only add included events
                        {
                            foreach (var g in dcrEvent.Groups)
                            {
                                if ((dcrEvent.Pending == true || g.Name == "Edit events")) // we only want to give pending events and edit events
                                {
                                    var businessEvent = new Event
                                    {
                                        Id = dcrEvent.Id,
                                        Description = dcrEvent.Description ?? "",
                                        Executed = dcrEvent.Executed,
                                        Included = dcrEvent.Included,
                                        Label = dcrEvent.Label,
                                        Pending = dcrEvent.Pending,
                                        Roles = null,
                                        Groups = new List<DROM_Client.Models.BusinessObjects.Group>()
                                    };

                                    //get groups onto the event
                                    foreach (var groupList in dcrEvent.Groups)
                                    {
                                        var group = new DROM_Client.Models.BusinessObjects.Group();
                                        group.Id = groupList.Id;
                                        group.Name = groupList.Name;
                                        businessEvent.Groups.Add(group);
                                    }

                                    //get roles onto the event
                                    businessEvent.Roles = new List<DROM_Client.Models.BusinessObjects.Role>();
                                    foreach (var r in dcrEvent.Roles)
                                    {
                                        var role = new DROM_Client.Models.BusinessObjects.Role();
                                        role.Id = r.Id;
                                        role.Name = r.Name;
                                        businessEvent.Roles.Add(role);
                                    }

                                    order.DCRGraph.Events.Add(businessEvent);
                                }
                            }
                        }

                    }

                    //put items and quantity on the order
                    var itemsAndQuantity = new List<DROM_Client.Models.BusinessObjects.ItemQuantity>();
                    foreach (var od in o.OrderDetails)
                    {
                        var itemQuantity = new ItemQuantity()
                        {
                            Item = new DROM_Client.Models.BusinessObjects.Item()
                            {
                                Id = od.Item.Id,
                                Category = od.Item.Category.Name,
                                Name = od.Item.Name,
                                Description = od.Item.Description,
                                Price = od.Item.Price

                            },
                            Quantity = od.Quantity

                    };
                        


                        itemsAndQuantity.Add(itemQuantity);
                    }
                    order.ItemsAndQuantity = itemsAndQuantity;






                    orderList.Add(order);
                }

                return orderList;
            }
        }

        public async Task<HttpStatusCode> UpdateOrder(Tuple<DROM_Client.Models.BusinessObjects.Order, List<int>> data)
        {

            using (var db = new Database())
            {

                var dbInter = new DbInteractions();
                foreach (var i in data.Item2)
                {
                    var status = await dbInter.ExecuteEvent(i);
                    if (status != HttpStatusCode.OK) return status; //Preconditions were not meet
                }

                var orderToBeUpdated = await db.Orders.FindAsync(data.Item1.Id);

                //update related customer
                orderToBeUpdated.Customer.City = data.Item1.Customer.City;
                orderToBeUpdated.Customer.Email = data.Item1.Customer.Email;
                orderToBeUpdated.Customer.FirstName = data.Item1.Customer.FirstAndMiddleNames;
                orderToBeUpdated.Customer.LastName = data.Item1.Customer.LastName;
                orderToBeUpdated.Customer.Phone = data.Item1.Customer.Phone;
                orderToBeUpdated.Customer.StreetAndNumber = data.Item1.Customer.StreetAndNumber;
                orderToBeUpdated.Customer.Zipcode = data.Item1.Customer.ZipCode;

                //update the order
                orderToBeUpdated.Notes = data.Item1.Notes;
                orderToBeUpdated.Table = data.Item1.Table;
                orderToBeUpdated.OrderType = data.Item1.OrderType;
                data.Item1.ItemsAndQuantity = data.Item1.ItemsAndQuantity;

                db.Entry(orderToBeUpdated).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return HttpStatusCode.OK;
            }
        }

        public async Task<HttpStatusCode> ExecuteEvent(int id)
        {
            try
            {

            
            using (var db = new Database())
            {
                var eventToBeExecuted = await db.DCREvents.FirstOrDefaultAsync(e => e.Id == id);

                

                var loadedEvents = new Dictionary<int, DCREvent>();

                //preconditions:
                //the event must be included
                if (eventToBeExecuted.Included == false) return HttpStatusCode.InternalServerError;

                //check if conditions are executed
                var conditions = this.GetBySqlQuery(id, "Conditions", true);
                foreach (var condition in conditions)
                {
                    var cEvent = await db.DCREvents.FirstOrDefaultAsync(e => e.Id == condition.Item2);
                    
                    if (cEvent.Executed != true && cEvent.Included) return HttpStatusCode.InternalServerError;
                }

                //there must not be a pending milestone 
                var milestones = this.GetBySqlQuery(id, "Milestones", true);
                foreach (var milestone in milestones)
                {
                    var mEvent = await db.DCREvents.FirstOrDefaultAsync(e => e.Id == milestone.Item2);
                    
                    if (mEvent.Pending != false && mEvent.Included) return HttpStatusCode.InternalServerError;
                }

                //Preconditions have succeded!


                //Setup postconditions:
                eventToBeExecuted.Pending = false;
                eventToBeExecuted.Executed = true;
                loadedEvents.Add(eventToBeExecuted.Id,eventToBeExecuted);

                //exclude related events
                var excludes = this.GetBySqlQuery(id, "Excludes", true);
                foreach (var exclude in excludes)
                {
                    if (loadedEvents.ContainsKey(exclude.Item2))
                    {
                        if (loadedEvents[exclude.Item2].Included)
                        {
                            loadedEvents[exclude.Item2].Included = false;
                        }
                    }
                    else
                    {
                        var eEvent = await db.DCREvents.FirstOrDefaultAsync(e => e.Id == exclude.Item2);
                        if (eEvent.Included)
                        {
                            eEvent.Included = false;
                            loadedEvents.Add(eEvent.Id, eEvent);
                        }
                    }
                }

                //Include related events
                var includes = this.GetBySqlQuery(id, "Includes", true);
                foreach (var include in includes)
                {
                    if (loadedEvents.ContainsKey(include.Item2))
                    {
                        if (!loadedEvents[include.Item2].Included)
                        {
                            loadedEvents[include.Item2].Included = true;
                        }
                    }
                    else
                    {
                        var iEvent = await db.DCREvents.FirstOrDefaultAsync(e => e.Id == include.Item2);
                        if (!iEvent.Included)
                        {
                            iEvent.Included = true;
                            loadedEvents.Add(iEvent.Id, iEvent);
                        }
                    }
                }

                //set related events pending
                var responses = this.GetBySqlQuery(id, "Responses", true);
                foreach (var response in responses)
                {
                    if (loadedEvents.ContainsKey(response.Item2))
                    {
                        if (!loadedEvents[response.Item2].Pending)
                        {
                            loadedEvents[response.Item2].Pending = true;
                        }
                    }
                    else
                    {
                        var rEvent = await db.DCREvents.FirstOrDefaultAsync(e => e.Id == response.Item2);
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
        


        List<Tuple<int,int>> GetBySqlQuery(int i, string table, bool d)
            // i is the to or from id. d decides whether to look for from or to. true for from
        {
            System.Configuration.Configuration rootWebConfig =
                System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/MyWebSiteRoot");
            System.Configuration.ConnectionStringSettings connString;

            connString = rootWebConfig.ConnectionStrings.ConnectionStrings["Database"];
            try
            {

            

            // Create an SqlConnection from the provided connection string.
            using (SqlConnection connection = new SqlConnection(connString.ConnectionString))
            {
                // Formulate the command.
                SqlCommand command = new SqlCommand();
                command.Connection = connection;

                // Specify the query to be executed.
                command.CommandType = CommandType.Text;
                if (d)
                {
                    command.CommandText = @"
                    SELECT FromId, ToId
                    FROM " + table
                    + " WHERE FromId=" + i + ";";
                }
                else
                {
                    command.CommandText = @"
                    SELECT FromId, ToId
                    FROM " + table
                    + " WHERE ToId=" + i + ";";
                }

                // Open a connection to database.
                connection.Open();

                // Read data returned for the query.
                SqlDataReader reader = command.ExecuteReader();
                var result = new List<Tuple<int, int>>();
                while (reader.Read())
                    {
                        result.Add(new Tuple<int, int>(Int32.Parse(reader[0].ToString()), Int32.Parse(reader[1].ToString())));
                    }
                return result;
                
            }
            }
            catch (Exception ex )
            {

                throw;
            }
        }



    }
}
