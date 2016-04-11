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
            var viewModel = ((Button) sender).DataContext as OrderPageViewModel;
            if (viewModel.Chef) viewModel.Chef = false;
            else viewModel.Chef = true;

        }

        private void Delivery_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = ((Button)sender).DataContext as OrderPageViewModel;
            if (viewModel.Delivery)viewModel.Delivery = false;
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
            ViewModel.ExecuteEvent(EventToExecute);
            ViewModel.setupData();
        }
        
        //private void TempAddOrderClick(object sender, RoutedEventArgs e)
        //{
        //    var ViewModel = DataContext as OrderPageViewModel;
        //    ViewModel.OrderList.Add(new Order { Id = 9001});
        //}
    }
}
