using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DROM_Client.Models.NewOrderData;
using Newtonsoft.Json;
using WebAPI.Models.DBObjects;
using WebAPI.Models.Parsing;
using WebAPI.XMLParser;

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

        public HttpResponseMessage Post([FromBody] NewOrderInfo info)
        {

            try
            {
                var qwe = new Mapper(new DCRXmlParser().Parse(new Workflow1().Workflow), info);
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.NoContent
                };
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest
                };
                throw;
            }
            

            
        }
    }
}
