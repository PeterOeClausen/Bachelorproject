using DROM_Client.Models.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DROM_Client.Models.NewOrderData;
using DROM_Client.Services;
using DROM_Client.Models.ObjectsOptimizedForUI;
using System.Collections.ObjectModel;

namespace DROM_Client.ViewModels
{
    public class CreateOrderPageViewModel
    {
        private APICaller _APICaller { get; set; }
        public ObservableCollection<Item> ItemCollection {get; set;}

        public CreateOrderPageViewModel()
        {
            ItemCollection = new ObservableCollection<Item>();
            this._APICaller = new APICaller();
            List<Item> items = _APICaller.GetItems();
            foreach (Item i in items)
            {
                ItemCollection.Add(i);
            }
            //getItems();

            //ItemCollection = new ObservableCollection<Item>()
            //{
            //    new Item
            //    {
            //        Name = "Cola"
            //    },
            //    new Item
            //    {
            //        Name = "Sprite"
            //    },
            //    new Item
            //    {
            //        Name = "Pizza"
            //    },
            //    new Item
            //    {
            //        Name = "Burger"
            //    }
            //};

            //Change to get items from APICaller.
        }

        private async void getItems()
        {
            List<Item> items = _APICaller.GetItems();
        }
        
        public UINewOrderInfo OrderBeingCreated { get; set; } = new UINewOrderInfo() //Just bindable data for design
        {
            //Id = 2,
            ItemsAndQuantity = new Dictionary<Item, int>() {
                    {
                        new Item() {
                            Id = 3,
                            Name = "Sprite",
                            Category = "Drink",
                            Price = 30.0,
                            Description = "Soda"
                        },2
                    }
                },
            Customer = new Customer()
            {
                Id = 4,
                FirstAndMiddleNames = "Fjong",
                LastName = "Fjongson",
                Email = "Fjong@Fjongson.com",
                Phone = 22222222,
                StreetAndNumber = "Qwerty Road 66",
                ZipCode = 1234,
                City = "Amsterdam"
            },
            OrderDate = DateTime.Now,
            Notes = "With extra ice please",
            Table = 1,
            OrderType = "To be served"            
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
            foreach (KeyValuePair<Item, int> entry in OrderBeingCreated.ItemsAndQuantity)
            {
                replacementDictionary.Add(entry.Key, entry.Value);
            }
            replacementDictionary.Add(item, quantity);
            OrderBeingCreated.ItemsAndQuantity = replacementDictionary;
        }

        internal async void SaveOrder()
        {
            NewOrderInfo createdOrder = new NewOrderInfo()
            {
                ItemsAndQuantity = OrderBeingCreated.ItemsAndQuantity,
                OrderType = OrderBeingCreated.OrderType,
                Customer = OrderBeingCreated.Customer,
                OrderDate = OrderBeingCreated.OrderDate,
                Notes = OrderBeingCreated.Notes,
                Table = OrderBeingCreated.Table
            };
            await _APICaller.PostOrderAsync(createdOrder);
        }
    }
}
