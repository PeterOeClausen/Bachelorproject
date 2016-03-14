using System;
using System.Collections.Generic;
using System.Linq;

namespace DcrWebAPI.Models.BusinessObjects
{
    //HEY! Please talk to Peter/Johan before changing!
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public CategoryEnum Category { get; set; }
        public enum CategoryEnum { Burger, Drinks }
        public string Description { get; set; }
        
        public Item()
        {
            this.Category = CategoryEnum.Burger;
            this.Name = Category.ToString(); //Does this work? Have to check later...
        }
    }
}