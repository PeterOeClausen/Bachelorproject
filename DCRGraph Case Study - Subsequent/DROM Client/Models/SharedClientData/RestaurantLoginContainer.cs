using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DROM_Client.Models.SharedClientData
{
    public class RestaurantLoginContainer
    {
        #region Singleton pattern
        private static RestaurantLoginContainer instance;
        private RestaurantLoginContainer() { }
        public static RestaurantLoginContainer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RestaurantLoginContainer();
                }
                return instance;
            }
        }
        #endregion

        public int RestaurantId { get; set; }
    }
}
