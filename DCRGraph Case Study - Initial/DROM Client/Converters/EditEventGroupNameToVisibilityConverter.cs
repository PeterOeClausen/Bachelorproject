using DROM_Client.Models.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace DROM_Client.Converters
{
    public class EditEventGroupNameToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            List<Group> groups = (List<Group>)value;
            bool visible = false;
            foreach(Group g in groups)
            {
                if (g.Name == "Edit events")
                {
                    visible = true;
                }
            }
            if (visible)
            {
                return "Visible";
            }
            else return "Collapsed";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
