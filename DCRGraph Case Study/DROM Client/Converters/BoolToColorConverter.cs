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
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool enabled = (bool)value;
            if (enabled)
            {
                return "#428bca"; //DCRGraph Blue
                //return new SolidColorBrush(Color.FromArgb(100, 66, 139, 202));
            }
            else
            {
                return "LightGray";
                //return new SolidColorBrush(Color.FromArgb(100, 204, 204, 204)); 
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
