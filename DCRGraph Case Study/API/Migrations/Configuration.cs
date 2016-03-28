

using API.Models.DBObjects;

namespace API.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<API.Models.DBObjects.Database>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(API.Models.DBObjects.Database context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            var cat0 = new Category()
            {

                Name = "Burger"
            };
            var cat1 = new Category()
            {

                Name = "Pizza"
            };
            var cat2 = new Category()
            {

                Name = "Drink"
            };
            context.Categories.AddOrUpdate(
                a => a.Name,
                cat0);
            context.Categories.AddOrUpdate(
                a => a.Name,
                cat2);
            context.Categories.AddOrUpdate(
                a => a.Name,
                cat1);

            var item0 = new Item()
            {

                Name = "Koebenhavn",
                Category = cat0,
                Price = 50.50,
                Description = "Inspired by the culture in Copenhagen"
            };
            var item1 = new Item()
            {

                Name = "Liverpool",
                Category = cat0,
                Price = 60.00,
                Description = "Inspired by the culture in Liverpool"
            };
            var item2 = new Item()
            {

                Name = "Kreta",
                Category = cat0,
                Price = 50.00,
                Description = "Inspired by the culture on Kreta"
            };
            var item3 = new Item()
            {

                Name = "Lone Star",
                Category = cat0,
                Price = 100.00,
                Description = "A good but very lonely burger"
            };
            var item4 = new Item()
            {

                Name = "Cola",
                Category = cat2,
                Price = 25.00,
                Description = "qwe"
            };
            var item5 = new Item()
            {

                Name = "Fanta",
                Category = cat2,
                Price = 25.00,
                Description = "qwe"
            };
            var item6 = new Item()
            {

                Name = "Sprite",
                Category = cat2,
                Price = 25.00,
                Description = "qwe"
            };
            context.Items.AddOrUpdate(
              a => a.Name,
              item0);
            context.Items.AddOrUpdate(
              a => a.Name,
              item1);
            context.Items.AddOrUpdate(
              a => a.Name,
              item2);
            context.Items.AddOrUpdate(
              a => a.Name,
              item3);
            context.Items.AddOrUpdate(
              a => a.Name,
              item4);
            context.Items.AddOrUpdate(
              a => a.Name,
              item5);
            context.Items.AddOrUpdate(
                a => a.Name,
                item6);

        }
    }
}
