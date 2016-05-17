using DROM_Client.Models.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace DROM_Client.Converters
{
    /// <summary>
    /// Class for converting an Order object to a total price.
    /// </summary>
    public class OrderToTotalPriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var order = value as Order;
            if (order == null)
            {
                return "0";
            }
            else
            {
                double totalPrice = 0.0;
                foreach(var itemQuantity in order.ItemsAndQuantity) //Loop summing total price.
                {
                    totalPrice += (itemQuantity.Item.Price * itemQuantity.Quantity);
                }
                return totalPrice.ToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
