using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DROM_Client.Models.NewOrderData;
using Newtonsoft.Json;
using WebAPI.Models.DBMethods;
using WebAPI.Models.DBObjects;
using WebAPI.Models.Parsing;
using WebAPI.XMLParser;

namespace WebAPI.Controllers
{
    public class ParseController : ApiController
    {

        

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
