using DROM_Client.Models.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace DROM_Client.Converters
{
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
                foreach(var itemQuantity in order.ItemsAndQuantity)
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
