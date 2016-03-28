using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DROM_Client.Models.NewOrderData;
using Newtonsoft.Json;
using WebAPI.Models.DBObjects;

namespace WebAPI.Controllers
{
    public class ParseController : ApiController
    {

        public string Get()
        {

            using (var db = new Database())
            {
                var items = db.Items.Count();

                return "" + items;
            }
        }

        public void Post([FromBody] NewOrderInfo info)
        {
            var qwe = info;

            Console.WriteLine(qwe.ToString());
        }
    }
}
