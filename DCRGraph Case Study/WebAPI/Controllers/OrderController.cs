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



namespace WebAPI.Controllers
{
    public class OrderController : ApiController
    {
        [Route("api/order/items")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetItems()
        {
            return Request.CreateResponse(HttpStatusCode.OK, await new DbInteractions().GetItems());
        }

        [Route("api/order/ordersWithSortedEvents")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetOrders()
        {
            //var orders = await new DbInteractions().GetOrdersWithSortedEvents();
            //var json = JsonConvert.SerializeObject(orders);
            //var des = JsonConvert.DeserializeObject<List<DROM_Client.Models.BusinessObjects.Order>>(json);
            return Request.CreateResponse(HttpStatusCode.OK, await new DbInteractions().GetOrdersWithSortedEvents());
        }

        [Route("api/order/UpdateOrder")]
        [HttpPut]
        public async Task<HttpResponseMessage> UpdateOrder(Tuple<DROM_Client.Models.BusinessObjects.Order, List<int>> data)
        {
            return Request.CreateResponse(await new DbInteractions().UpdateOrder(data));
        }

        [Route("api/order/executeEvent")]
        [HttpPut]
        public async Task<HttpResponseMessage> ExecuteEvent(Event e)
        {
            
            return Request.CreateResponse(await new DbInteractions().ExecuteEvent(e.Id));
        }

        [Route("api/order/deliveryTypes/{orderType}")]
        [HttpGet]
        public async Task<HttpResponseMessage> DeliveryTypes(int orderType)
        {
            return Request.CreateResponse(
                HttpStatusCode.OK, 
                await new DbInteractions().DeliveryTypes(orderType));
        }

        [Route("api/order/Archive")]
        [HttpPut]
        public async Task<HttpResponseMessage> ArchiveOrder(int order)
        {
            return Request.CreateResponse(await new DbInteractions().AchiveOrder(order));
        }


    }
}