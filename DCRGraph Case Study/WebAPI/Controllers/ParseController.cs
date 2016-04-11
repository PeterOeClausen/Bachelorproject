using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        

        public HttpResponseMessage Post([FromBody] NewOrderInfo info)
        {

            

            //try
            //{
                new Mapper(new DCRXmlParser().Parse(Properties.Resources.Bachelor2), info);
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.NoContent
                };
            /*}
            catch (Exception ex)
            {
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest
                };
                throw;
            }*/
            

            
        }
    }
}
