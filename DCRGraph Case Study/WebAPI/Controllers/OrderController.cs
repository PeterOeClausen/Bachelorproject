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
        public HttpResponseMessage GetItems()
        {
            return Request.CreateResponse(HttpStatusCode.OK, new DbInteractions().GetItems().Result);
        }

        [Route("api/order/ordersWithSortedEvents")]
        [HttpGet]
        public HttpResponseMessage GetOrders()
        {
            return Request.CreateResponse(HttpStatusCode.OK, new DbInteractions().GetOrdersWithSortedEvents().Result);
        }

        [Route("api/order/UpdateOrder")]
        [HttpPut]
        public HttpResponseMessage UpdateOrder(DROM_Client.Models.BusinessObjects.Order order)
        {
            return Request.CreateResponse(new DbInteractions().UpdateOrder(order));
        }

        [Route("api/order/executeEvent")]
        [HttpPut]
        public HttpResponseMessage ExecuteEvent(Event e)
        {
            return Request.CreateResponse(new DbInteractions().ExecuteEvent(e.Id).Result);
        }


    }
}