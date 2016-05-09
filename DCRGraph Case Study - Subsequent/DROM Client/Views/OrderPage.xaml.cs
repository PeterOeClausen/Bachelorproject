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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DROM_Client.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OrderPage : Page
    {
        public OrderPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var viewModel = DataContext as OrderPageViewModel;
            viewModel.setupData();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = ((Button)sender).Tag as Order;
            var viewModel = DataContext as OrderPageViewModel;
            Order originalOrder = viewModel.OrdersFromWebAPI.Find(o => o.Id == selectedOrder.Id);
            Frame.Navigate(typeof(EditOrderPage), originalOrder);
        }

        private void Create_New_Order_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CreateOrderPage));
        }

        #region View selection buttons
        private void Chef_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = ((Button)sender).DataContext as OrderPageViewModel;
            if (viewModel.Chef) viewModel.Chef = false;
            else viewModel.Chef = true;

        }

        private void Delivery_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = ((Button)sender).DataContext as OrderPageViewModel;
            if (viewModel.Delivery) viewModel.Delivery = false;
            else viewModel.Delivery = true;
        }

        private void Manger_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = ((Button)sender).DataContext as OrderPageViewModel;
            if (viewModel.Manager) viewModel.Manager = false;
            else viewModel.Manager = true;
        }

        private void Waiter_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = ((Button)sender).DataContext as OrderPageViewModel;
            if (viewModel.Waiter) viewModel.Waiter = false;
            else viewModel.Waiter = true;
        }

        private void Show_Only_Pending_Orders_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = ((Button)sender).DataContext as OrderPageViewModel;
            if (viewModel.ShowOnlyPendingOrders) viewModel.ShowOnlyPendingOrders = false;
            else viewModel.ShowOnlyPendingOrders = true;
        }
        #endregion

        private void Execute_Event_Click(object sender, RoutedEventArgs e)
        {
            var EventToExecute = ((Button)sender).Tag as Event;
            var ViewModel = DataContext as OrderPageViewModel;
            Tuple<bool, string> answer = ViewModel.ExecuteEvent(EventToExecute);
            if (answer.Item1 == false) CreateAndShowMessageDialog(answer.Item2); //If API fails, create popup message.
            ViewModel.setupData();
        }

        private void Archive_Click(object sender, RoutedEventArgs e)
        {
            var orderToBeArchived = ((Button)sender).Tag as Order;
            if (orderToBeArchived.AcceptingState)
            {
                var viewModel = DataContext as OrderPageViewModel;
                viewModel.ArchiveOrder(orderToBeArchived);
                viewModel.setupData();
            }
            else CreateAndShowMessageDialog("The order process needs to be finnished before you can archive the order. Check to see if other views can execute these events.");
        }

        private void GetOrdersFromWebAPI_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as OrderPageViewModel;
            Tuple<bool, string, List<Order>> answerFromViewModel = viewModel.setupData();
            if (answerFromViewModel.Item1 == false) CreateAndShowMessageDialog(answerFromViewModel.Item2);
            else if (answerFromViewModel.Item3.Count == 0) CreateAndShowMessageDialog("No orders were found on the Web API. Please 'Create new order', or click 'Get orders from Web API' again later.");
        }

        //private void TempAddOrderClick(object sender, RoutedEventArgs e)
        //{
        //    var ViewModel = DataContext as OrderPageViewModel;
        //    ViewModel.OrderList.Add(new Order { Id = 9001});
        //}

        private async void CreateAndShowMessageDialog(string message)
        {
            var messageDialog = new MessageDialog(message);
            messageDialog.CancelCommandIndex = 0;
            await messageDialog.ShowAsync();
        }

        private Order orderToBeDeleted;
        private async void Delete_Order_Click(object sender, RoutedEventArgs e)
        {
            orderToBeDeleted = ((Button)sender).Tag as Order;
            // Create the message dialog and set its content
            var messageDialog = new MessageDialog("Are you sure you want to delete this order?");

            // Add commands and set their callbacks; both buttons use the same callback function instead of inline event handlers
            messageDialog.Commands.Add(new UICommand("Yes",
                new UICommandInvokedHandler(this.delete_Popup_Yes)));
            messageDialog.Commands.Add(new UICommand("No",
                new UICommandInvokedHandler(this.delete_Popup_No)));

            // Set the command to be invoked when escape is pressed
            messageDialog.CancelCommandIndex = 1;

            // Show the message dialog
            await messageDialog.ShowAsync();
        }

        private async void delete_Popup_Yes(IUICommand command)
        {
            var viewModel = DataContext as OrderPageViewModel;
            viewModel.DeleteOrder(orderToBeDeleted);
            viewModel.setupData();
        }

        private async void delete_Popup_No(IUICommand command)
        {
            //Do nothing
        }
    }
}
