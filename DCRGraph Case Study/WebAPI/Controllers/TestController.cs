using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebAPI.Models.DBMethods;

namespace WebAPI.Controllers
{
    public class TestController : ApiController
    {
        public async Task<string> get()
        {
            await new DbInteractions().ExecuteEvent(800);
            return "test";
        }
    }
}
