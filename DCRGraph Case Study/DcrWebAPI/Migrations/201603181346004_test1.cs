namespace DcrWebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Price = c.Double(nullable: false),
                        Description = c.String(nullable: false),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemId = c.Int(nullable: false),
                        OrderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId)
                .ForeignKey("dbo.Items", t => t.ItemId)
                .Index(t => t.ItemId)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderDate = c.DateTime(nullable: false),
                        Notes = c.String(nullable: false),
                        Table = c.String(nullable: false),
                        DCRGraph_Id = c.Int(nullable: false),
                        Customer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .ForeignKey("dbo.DCRGraphs", t => t.DCRGraph_Id)
                .Index(t => t.DCRGraph_Id)
                .Index(t => t.Customer_Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        Phone = c.String(),
                        StreetAndNumber = c.String(),
                        Zipcode = c.String(),
                        City = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DCRGraphs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DCREvents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EventId = c.String(nullable: false),
                        Label = c.String(nullable: false),
                        Description = c.String(),
                        StatusMessageAfterExecution = c.String(),
                        Included = c.Boolean(nullable: false),
                        Pending = c.Boolean(nullable: false),
                        Executed = c.Boolean(nullable: false),
                        Parent = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EventUIElemements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IntegerSpecifyingUIElementId = c.Int(nullable: false),
                        DCREventId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.IntegerSpecifyingUIElements", t => t.IntegerSpecifyingUIElementId)
                .ForeignKey("dbo.DCREvents", t => t.DCREventId)
                .Index(t => t.IntegerSpecifyingUIElementId)
                .Index(t => t.DCREventId);
            
            CreateTable(
                "dbo.IntegerSpecifyingUIElements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Integer = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Children",
                c => new
                    {
                        ToEventId = c.Int(nullable: false),
                        FromEventId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ToEventId, t.FromEventId })
                .ForeignKey("dbo.DCREvents", t => t.ToEventId)
                .ForeignKey("dbo.DCREvents", t => t.FromEventId)
                .Index(t => t.ToEventId)
                .Index(t => t.FromEventId);
            
            CreateTable(
                "dbo.Conditions",
                c => new
                    {
                        ToEventId = c.Int(nullable: false),
                        FromEventId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ToEventId, t.FromEventId })
                .ForeignKey("dbo.DCREvents", t => t.ToEventId)
                .ForeignKey("dbo.DCREvents", t => t.FromEventId)
                .Index(t => t.ToEventId)
                .Index(t => t.FromEventId);
            
            CreateTable(
                "dbo.Excludes",
                c => new
                    {
                        FromEventId = c.Int(nullable: false),
                        ToDCREventId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.FromEventId, t.ToDCREventId })
                .ForeignKey("dbo.DCREvents", t => t.FromEventId)
                .ForeignKey("dbo.DCREvents", t => t.ToDCREventId)
                .Index(t => t.FromEventId)
                .Index(t => t.ToDCREventId);
            
            CreateTable(
                "dbo.Includes",
                c => new
                    {
                        FromEventId = c.Int(nullable: false),
                        ToEventId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.FromEventId, t.ToEventId })
                .ForeignKey("dbo.DCREvents", t => t.FromEventId)
                .ForeignKey("dbo.DCREvents", t => t.ToEventId)
                .Index(t => t.FromEventId)
                .Index(t => t.ToEventId);
            
            CreateTable(
                "dbo.Milestones",
                c => new
                    {
                        ToEventId = c.Int(nullable: false),
                        FromEventId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ToEventId, t.FromEventId })
                .ForeignKey("dbo.DCREvents", t => t.ToEventId)
                .ForeignKey("dbo.DCREvents", t => t.FromEventId)
                .Index(t => t.ToEventId)
                .Index(t => t.FromEventId);
            
            CreateTable(
                "dbo.Responses",
                c => new
                    {
                        ToEventId = c.Int(nullable: false),
                        FromEventId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ToEventId, t.FromEventId })
                .ForeignKey("dbo.DCREvents", t => t.ToEventId)
                .ForeignKey("dbo.DCREvents", t => t.FromEventId)
                .Index(t => t.ToEventId)
                .Index(t => t.FromEventId);
            
            CreateTable(
                "dbo.GraphEvents",
                c => new
                    {
                        DCREventId = c.Int(nullable: false),
                        DCRGraphId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DCREventId, t.DCRGraphId })
                .ForeignKey("dbo.DCREvents", t => t.DCREventId, cascadeDelete: true)
                .ForeignKey("dbo.DCRGraphs", t => t.DCRGraphId, cascadeDelete: true)
                .Index(t => t.DCREventId)
                .Index(t => t.DCRGraphId);
            
            CreateTable(
                "dbo.EventGroups",
                c => new
                    {
                        DCREventId = c.Int(nullable: false),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DCREventId, t.GroupId })
                .ForeignKey("dbo.DCREvents", t => t.DCREventId, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.DCREventId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.EventRoles",
                c => new
                    {
                        DCREventId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DCREventId, t.RoleId })
                .ForeignKey("dbo.DCREvents", t => t.DCREventId, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.DCREventId)
                .Index(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.OrderDetails", "ItemId", "dbo.Items");
            DropForeignKey("dbo.OrderDetails", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "DCRGraph_Id", "dbo.DCRGraphs");
            DropForeignKey("dbo.EventRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.EventRoles", "DCREventId", "dbo.DCREvents");
            DropForeignKey("dbo.EventGroups", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.EventGroups", "DCREventId", "dbo.DCREvents");
            DropForeignKey("dbo.EventUIElemements", "DCREventId", "dbo.DCREvents");
            DropForeignKey("dbo.EventUIElemements", "IntegerSpecifyingUIElementId", "dbo.IntegerSpecifyingUIElements");
            DropForeignKey("dbo.GraphEvents", "DCRGraphId", "dbo.DCRGraphs");
            DropForeignKey("dbo.GraphEvents", "DCREventId", "dbo.DCREvents");
            DropForeignKey("dbo.Responses", "FromEventId", "dbo.DCREvents");
            DropForeignKey("dbo.Responses", "ToEventId", "dbo.DCREvents");
            DropForeignKey("dbo.Milestones", "FromEventId", "dbo.DCREvents");
            DropForeignKey("dbo.Milestones", "ToEventId", "dbo.DCREvents");
            DropForeignKey("dbo.Includes", "ToEventId", "dbo.DCREvents");
            DropForeignKey("dbo.Includes", "FromEventId", "dbo.DCREvents");
            DropForeignKey("dbo.Excludes", "ToDCREventId", "dbo.DCREvents");
            DropForeignKey("dbo.Excludes", "FromEventId", "dbo.DCREvents");
            DropForeignKey("dbo.Conditions", "FromEventId", "dbo.DCREvents");
            DropForeignKey("dbo.Conditions", "ToEventId", "dbo.DCREvents");
            DropForeignKey("dbo.Children", "FromEventId", "dbo.DCREvents");
            DropForeignKey("dbo.Children", "ToEventId", "dbo.DCREvents");
            DropForeignKey("dbo.Orders", "Customer_Id", "dbo.Customers");
            DropIndex("dbo.EventRoles", new[] { "RoleId" });
            DropIndex("dbo.EventRoles", new[] { "DCREventId" });
            DropIndex("dbo.EventGroups", new[] { "GroupId" });
            DropIndex("dbo.EventGroups", new[] { "DCREventId" });
            DropIndex("dbo.GraphEvents", new[] { "DCRGraphId" });
            DropIndex("dbo.GraphEvents", new[] { "DCREventId" });
            DropIndex("dbo.Responses", new[] { "FromEventId" });
            DropIndex("dbo.Responses", new[] { "ToEventId" });
            DropIndex("dbo.Milestones", new[] { "FromEventId" });
            DropIndex("dbo.Milestones", new[] { "ToEventId" });
            DropIndex("dbo.Includes", new[] { "ToEventId" });
            DropIndex("dbo.Includes", new[] { "FromEventId" });
            DropIndex("dbo.Excludes", new[] { "ToDCREventId" });
            DropIndex("dbo.Excludes", new[] { "FromEventId" });
            DropIndex("dbo.Conditions", new[] { "FromEventId" });
            DropIndex("dbo.Conditions", new[] { "ToEventId" });
            DropIndex("dbo.Children", new[] { "FromEventId" });
            DropIndex("dbo.Children", new[] { "ToEventId" });
            DropIndex("dbo.EventUIElemements", new[] { "DCREventId" });
            DropIndex("dbo.EventUIElemements", new[] { "IntegerSpecifyingUIElementId" });
            DropIndex("dbo.Orders", new[] { "Customer_Id" });
            DropIndex("dbo.Orders", new[] { "DCRGraph_Id" });
            DropIndex("dbo.OrderDetails", new[] { "OrderId" });
            DropIndex("dbo.OrderDetails", new[] { "ItemId" });
            DropIndex("dbo.Items", new[] { "CategoryId" });
            DropTable("dbo.EventRoles");
            DropTable("dbo.EventGroups");
            DropTable("dbo.GraphEvents");
            DropTable("dbo.Responses");
            DropTable("dbo.Milestones");
            DropTable("dbo.Includes");
            DropTable("dbo.Excludes");
            DropTable("dbo.Conditions");
            DropTable("dbo.Children");
            DropTable("dbo.Roles");
            DropTable("dbo.Groups");
            DropTable("dbo.IntegerSpecifyingUIElements");
            DropTable("dbo.EventUIElemements");
            DropTable("dbo.DCREvents");
            DropTable("dbo.DCRGraphs");
            DropTable("dbo.Customers");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderDetails");
            DropTable("dbo.Items");
            DropTable("dbo.Categories");
        }
    }
}
