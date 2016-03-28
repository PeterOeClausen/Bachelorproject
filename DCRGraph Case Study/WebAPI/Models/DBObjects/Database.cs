namespace WebAPI.Models.DBObjects
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class Database : DbContext
    {
        // Your context has been configured to use a 'Database' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'WebAPI.Models.DBObjects.Database' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'Database' 
        // connection string in the application configuration file.
        public Database()
            : base("name=Database")
        {
            
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<DCREvent> DCREvents { get; set; }
        public virtual DbSet<DCRGraph> DCRGraphs { get; set; }
        public virtual DbSet<EventUIElemement> EventUIElemements { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<IntegerSpecifyingUIElement> IntegerSpecifyingUIElements { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Role> Roles { get; set; }

    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}