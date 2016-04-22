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
        public List<string> DeliveryMethodsList { get; set; }

        public CreateOrderPageViewModel()
        {
            ItemCollection = new ObservableCollection<Item>();
            this._APICaller = new APICaller();
            List<Item> items = _APICaller.GetItems();
            foreach (Item i in items)
            {
                ItemCollection.Add(i);
            }
            DeliveryMethodsList = _APICaller.GetDeliveryTypes();

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

            //DeliveryMethodsList = new List<string>(){"For serving", "For delivery", "For pickup"};
        }

        private async void getItems()
        {
            List<Item> items = _APICaller.GetItems();
        }

        public UINewOrderInfo OrderBeingCreated { get; set; } = new UINewOrderInfo() //Just bindable data for design
        {
            ItemsAndQuantity = new Dictionary<Item, int>(),
            Customer = new Customer()
            {
                FirstAndMiddleNames = "",
                LastName = "",
                Email = "",
                //Phone = 0,
                StreetAndNumber = "",
                //ZipCode = 0,
                City = "",
            },
            OrderDate = DateTime.Now,
            Notes = "",
            //Table = 0
        };
        //{
        //    //Id = 2,
        //    ItemsAndQuantity = new Dictionary<Item, int>() {
        //            {
        //                new Item() {
        //                    Id = 3,
        //                    Name = "Sprite",
        //                    Category = "Drink",
        //                    Price = 30.0,
        //                    Description = "Soda"
        //                },2
        //            }
        //        },
        //    Customer = new Customer()
        //    {
        //        Id = 4,
        //        FirstAndMiddleNames = "Fjong",
        //        LastName = "Fjongson",
        //        Email = "Fjong@Fjongson.com",
        //        Phone = 22222222,
        //        StreetAndNumber = "Qwerty Road 66",
        //        ZipCode = 1234,
        //        City = "Amsterdam"
        //    },
        //    OrderDate = DateTime.Now,
        //    Notes = "With extra ice please",
        //    Table = 1,
        //    OrderType = "To be served"            
        //};

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
                Table = OrderBeingCreated.Table
            };
            return _APICaller.PostOrderAsync(createdOrder);
        }
    }
}
