using DROM_Client.Models.NewOrderData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DROM_Client.Models.BusinessObjects;
using Windows.UI.ViewManagement;

namespace DROM_Client.Services
{
    public class APICaller
    {

        public Uri BaseAddress { get; set; }

        public APICaller()
        {
            BaseAddress = new Uri("http://localhost:57815/"); //set the address of the api here
        }

        /// <summary>
        /// Save order on web api
        /// </summary>
        /// <param name="newOrder"></param>
        /// <returns></returns>
        public async Task<string> PostOrderAsync(NewOrderInfo newOrder) //Rename to: PostNewOrder
        {
            using (var client = new HttpClient())
            {
                try
                {

                
                client.BaseAddress = BaseAddress;
                var response = await client.PostAsXmlAsync("api/parse", newOrder, new CancellationToken());
                return response.StatusCode.ToString();
                    //var content = new FormUrlEncodedContent(newOrder);
                    //var response = await client.PostAsJson("api/parse", content);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public async Task<string> PutUpdateOrder(Order updatedOrder)
        {

            using (var client = new HttpClient())
            {
                try
                {


                    client.BaseAddress = BaseAddress;
                    var response = await client.PutAsXmlAsync("api/order/updateorder", updatedOrder, new CancellationToken());
                    return response.StatusCode.ToString();
                    //var content = new FormUrlEncodedContent(newOrder);
                    //var response = await client.PostAsJson("api/parse", content);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Execute event on API
        /// </summary>
        /// <param name="eventToExecute"></param>
        /// <returns></returns>
        public async Task<string> PutExecuteEvent(Event eventToExecute)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                var response = await client.PutAsXmlAsync("api/order/executeevent", eventToExecute);
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
                try
                {
                    client.BaseAddress = BaseAddress;
                    var response = await client.GetAsync("api/order/ordersWithSortedEvents", new CancellationToken());
                    var ordersReceived = await response.Content.ReadAsAsync<List<Order>>();
                    response.EnsureSuccessStatusCode();
                    return ordersReceived;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public async Task<List<Item>> GetItems() //Needs to be called only one time.
        {
            var items = new List<Item> {
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
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = BaseAddress;
                    var response = await client.GetAsync("api/order/items", new CancellationToken());
                    var itemsReceived = await response.Content.ReadAsAsync<List<Item>>();
                    response.EnsureSuccessStatusCode();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return items;
        }
    }
}
