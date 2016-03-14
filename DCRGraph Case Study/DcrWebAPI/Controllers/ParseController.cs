using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Razor.Text;
using System.Xml.Linq;

namespace DcrWebAPI.Controllers
{
    public class ParseController : ApiController
    {

        public async void Post()
        {
            XDocument doc = XDocument.Load(await Request.Content.ReadAsStreamAsync());
            var EventsAndRoles = new DCRXml

            //String saveLoc = @"C:\Users\Archigo\Desktop\test.xml";
            //doc.Save(saveLoc); Console.WriteLine(doc.ToString());
        }

        
    }
}