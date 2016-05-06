using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DROM_Client.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private int _restaurantId;



        private void LogInButtonClick(object sender, RoutedEventArgs e)
        {
            //Check login here
            if (_restaurantId != 0)
            {
                Frame.Navigate(typeof(OrderPage),_restaurantId);
            }
            else CreateAndShowMessageDialog("You must choose a restaurant before you can proceed.");
        }

        private void Restaurant_Number_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var choosenRestaurantItem = e.AddedItems.First() as ComboBoxItem;
            int.TryParse(choosenRestaurantItem.Content as string, out _restaurantId);
        }

        private async void CreateAndShowMessageDialog(string message)
        {
            var messageDialog = new MessageDialog(message);
            messageDialog.CancelCommandIndex = 0;
            await messageDialog.ShowAsync();
        }
    }
}
