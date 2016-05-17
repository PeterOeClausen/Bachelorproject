using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DROM_Client.Models.SharedClientData
{
    /// <summary>
    /// Singleton class used to store id of restaurant from login screen. Used instead of sending data between views. Simpler than creating a viewmodel for LoginView.
    /// </summary>
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
