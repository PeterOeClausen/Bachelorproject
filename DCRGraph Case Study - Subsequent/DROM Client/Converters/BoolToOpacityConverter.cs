using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace DROM_Client.Converters
{
    /// <summary>
    /// Bool to opacity converter. Used to convert er bool to specific opacity values. Used in view on buttons.
    /// </summary>
    public class BoolToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool boolean = (bool)value;
            if (boolean)
            {
                return "1"; //Solid
            }
            else return "0.1"; //See through
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
