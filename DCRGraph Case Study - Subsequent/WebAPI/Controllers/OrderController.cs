using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DROM_Client.Models.BusinessObjects;
using WebAPI.Models.DBMethods;
using Item = DROM_Client.Models.BusinessObjects.Item; //make it easier to refer to this
using System.Web.Http.Cors;
using DROM_Client.Models.NewOrderData;
using WebAPI.Models.Parsing;
using WebAPI.XMLParser;

namespace WebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")] //allows any origin to access the api
    public class OrderController : ApiController
    {

        /// <summary>
        /// Method to receive requests for existing items in the database. Will find and return all items.
        /// </summary>
        /// <returns></returns>
        [Route("api/order/items")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetItems()
        {
            var result = await new DbInteractions().GetItems();
            var response = Request.CreateResponse(result.Item3, result.Item1 ?? new List<Item>());
            response.ReasonPhrase = result.Item2;
            return response;
        }

        /// <summary>
        /// Method to receive requests for orders. Expects a restaurant id in the form of an int.
        /// Once a requests is received, it will look for the orders related to that restaurant in the database, and return it to the requester.
        /// The events on the orders DCRGraph are filtered to only contain events which are pending, or edit events.
        /// </summary>
        /// <param name="restaurant"></param>
        /// <returns></returns>
        [Route("api/order/ordersWithSortedEvents")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetOrders(int restaurant)
        {

            var result = await new DbInteractions().GetOrdersWithSortedEvents(restaurant);
            var response = Request.CreateResponse(result.Item3, result.Item1 ?? new List<DROM_Client.Models.BusinessObjects.Order>());
            response.ReasonPhrase = result.Item2;
            return response;
        }

        /// <summary>
        /// Method to receive order update requests. It expects the order with updated information as well as a list of events of on the order to execute. The list can be empty.
        /// The order will be found in the database and updated. If there was any events in the list, these also be executed.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("api/order/UpdateOrder")]
        [HttpPut]
        public async Task<HttpResponseMessage> UpdateOrder(Tuple<DROM_Client.Models.BusinessObjects.Order, List<int>> data)
        {
            var result = await new DbInteractions().UpdateOrder(data);
            var response = Request.CreateResponse(result.Item2);
            response.ReasonPhrase = result.Item1;
            return response;
        }

        /// <summary>
        /// Method to receive event execution requests. Expects to receive the event it has to execute.
        /// Once a request is received, it will attempt to execute the event.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        [Route("api/order/executeEvent")]
        [HttpPut]
        public async Task<HttpResponseMessage> ExecuteEvent(Event e)
        {
            var result = await new DbInteractions().ExecuteEvent(e.Id, false);
            var response = Request.CreateResponse(result.Item2);
            response.ReasonPhrase = result.Item1;
            return response;
        }

        /// <summary>
        /// Method to receive requests for delivery types.
        /// When receiving requests, it will find all the delivery types in the database and return them.
        /// </summary>
        /// <param name="orderType"></param>
        /// <returns></returns>
        [Route("api/order/deliveryTypes/{orderType}")]
        [HttpGet]
        public async Task<HttpResponseMessage> DeliveryTypes(int orderType)
        {
            var result = await new DbInteractions().DeliveryTypes(orderType);
            var response = Request.CreateResponse(result.Item3, result.Item1);
            response.ReasonPhrase = result.Item2;
            return response;
        }

        /// <summary>
        /// Method to receive archive requests. Expects iformation about wat order to archive.
        /// Once a requests is received, it will archive the order.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [Route("api/order/Archive")]
        [HttpPut]
        public async Task<HttpResponseMessage> ArchiveOrder(DROM_Client.Models.BusinessObjects.Order order)
        {
            var result = await new DbInteractions().AchiveOrder(order.Id);
            var response = Request.CreateResponse(result.Item2);
            response.ReasonPhrase = result.Item1;
            return response;
        }

        /// <summary>
        /// Method to receive create order requests. Expects all the information about a new order to be create.
        /// Once a request is received, it will create a new order in the database with the DCRGraph that is currently loaded.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [Route("api/order/create")]
        [HttpPost]
        public async Task<HttpResponseMessage> Post([FromBody] NewOrderInfo info)
        {
            try
            {
                await new Mapper().CreateOrder(new DCRXmlParser().Parse(Properties.Resources.Bachelor2), info);
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.ReasonPhrase = "success";
                return response;
            }
            catch (Exception ex)
            {
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.ReasonPhrase = ex.Message;
                return response;
            }
        }


    }
}