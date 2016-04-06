using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DROM_Client.Models.BusinessObjects;
using Database = WebAPI.Models.DBObjects.Database;

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
                                if ((dcrEvent.Pending == true || g.Name == "Edit Events")) // we only want to give pending events and edit events
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

        public async Task<HttpStatusCode> UpdateOrder(DROM_Client.Models.BusinessObjects.Order order)
        {

            using (var db = new Database())
            {
                var orderToBeUpdated = await db.Orders.FindAsync(order.Id);

                //update related customer
                orderToBeUpdated.Customer.City = order.Customer.City;
                orderToBeUpdated.Customer.Email = order.Customer.Email;
                orderToBeUpdated.Customer.FirstName = order.Customer.FirstAndMiddleNames;
                orderToBeUpdated.Customer.LastName = order.Customer.LastName;
                orderToBeUpdated.Customer.Phone = order.Customer.Phone;
                orderToBeUpdated.Customer.StreetAndNumber = order.Customer.StreetAndNumber;
                orderToBeUpdated.Customer.Zipcode = order.Customer.ZipCode;

                //update the order
                orderToBeUpdated.Notes = order.Notes;
                orderToBeUpdated.Table = order.Table;
                orderToBeUpdated.OrderType = order.OrderType;
                order.ItemsAndQuantity = order.ItemsAndQuantity;

                db.Entry(orderToBeUpdated).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return HttpStatusCode.OK;
            }
        }

        public async Task<HttpStatusCode> ExecuteEvent(int id)
        {
            using (var db = new Database())
            {
                var eventToBeExecuted = await db.DCREvents
                    .Where(e => e.Id == id)
                    .Include(e => e.ConditionReverseTo)
                    .Include(e => e.ExcludeTo)
                    .Include(e => e.IncludeTo)
                    .Include(e => e.MilestoneReverseTo)
                    .Include(e => e.ResponseTo)
                    .FirstOrDefaultAsync();
                var dcrGraph = db.DCRGraphs.FindAsync(eventToBeExecuted.DCRGraphId);
                //preconditions:
                //the event must be included
                if (eventToBeExecuted.Included == false) return HttpStatusCode.InternalServerError;


                foreach (var condition in eventToBeExecuted.ConditionReverseTo)
                {
                    //condition events must be executed
                    if (condition.Executed != true && condition.Included) return HttpStatusCode.InternalServerError;
                }
                foreach (var milestone in eventToBeExecuted.MilestoneReverseTo)
                {
                    //there must not be a pending milestone 
                    if (milestone.Pending != false && milestone.Included) return HttpStatusCode.InternalServerError;
                }

                //Preconditions have succeded!
                //Setup postconditions:

                
                foreach (var e in eventToBeExecuted.ExcludeTo)
                {
                    //exclude related events
                    e.Included = false;
                }

                foreach (var e in eventToBeExecuted.IncludeTo)
                {
                    //Include related events
                    e.Included = true;
                }

                foreach (var e in eventToBeExecuted.ResponseTo)
                {
                    //set related events pending
                    e.Pending = true;
                }

               


                return HttpStatusCode.OK;
            }
        }
        /*
        public async Task<List<string>> DeliveryTypes(int type)
        {
            using (var db = new Database() )
            {
                switch (type)
                {
                    case 1:
                        return await db.DCREvents.FindAsync(slec)
                }
            }
        }
        */
    }
}
