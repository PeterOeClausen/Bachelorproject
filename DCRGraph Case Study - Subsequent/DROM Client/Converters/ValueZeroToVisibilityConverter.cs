using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace DROM_Client.Converters
{
    /// <summary>
    /// Class for converting int value to a visible value. Used in view to hide value 0, to not show anything if value is 0.
    /// </summary>
    public class ValueZeroToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int valueReceived = (int)value;
            if (valueReceived == 0) return "Collapsed";
            else return "Visible";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
