using DROM_Client.Models.NewOrderData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DROM_Client.Models.BusinessObjects;

namespace DROM_Client.Services
{
    public class APICaller
    {
        /// <summary>
        /// Save order on web api
        /// </summary>
        /// <param name="newOrder"></param>
        /// <returns></returns>
        public async Task<string> PostOrderAsync(NewOrderInfo newOrder) //Rename to: PostNewOrder
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:57815/");
                var response = await client.PutAsXmlAsync("api/parse", newOrder);
                return response.StatusCode.ToString();
            }
        }

        public async Task<string> PutUpdateOrder(Order updatedOrder)
        {
            return null; //Not implemented yet
        }

        /// <summary>
        /// Execute event on API
        /// </summary>
        /// <param name="eventToExecute"></param>
        /// <returns></returns>
        public async Task<string> PostExecuteEvent(Event eventToExecute)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:57815/");
                var response = await client.PostAsXmlAsync("api/eventtoexecute", eventToExecute);
                return response.StatusCode.ToString();
            }
        }

        /// <summary>
        /// Receive all orders
        /// </summary>
        /// <returns></returns>
        public async Task<List<Order>> GetOrders()
        {
            using (var client = new HttpClient())
            {
                return null; //Not implemented yet
            }
        }

        public async Task<List<Item>> GetItems() //Needs to be called only one time.
        {
            return new List<Item> {
                new Item
                {
                    Name = "Cola"
                },
                new Item
                {
                    Name = "Sprite"
                },
                new Item
                {
                    Name = "Pizza"
                },
                new Item
                {
                    Name = "Burger"
                }
            };
        }
    }
}
