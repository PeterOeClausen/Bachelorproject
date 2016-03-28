using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web.Http;
using System.Web.Razor.Text;
using System.Xml.Linq;
using DcrWebAPI.Models.DBObjects;
using DcrWebAPI.Models.Parsing;
using DcrWebAPI.XMLParser;

namespace DcrWebAPI.Controllers
{
    public class ParseController : ApiController
    {

        public async void Post()
        {
            XDocument doc = XDocument.Load(await Request.Content.ReadAsStreamAsync());
            var parser = new DCRXmlParser();
            var eventsAndRoles = parser.Parse(doc.ToString());
           // new Parsing(eventsAndRoles);
        }
        
        
        public string Get()
        {

            using (var db = new Database())
            {
                var items = db.Items.Count();
                 
                return "" + items;
            }
        }


    }
}