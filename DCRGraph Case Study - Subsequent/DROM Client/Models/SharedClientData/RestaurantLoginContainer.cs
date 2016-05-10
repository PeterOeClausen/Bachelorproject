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
        private static RestaurantLoginContainer _instance;
        private RestaurantLoginContainer() { }
        public static RestaurantLoginContainer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new RestaurantLoginContainer();
                }
                return _instance;
            }
        }
        #endregion

        public int RestaurantId { get; set; }
    }
}
