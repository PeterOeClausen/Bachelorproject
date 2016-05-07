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
using DROM_Client.Models.BusinessObjects;
using DROM_Client.ViewModels;
using Windows.UI.Popups;
using DROM_Client.Models.ObjectsOptimizedForUI;
using System.Collections.ObjectModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DROM_Client.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditOrderPage : Page
    {        
        public EditOrderPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var viewModel = this.DataContext as EditOrderPageViewModel;
            Order orderReceived = e.Parameter as Order;
            viewModel.OrderBeingEdited = new UIOrder
            {
                Id = orderReceived.Id,
                Customer = orderReceived.Customer,
                OrderDate = orderReceived.OrderDate,
                Notes = orderReceived.Notes,
                DCRGraph = new UIDCRGraph { Events = new ObservableCollection<Event>()},
                ItemsAndQuantity = new ObservableCollection<ItemQuantity>(),
                Table = orderReceived.Table,
                OrderType = orderReceived.OrderType,
            };

            if(orderReceived.Customer == null)
            {
                viewModel.OrderBeingEdited.Customer = new Customer()
                {
                    FirstAndMiddleNames = "",
                    LastName = "",
                    Email = "",
                    StreetAndNumber = "",
                    City = ""
                };
            }

            foreach (Event evnt in orderReceived.DCRGraph.Events)
            {
                if(evnt.Groups.Exists(g => g.Name == "Edit events") && !evnt.Groups.Exists(g => g.Name == "Hidden edit events")) //Filters to only "Edit events"
                {
                    viewModel.OrderBeingEdited.DCRGraph.Events.Add(evnt);
                }
                if (evnt.Groups.Exists(g => g.Name == "Hidden edit events")) viewModel.ItemsOnOrderHasBeenChangedEvent = evnt;
            }

            foreach (var entry in orderReceived.ItemsAndQuantity)
            {
                viewModel.OrderBeingEdited.ItemsAndQuantity.Add(new ItemQuantity()
                {
                    Item = entry.Item,
                    Quantity = entry.Quantity
                });
            }
        }

        //From microsoft guide: https://msdn.microsoft.com/da-dk/library/windows/apps/xaml/br208674?cs-save-lang=1&cs-lang=csharp
        /// <summary>
        /// Event handler for 'Save' button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            #region Checks if all information is entered correctly before asking for save:
            var viewModel = this.DataContext as EditOrderPageViewModel;
            if (viewModel.OrderBeingEdited.ItemsAndQuantity.Count == 0)
            {
                CreateAndShowMessageDialog("Sorry, an order must have items on order.");
                return;
            }
            switch (viewModel.OrderBeingEdited.OrderType as string) //Check if all information is entered
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

        /// <summary>
        /// Saves order.
        /// </summary>
        /// <param name="command"></param>
        private async void Save_Popup_Yes(IUICommand command)
        {
            var viewModel = this.DataContext as EditOrderPageViewModel;
            Tuple<bool, string> answerFromWebApi = viewModel.SaveOrder();
            if (answerFromWebApi.Item1 == false) CreateAndShowMessageDialog(answerFromWebApi.Item2); //Shows error message if API failed.
            Frame.Navigate(typeof(OrderPage));
        }

        private void Save_Popup_No(IUICommand command)
        {
            //Do nothing
        }

        #region Checks to ensure that all information has been entered

        /// <summary>
        /// Checks if all information is entered for serving.
        /// </summary>
        /// <returns></returns>
        private bool All_Information_Entered_For_Serving()
        {
            if (Table_Number_Text_Box.Text == "")
            {
                CreateAndShowMessageDialog("Please enter a table number to serve to.");
                return false;
            }
            int tableNumber;
            if (!(int.TryParse(Table_Number_Text_Box.Text, out tableNumber)))
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

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(OrderPage));
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if(this.Item_Box.SelectedItem == null)
            {
                CreateAndShowMessageDialog("You have to select an item to add.");
                return;
            }
            var selectedItem = Item_Box.SelectedItem as Item;
            string quanAsString = this.Quantity_Box.Text;
            int quanAsInt;
            if (int.TryParse(quanAsString, out quanAsInt))
            {
                var viewModel = this.DataContext as EditOrderPageViewModel;
                viewModel.AddItemQuantity(selectedItem, quanAsInt);
                viewModel.ItemsOnOrderHasBeenChanged = true;
            }
            else
            {
                CreateAndShowMessageDialog("Quantity needs to be an integer value.");
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (this.Items_On_Order_List_View.SelectedItems.Count == 1)
            {
                var selectedItemQuantity = Items_On_Order_List_View.SelectedItem as ItemQuantity;
                var viewModel = this.DataContext as EditOrderPageViewModel;
                viewModel.RemoveItemQuantity(selectedItemQuantity);
                viewModel.ItemsOnOrderHasBeenChanged = true;
            }
            else
            {
                CreateAndShowMessageDialog("You need to select an item from the order to remove.");
            }
        }

        private void Edit_Event_Execute_Click(object sender, RoutedEventArgs e)
        {
            var eventToExecute = ((Button)sender).Tag as Event;
            var viewModel = this.DataContext as EditOrderPageViewModel;
            viewModel.EditEventsToExecute.Clear();
            viewModel.EditEventsToExecute.Add(eventToExecute);
            viewModel.OrderBeingEdited.OrderType = eventToExecute.Label; //ConvertEventToOrdertypeString(eventToExecute);
            CreateAndShowMessageDialog("Order type will be saved as: '" + eventToExecute.Label + "' when you save.");
        }

        //To be deleted:
        //private string ConvertEventToOrdertypeString (Event e)
        //{
        //    switch (e.Label)
        //    {
        //        case "Change to takeaway": return "For takeaway";
        //        case "Change to delivery": return "For delivery";
        //        case "Change to serve": return "For serving";
        //        default: return null;
        //    }
        //}
        
        private async void CreateAndShowMessageDialog(string message)
        {
            var messageDialog = new MessageDialog(message);
            messageDialog.CancelCommandIndex = 0;
            await messageDialog.ShowAsync();
        }
    }
}
