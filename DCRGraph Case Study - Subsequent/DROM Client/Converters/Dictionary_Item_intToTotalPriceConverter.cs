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
    /// Converter class to convert a Dictionary<Item,int> to a total price. Item must have a price, int must be the quantity of the item.
    /// </summary>
    public class Dictionary_Item_intToTotalPriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var dictionary = value as Dictionary<Item, int>;
            if (dictionary == null)
            {
                return "0";
            }
            else
            {
                double totalPrice = 0.0;
                foreach (var itemQuantity in dictionary) //Loop summing up totalprice
                {
                    totalPrice += ((itemQuantity.Key as Item).Price * itemQuantity.Value);
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
