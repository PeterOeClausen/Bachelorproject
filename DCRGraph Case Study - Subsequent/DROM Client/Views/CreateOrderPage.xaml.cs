using DROM_Client.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DROM_Client.Models.NewOrderData;
using DROM_Client.Models.BusinessObjects;
using Windows.UI.Popups;
using DROM_Client.ViewModels;
using System.Threading.Tasks;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DROM_Client.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateOrderPage : Page
    {
        public CreateOrderPage()
        {
            this.InitializeComponent();
        }

        #region Save click and Popups and cancel
        //From microsoft guide: https://msdn.microsoft.com/da-dk/library/windows/apps/xaml/br208674?cs-save-lang=1&cs-lang=csharp
        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            #region Checks if all information is entered correctly before asking for save:
            var viewModel = DataContext as CreateOrderPageViewModel;
            if (DeliveryCombobox.SelectedItem == null) //If no Delivery method is selected
            {
                CreateAndShowMessageDialog("Please select a delivery method.");
                return;
            }
            if (viewModel.OrderBeingCreated.ItemsAndQuantity.Count == 0) //If no items are on order
            {
                CreateAndShowMessageDialog("Sorry, an order must have items on order.");
                return;
            }

            switch (DeliveryCombobox.SelectedItem as string) // Check if all information is entered
            {
                case "For serving":
                    if (!All_Information_Entered_For_Serving()) return;
                    break;
                case "For takeaway":
                    if (!All_Information_Entered_For_Pickup()) return;
                    break;
                case "For delivery":
                    if (!All_Information_Entered_For_Delivery()) return;
                    break;
                case "Bulk order":
                    if (!All_Information_Entered_For_Delivery()) return; //Same requirements as delivery
                    break;
            }
            #endregion

            // Create the message dialog and set its content
            var messageDialog = new MessageDialog("Do you want to save this order?");

            // Add commands and set their callbacks; both buttons use the same callback function instead of inline event handlers
            messageDialog.Commands.Add(new UICommand("Yes",
                new UICommandInvokedHandler(this.Save_Popup_Yes)));
            messageDialog.Commands.Add(new UICommand("No",
                new UICommandInvokedHandler(this.Save_Popup_No)));

            // Set the command to be invoked when escape is pressed
            messageDialog.CancelCommandIndex = 1;

            // Show the message dialog
            await messageDialog.ShowAsync();
        }

        private async void Save_Popup_Yes(IUICommand command)
        {
            var viewModel = DataContext as CreateOrderPageViewModel;
            Tuple<bool,string> answer = viewModel.SaveOrder();
            if (answer.Item1 == false) CreateAndShowMessageDialog(answer.Item2);
            Frame.Navigate(typeof(OrderPage));
        }

        #region Checks to ensure that all information has been entered

        /// <summary>
        /// Checks if all information is entered for serving.
        /// </summary>
        /// <returns></returns>
        private bool All_Information_Entered_For_Serving()
        {
            if(Table_Number_Text_Box.Text == "")
            {
                CreateAndShowMessageDialog("Please enter a table number to serve to.");
                return false;
            }
            int tableNumber;
            if(!(int.TryParse(Table_Number_Text_Box.Text,out tableNumber)))
            {
                CreateAndShowMessageDialog("Please enter a valid table integer number.");
                return false;
            }
            if (tableNumber == 0)
            {
                CreateAndShowMessageDialog("Sorry, but you need to specify a table number that is not '0'");
                return false;
            }
            return true;
        }

        private bool All_Information_Entered_For_Pickup()
        {
            return true;
        }

        private bool All_Information_Entered_For_Delivery()
        {
            int phoneNumber;
            int zip;
            if (First_Name_Text_Box.Text == "")
            {
                CreateAndShowMessageDialog("Please enter the customer's first name.");
                return false;
            }
            else if (Last_Name_Text_Box.Text == "")
            {
                CreateAndShowMessageDialog("Please enter the customer's last name.");
                return false;
            }
            else if (Email_Text_Box.Text == "")
            {
                CreateAndShowMessageDialog("Please enter the customer's email address.");
                return false;
            }
            else if (!Email_Text_Box.Text.Contains('@'))
            {
                CreateAndShowMessageDialog("Please enter a valid email address, '@' is missing.");
                return false;
            }
            else if (Phone_Text_Box.Text == "")
            {
                CreateAndShowMessageDialog("Please enter the customer's phone number.");
                return false;
            }
            else if (!int.TryParse(Phone_Text_Box.Text, out phoneNumber))
            {
                CreateAndShowMessageDialog("Please enter a valid phone number (use integers only).");
                return false;
            }
            else if (phoneNumber == 0)
            {
                CreateAndShowMessageDialog("Sorry, but the phone number cannot be '0'.");
                return false;
            }
            else if (Street_And_Number_Text_Box.Text == "")
            {
                CreateAndShowMessageDialog("Please enter the customer's street and number.");
                return false;
            }
            else if (Zip_Text_Box.Text == "")
            {
                CreateAndShowMessageDialog("Please enter the customer's zip code.");
                return false;
            }
            else if (!int.TryParse(Zip_Text_Box.Text, out zip))
            {
                CreateAndShowMessageDialog("Please enter a valid zip code (integers only).");
                return false;
            }
            else if (City_Text_Box.Text == "")
            {
                CreateAndShowMessageDialog("Please enter the customer's city.");
                return false;
            }
            return true;
        }

        #endregion

        private async void CreateAndShowMessageDialog(string message)
        {
            var messageDialog = new MessageDialog(message);
            messageDialog.CancelCommandIndex = 0;
            await messageDialog.ShowAsync();
        }
        
        private void Save_Popup_No(IUICommand command)
        {
            //Do nothing
        }
        
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(OrderPage));
        }
        #endregion

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            string quanAsString = this.Quantity_Box.Text;
            int quanAsInt;
            if(int.TryParse(quanAsString, out quanAsInt))
            {
                var NewItem = this.Item_Box.SelectedItem as Item;
                var viewModel = this.DataContext as CreateOrderPageViewModel;
                viewModel.AddQuantityAndItem(quanAsInt, NewItem);
            }
            else
            {
                var messageDialog = new MessageDialog("Quantity needs to be an integer value.");
                await messageDialog.ShowAsync();
            }
        }

        private async void Remove_Click(object sender, RoutedEventArgs e)
        {
            if(this.Items_On_Order_List_View.SelectedItems.Count == 1)
            {
                var selected = (KeyValuePair<Item, int>) this.Items_On_Order_List_View.SelectedItem;

                var viewModel = this.DataContext as CreateOrderPageViewModel;
                viewModel.RemoveItem(selected.Key);
            }
            else
            {
                CreateAndShowMessageDialog("You need to select one and only one item from the list above.");
            }
        }

        private void DeliveryCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = this.DataContext as CreateOrderPageViewModel;
            viewModel.OrderBeingCreated.OrderType = e.AddedItems.First() as string;
        }
    }
}
