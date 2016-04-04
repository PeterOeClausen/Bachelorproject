using System.CodeDom;
using System.Data.Entity.Validation;

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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Includes
            modelBuilder.Entity<DCREvent>()
                .HasMany(c => c.IncludeFrom)
                .WithMany(c => c.IncludeTo)
                .Map(m =>
                {
                    m.MapLeftKey("FromId");
                    m.MapRightKey("ToId");
                    m.ToTable("Includes");
                });

            //Exludes
            modelBuilder.Entity<DCREvent>()
                .HasMany(c => c.ExcludeFrom)
                .WithMany(c => c.ExcludeTo)
                .Map(m =>
                {
                    m.MapLeftKey("FromId");
                    m.MapRightKey("ToId");
                    m.ToTable("Excludes");
                });

            //Responses
            modelBuilder.Entity<DCREvent>()
                .HasMany(c => c.ResponseFrom)
                .WithMany(c => c.ResponseTo)
                .Map(m =>
                {
                    m.MapLeftKey("FromId");
                    m.MapRightKey("ToId");
                    m.ToTable("Responses");
                });

            //Milestones
            modelBuilder.Entity<DCREvent>()
                .HasMany(c => c.MilestoneReverseFrom)
                .WithMany(c => c.MilestoneReverseTo)
                .Map(m =>
                {
                    m.MapLeftKey("FromId");
                    m.MapRightKey("ToId");
                    m.ToTable("Milestones");
                });

            //Conditions
            modelBuilder.Entity<DCREvent>()
                .HasMany(c => c.ConditionReverseFrom)
                .WithMany(c => c.ConditionReverseTo)
                .Map(m =>
                {
                    m.MapLeftKey("FromId");
                    m.MapRightKey("ToId");
                    m.ToTable("Conditions");
                });
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
        //public virtual DbSet<Include> Includes { get; set; }




        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}