﻿using DROM_Client.Models.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DROM_Client.Models.NewOrderData;
using DROM_Client.Services;
using DROM_Client.Models.ObjectsOptimizedForUI;
using System.Collections.ObjectModel;
using Windows.UI.Popups;
using DROM_Client.Models.SharedClientData;

namespace DROM_Client.ViewModels
{
    public class CreateOrderPageViewModel
    {
        private APICaller _APICaller { get; set; }
        public ObservableCollection<Item> ItemCollection {get; set;}
        public List<string> DeliveryMethodsList { get; set; }

        public CreateOrderPageViewModel()
        {
            ItemCollection = new ObservableCollection<Item>();
            this._APICaller = new APICaller();

            //Getting items from web api
            Tuple<bool, string, List<Item>> ItemsFromWebAPI = _APICaller.GetItems();
            if (ItemsFromWebAPI.Item1 == false) CreateAndShowMessageDialog(ItemsFromWebAPI.Item2);
            foreach (Item i in ItemsFromWebAPI.Item3)
            {
                ItemCollection.Add(i);
            }

            //Getting delivery types from web api
            Tuple<bool, string, List<string>> deliveryTypesFromWebAPI = _APICaller.GetDeliveryTypes();
            if (deliveryTypesFromWebAPI.Item1 == false) CreateAndShowMessageDialog(deliveryTypesFromWebAPI.Item2);
            DeliveryMethodsList = deliveryTypesFromWebAPI.Item3;
        }

        public UINewOrderInfo OrderBeingCreated { get; set; } = new UINewOrderInfo() //Just bindable data for design
        {
            ItemsAndQuantity = new Dictionary<Item, int>(),
            Customer = new Customer()
            {
                FirstAndMiddleNames = "",
                LastName = "",
                Email = "",
                StreetAndNumber = "",
                City = "",
            },
            OrderDate = DateTime.Now,
            Notes = ""
        };
        
        /// <summary>
        /// RemoveItem takes a key item specified in OrderBeingCreated.ItemsAndQuatity and copies all values from old dictionary into a new dictionary, and replaces the reference.
        /// Note: This causes PropertyChanged update in UI.
        /// </summary>
        /// <param name="key"></param>
        internal void RemoveItem(Item key)
        {
            Dictionary<Item, int> replacementDictionary = new Dictionary<Item, int>();
            foreach (KeyValuePair<Item, int> entry in OrderBeingCreated.ItemsAndQuantity)
            {
                if (entry.Key.Equals(key))  continue;
                else replacementDictionary.Add(entry.Key, entry.Value);
            }
            OrderBeingCreated.ItemsAndQuantity = replacementDictionary;
        }

        /// <summary>
        /// AddQuantityAndItem takes an int quantity and an Item (this needs to exist on webserver), copies all values from old dictionary into a new dictionary, adds the new item, and replaces the refference.
        /// Note: This causes PropertyChanged update in UI.
        /// </summary>
        /// <param name="quantity"></param>
        /// <param name="item"></param>
        internal void AddQuantityAndItem(int quantity, Item item)
        {
            Dictionary<Item, int> replacementDictionary = new Dictionary<Item, int>();
            foreach (KeyValuePair<Item, int> entry in OrderBeingCreated.ItemsAndQuantity) //Copy old dictionary
            {
                replacementDictionary.Add(entry.Key, entry.Value);
            }
            if (!replacementDictionary.ContainsKey(item)) //If item is not in dictionary
            {
                replacementDictionary.Add(item, quantity); //Add it
            }
            else //else update the value of item
            {
                int quan;
                replacementDictionary.TryGetValue(item, out quan);
                replacementDictionary.Remove(item);
                replacementDictionary.Add(item, quan + quantity);
            }
            OrderBeingCreated.ItemsAndQuantity = replacementDictionary;
        }

        internal Tuple<bool, string> SaveOrder()
        {
            NewOrderInfo createdOrder = new NewOrderInfo()
            {
                ItemsAndQuantity = OrderBeingCreated.ItemsAndQuantity,
                OrderType = OrderBeingCreated.OrderType,
                Customer = OrderBeingCreated.Customer,
                OrderDate = OrderBeingCreated.OrderDate,
                Notes = OrderBeingCreated.Notes,
                Table = OrderBeingCreated.Table,
                Restaurant = RestaurantLoginContainer.Instance.RestaurantId
            };
            return _APICaller.PostOrderAsync(createdOrder);
        }

        private async void CreateAndShowMessageDialog(string message)
        {
            var messageDialog = new MessageDialog(message);
            messageDialog.CancelCommandIndex = 0;
            await messageDialog.ShowAsync();
        }
    }
}
