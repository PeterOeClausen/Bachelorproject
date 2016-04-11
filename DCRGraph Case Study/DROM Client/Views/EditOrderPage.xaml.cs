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

            foreach (Event evnt in orderReceived.DCRGraph.Events)
            {
                if(evnt.Groups.Exists(g => g.Name == "Edit events")) //Filters to only "Edit events"
                {
                    viewModel.OrderBeingEdited.DCRGraph.Events.Add(evnt);
                }
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

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as EditOrderPageViewModel;
            viewModel.SaveOrder();
            Frame.Navigate(typeof(OrderPage));
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(OrderPage));
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string quanAsString = this.Quantity_Box.Text;
            int quanAsInt;
            if (int.TryParse(quanAsString, out quanAsInt))
            {
                var NewItem = this.Item_Box.SelectedItem as Item;
                var viewModel = this.DataContext as EditOrderPageViewModel;
                viewModel.AddQuantityAndItem(quanAsInt, NewItem);
            }
            else
            {
                CreateAndShowMessageDialog("Quantity needs to be an integer value.");
            }
        }

        private async void CreateAndShowMessageDialog(string message)
        {
            var messageDialog = new MessageDialog(message);
            messageDialog.CancelCommandIndex = 0;
            await messageDialog.ShowAsync();
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (this.Items_On_Order_List_View.SelectedItems.Count == 1)
            {
                var selected = (KeyValuePair<Item, int>)this.Items_On_Order_List_View.SelectedItem;

                var viewModel = this.DataContext as EditOrderPageViewModel;
                viewModel.RemoveItem(selected.Key);
            }
            else
            {
                CreateAndShowMessageDialog("You need to select one and only one item from the list above.");
            }
        }

        private void Edit_Event_Execute_Click(object sender, RoutedEventArgs e)
        {
            var eventToExecute = ((Button)sender).Tag as Event;
            var viewModel = this.DataContext as EditOrderPageViewModel;
            viewModel.EditEventToExecute = eventToExecute;
            //Update UI for delivery method.
        }
    }
}
