﻿using DROM_Client.Services;
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

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(OrderPage));
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(OrderPage));
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private async void Send_Test_Post_Call(object sender, RoutedEventArgs e)
        {
            APICaller apiCaller = new APICaller();
            await apiCaller.PostOrderAsync(new NewOrderInfo()
            {
                Table = "1",
                Customer = new Customer()
                {
                    FirstAndMiddleNames = "Bob",
                    LastName = "Bobson",
                    City = "TestCity",
                    Email = "Test@mail.dk",
                    Phone = 12121212,
                    StreetAndNumber = "testgade 12",
                    ZipCode = 2300
                },
                ItemsAndQuantity = new Dictionary<Item, int>()
                {
                    {new Item()
                    {
                        Name = "Cheese Burger",
                        Price = 50,
                        Category = "Burger",
                        Description = "Burger with cheese"
                    }, 2},
                    {new Item()
                    {
                        Name = "Coca Cola",
                        Price = 30,
                        Category = "Drink",
                        Description = "Coca Cola bottle 50cl"
                    }, 4 }
                },
                Notes = "this is a very special and very testy order",
                OrderType = "Delivery",
                OrderDate = DateTime.Now
            });
        }
    }
}
