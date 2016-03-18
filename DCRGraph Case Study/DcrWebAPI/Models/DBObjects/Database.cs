namespace DcrWebAPI.Models.DBObjects
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Database : DbContext
    {
        public Database()
            : base("name=Database")
        {
            System.Data.Entity.Database.SetInitializer<Database>(new DropCreateDatabaseAlways<Database>());
        }

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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(e => e.Items)
                .WithRequired(e => e.Category)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.Orders)
                .WithOptional(e => e.Customer)
                .HasForeignKey(e => e.Customer_Id);

            modelBuilder.Entity<DCREvent>()
                .HasMany(e => e.EventUIElemements)
                .WithRequired(e => e.DCREvent)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DCREvent>()
                .HasMany(e => e.DCREvents1)
                .WithMany(e => e.DCREvents)
                .Map(m => m.ToTable("Children").MapLeftKey("ToEventId").MapRightKey("FromEventId"));

            modelBuilder.Entity<DCREvent>()
                .HasMany(e => e.DCREvents11)
                .WithMany(e => e.DCREvents2)
                .Map(m => m.ToTable("Conditions").MapLeftKey("ToEventId").MapRightKey("FromEventId"));

            modelBuilder.Entity<DCREvent>()
                .HasMany(e => e.Groups)
                .WithMany(e => e.DCREvents)
                .Map(m => m.ToTable("EventGroups").MapLeftKey("DCREventId").MapRightKey("GroupId"));

            modelBuilder.Entity<DCREvent>()
                .HasMany(e => e.Roles)
                .WithMany(e => e.DCREvents)
                .Map(m => m.ToTable("EventRoles").MapLeftKey("DCREventId").MapRightKey("RoleId"));

            modelBuilder.Entity<DCREvent>()
                .HasMany(e => e.DCREvents12)
                .WithMany(e => e.DCREvents3)
                .Map(m => m.ToTable("Excludes").MapLeftKey("FromEventId").MapRightKey("ToDCREventId"));

            modelBuilder.Entity<DCREvent>()
                .HasMany(e => e.DCRGraphs)
                .WithMany(e => e.DCREvents)
                .Map(m => m.ToTable("GraphEvents").MapLeftKey("DCREventId").MapRightKey("DCRGraphId"));

            modelBuilder.Entity<DCREvent>()
                .HasMany(e => e.DCREvents13)
                .WithMany(e => e.DCREvents4)
                .Map(m => m.ToTable("Includes").MapLeftKey("FromEventId").MapRightKey("ToEventId"));

            modelBuilder.Entity<DCREvent>()
                .HasMany(e => e.DCREvents14)
                .WithMany(e => e.DCREvents5)
                .Map(m => m.ToTable("Milestones").MapLeftKey("ToEventId").MapRightKey("FromEventId"));

            modelBuilder.Entity<DCREvent>()
                .HasMany(e => e.DCREvents15)
                .WithMany(e => e.DCREvents6)
                .Map(m => m.ToTable("Responses").MapLeftKey("ToEventId").MapRightKey("FromEventId"));

            modelBuilder.Entity<DCRGraph>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.DCRGraph)
                .HasForeignKey(e => e.DCRGraph_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IntegerSpecifyingUIElement>()
                .HasMany(e => e.EventUIElemements)
                .WithRequired(e => e.IntegerSpecifyingUIElement)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Item>()
                .HasMany(e => e.OrderDetails)
                .WithRequired(e => e.Item)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.OrderDetails)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);
        }
    }
}
