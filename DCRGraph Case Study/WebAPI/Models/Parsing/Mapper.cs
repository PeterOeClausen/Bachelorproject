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
using WebAPI.Models.DBMethods;


namespace WebAPI.Models.Parsing
{
    class Mapper
    {
        
        public Mapper(EventAndRolesContainer container, NewOrderInfo orderInfo)
        {
            using (var db = new WebAPI.Models.DBObjects.Database())
            {

                
                //using (TransactionScope scope = new TransactionScope())
                //{
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
                            InsertBySqlQuery(fromEvent.Id, toEvent.Id, "Includes");
                        }
                        
                        //put exclusions on events
                        foreach (var i in container.Exclusions)
                        {
                            var fromEvent = container.Events.Find(x => x.EventId.Equals(i.fromNodeId));
                            var toEvent = container.Events.Find(x => x.EventId.Equals(i.toNodeId));
                            InsertBySqlQuery(fromEvent.Id, toEvent.Id, "Excludes");
                        }

                        //put responses on events
                        foreach (var i in container.Responses)
                        {
                            var fromEvent = container.Events.Find(x => x.EventId.Equals(i.fromNodeId));
                            var toEvent = container.Events.Find(x => x.EventId.Equals(i.toNodeId));
                            InsertBySqlQuery(fromEvent.Id, toEvent.Id, "Responses");
                        }

                        //put conditions on events
                        foreach (var i in container.Conditions)
                        {
                            var fromEvent = container.Events.Find(x => x.EventId.Equals(i.fromNodeId));
                            var toEvent = container.Events.Find(x => x.EventId.Equals(i.toNodeId));
                            InsertBySqlQuery(fromEvent.Id, toEvent.Id, "Conditions");
                        }




                        //put milestones on events
                        foreach (var i in container.Milestones)
                        {
                            var fromEvent = container.Events.Find(x => x.EventId.Equals(i.fromNodeId));
                            var toEvent = container.Events.Find(x => x.EventId.Equals(i.toNodeId));
                            InsertBySqlQuery(fromEvent.Id, toEvent.Id, "Milestones");
                        }
                        

                        foreach (var e in container.Events)
                        {
                            db.Entry(e).State = EntityState.Modified;
                        }
                        db.SaveChanges();
                        

                        //scope.Complete();
                        
                        
                    }
                    catch (Exception ex)
                    {

                        
                        throw;
                    }
                    
                //}

            }
        }

        void InsertBySqlQuery(int fromId, int toId, string table)
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
                    
                    command.CommandText = @"
                    INSERT INTO " + table + " (FromId, ToId)" +
                                          " VALUES ('" + fromId + "', '" + toId + "');";
                    

                    // Open a connection to database.
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
    
}
