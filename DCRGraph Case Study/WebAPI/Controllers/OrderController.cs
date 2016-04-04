using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using WebAPI.Models.DBObjects;
using Item = DROM_Client.Models.BusinessObjects.Item;


namespace WebAPI.Controllers
{
    public class OrderController : ApiController
    {
        // GET api/<controller>
        [Route("order/items")]
        [HttpGet]
        public HttpResponseMessage GetItems()
        {
            try
            {



                using (var db = new Database())
                {
                    //db.Configuration.LazyLoadingEnabled = false;
                    var items = db.Items;
                    List<Item> itemList = new List<Item>();
                    foreach (var i in items)
                    {
                        var item = new Item()
                        {
                            Category = i.Category.Name,
                            Description = i.Description,
                            Id = i.Id,
                            Name = i.Name,
                            Price = i.Price
                        };
                        itemList.Add(item);
                    }
                    //string output = JsonConvert.SerializeObject(itemList);
                    var res = Request.CreateResponse(HttpStatusCode.OK, itemList);
                    //res.Content = new StringContent(output);

                    return res;
                }
            }
            catch (Exception)
            {

                throw;
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