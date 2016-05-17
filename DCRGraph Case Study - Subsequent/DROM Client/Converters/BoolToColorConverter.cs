using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace DROM_Client.Converters
{
    /// <summary>
    /// Converter class used to convert from bool to specific colors. Used in view to color a button if a boolean is true (blue) or false (LightGray).
    /// </summary>
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool enabled = (bool)value;
            if (enabled)
            {
                return "#428bca"; //DCRGraph Blue
            }
            else
            {
                return "LightGray";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
