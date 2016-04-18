using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DROM_Client.Models.BusinessObjects;
using Newtonsoft.Json;
using WebAPI.Models.DBMethods;
using WebAPI.Models.DBObjects;
using Item = DROM_Client.Models.BusinessObjects.Item;


namespace WebAPI.Controllers
{
    public class OrderController : ApiController
    {
        [Route("api/order/items")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetItems()
        {
            var result = await new DbInteractions().GetItems();
            var response = Request.CreateResponse(result.Item3, result.Item1 ?? new List<Item>());
            response.ReasonPhrase = result.Item2;
            return response;
        }

        [Route("api/order/ordersWithSortedEvents")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetOrders()
        {

            var result = await new DbInteractions().GetOrdersWithSortedEvents();
            var response = Request.CreateResponse(result.Item3, result.Item1 ?? new List<DROM_Client.Models.BusinessObjects.Order>());
            response.ReasonPhrase = result.Item2;
            return response;
        }

        [Route("api/order/UpdateOrder")]
        [HttpPut]
        public async Task<HttpResponseMessage> UpdateOrder(Tuple<DROM_Client.Models.BusinessObjects.Order, List<int>> data)
        {
            var result = await new DbInteractions().UpdateOrder(data);
            var response = Request.CreateResponse(result.Item2);
            response.ReasonPhrase = result.Item1;
            return response;
        }

        [Route("api/order/executeEvent")]
        [HttpPut]
        public async Task<HttpResponseMessage> ExecuteEvent(Event e)
        {
            var result = await new DbInteractions().ExecuteEvent(e.Id);
            var response = Request.CreateResponse(result.Item2);
            response.ReasonPhrase = result.Item1;
            return response;
        }

        [Route("api/order/deliveryTypes/{orderType}")]
        [HttpGet]
        public async Task<HttpResponseMessage> DeliveryTypes(int orderType)
        {
            var result = await new DbInteractions().DeliveryTypes(orderType);
            var response = Request.CreateResponse(result.Item3, result.Item1);
            response.ReasonPhrase = result.Item2;
            return response;
        }

        [Route("api/order/Archive")]
        [HttpPut]
        public async Task<HttpResponseMessage> ArchiveOrder(DROM_Client.Models.BusinessObjects.Order order)
        {
            var result = await new DbInteractions().AchiveOrder(order.Id);
            var response = Request.CreateResponse(result.Item2);
            response.ReasonPhrase = result.Item1;
            return response;
        }


    }
}