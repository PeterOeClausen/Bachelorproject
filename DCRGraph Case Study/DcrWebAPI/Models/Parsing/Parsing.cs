using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using DcrWebAPI.XMLParser;

namespace DcrWebAPI.Models.Parsing
{
    class Parsing
    {
        /*
        public Parsing(EventAndRolesContainer container, OrderInfo orderInfo)
        {
            using (var db = new DBSchemaContainer())
            {

                
                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        var order = new Order();
                        order.DCRGraph = new DCRGraph();
                        order.OrderDate = DateTime.Now;
                        order.Notes = orderInfo.Notes;
                        order.Table = orderInfo.Table;


                        

                        List<Includes> includes = new List<Includes>();
                        foreach (var i in container.Inclusions)
                        {

                            container.Events.Find(x => x.EventId.Equals(i.fromNodeId)).
                        }

                        scope.Complete();
                    }
                    catch (Exception)
                    {
                        
                        throw;
                    }
                    
                }

            }
        }
    */
    }
    
}
