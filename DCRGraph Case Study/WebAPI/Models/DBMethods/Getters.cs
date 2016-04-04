using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Models.DBObjects;

namespace WebAPI.Models.DBMethods
{
    public class Getters
    {

        public async Task<List<Item>> GetItems()
        {
            using (var db = new Database())
            {
                var items = db.Items;
                List<Item> itemList = new List<Item>();
                foreach (var i in items)
                {
                    itemList.Add(i);
                }
                return itemList;
            }
        }

    }
}
