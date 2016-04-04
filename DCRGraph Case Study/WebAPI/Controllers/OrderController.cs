using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebAPI.Models.DBObjects;

namespace WebAPI.Controllers
{
    public class OrderController : ApiController
    {
        // GET api/<controller>
        public HttpResponseMessage GetItems()
        {

            using (var db = new Database())
            {
                var items = db.Items;
                List<Item> itemList = new List<Item>();
                foreach (var i in items)
                {
                    itemList.Add(i);
                }
                return Request.CreateResponse(HttpStatusCode.OK, itemList);

             
            }
        }


        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
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