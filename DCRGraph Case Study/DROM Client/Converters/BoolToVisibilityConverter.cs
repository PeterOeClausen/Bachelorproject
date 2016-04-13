using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace DROM_Client.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool visible = (bool)value;
            if (visible)
            {
                return "Visible";
            }
            else return "Collapsed";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
            //string visible = (string)value;
            //if (visible == "Visible") return true;
            //else return false;
        }
    }
}
