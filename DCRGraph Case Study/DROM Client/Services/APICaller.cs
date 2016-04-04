using DROM_Client.Models.NewOrderData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;


namespace DROM_Client.Services
{
    public class APICaller
    {
        public async Task<string> PostOrderAsync(NewOrderInfo newOrder)
        {
            using (var client = new HttpClient())
            {
                try
                {

                
                client.BaseAddress = new Uri("http://localhost:57815/");
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
    }
}
