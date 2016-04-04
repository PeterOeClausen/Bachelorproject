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

        [Route("api/order/orders")]
        [HttpGet]
        public HttpResponseMessage GetOrders()
        {
            return Request.CreateResponse(HttpStatusCode.OK, new DbInteractions().GetOrders().Result);
        }

        [Route("api/order/UpdateOrder")]
        [HttpPut]
        public HttpResponseMessage UpdateOrder(DROM_Client.Models.BusinessObjects.Order order)
        {
            return Request.CreateResponse(new DbInteractions().UpdateOrder(order));
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}