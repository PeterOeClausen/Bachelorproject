namespace API.Migrations
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
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
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
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
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
                        Customer_Id1 = c.Int(),
                        DCRGraph_Id1 = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id1)
                .ForeignKey("dbo.DCRGraphs", t => t.DCRGraph_Id1)
                .Index(t => t.Customer_Id1)
                .Index(t => t.DCRGraph_Id1);
            
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
                        DCREvent_Id = c.Int(),
                        DCREvent_Id1 = c.Int(),
                        DCREvent_Id2 = c.Int(),
                        DCREvent_Id3 = c.Int(),
                        DCREvent_Id4 = c.Int(),
                        DCREvent_Id5 = c.Int(),
                        DCREvent_Id6 = c.Int(),
                        DCREvent_Id7 = c.Int(),
                        DCREvent_Id8 = c.Int(),
                        DCREvent_Id9 = c.Int(),
                        DCREvent_Id10 = c.Int(),
                        DCREvent_Id11 = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DCREvents", t => t.DCREvent_Id)
                .ForeignKey("dbo.DCREvents", t => t.DCREvent_Id1)
                .ForeignKey("dbo.DCREvents", t => t.DCREvent_Id2)
                .ForeignKey("dbo.DCREvents", t => t.DCREvent_Id3)
                .ForeignKey("dbo.DCREvents", t => t.DCREvent_Id4)
                .ForeignKey("dbo.DCREvents", t => t.DCREvent_Id5)
                .ForeignKey("dbo.DCREvents", t => t.DCREvent_Id6)
                .ForeignKey("dbo.DCREvents", t => t.DCREvent_Id7)
                .ForeignKey("dbo.DCREvents", t => t.DCREvent_Id8)
                .ForeignKey("dbo.DCREvents", t => t.DCREvent_Id9)
                .ForeignKey("dbo.DCREvents", t => t.DCREvent_Id10)
                .ForeignKey("dbo.DCREvents", t => t.DCREvent_Id11)
                .Index(t => t.DCREvent_Id)
                .Index(t => t.DCREvent_Id1)
                .Index(t => t.DCREvent_Id2)
                .Index(t => t.DCREvent_Id3)
                .Index(t => t.DCREvent_Id4)
                .Index(t => t.DCREvent_Id5)
                .Index(t => t.DCREvent_Id6)
                .Index(t => t.DCREvent_Id7)
                .Index(t => t.DCREvent_Id8)
                .Index(t => t.DCREvent_Id9)
                .Index(t => t.DCREvent_Id10)
                .Index(t => t.DCREvent_Id11);
            
            CreateTable(
                "dbo.EventUIElemements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IntegerSpecifyingUIElementId = c.Int(nullable: false),
                        DCREventId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DCREvents", t => t.DCREventId, cascadeDelete: true)
                .ForeignKey("dbo.IntegerSpecifyingUIElements", t => t.IntegerSpecifyingUIElementId, cascadeDelete: true)
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
                "dbo.DCREventDCRGraphs",
                c => new
                    {
                        DCREvent_Id = c.Int(nullable: false),
                        DCRGraph_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DCREvent_Id, t.DCRGraph_Id })
                .ForeignKey("dbo.DCREvents", t => t.DCREvent_Id, cascadeDelete: true)
                .ForeignKey("dbo.DCRGraphs", t => t.DCRGraph_Id, cascadeDelete: true)
                .Index(t => t.DCREvent_Id)
                .Index(t => t.DCRGraph_Id);
            
            CreateTable(
                "dbo.GroupDCREvents",
                c => new
                    {
                        Group_Id = c.Int(nullable: false),
                        DCREvent_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Group_Id, t.DCREvent_Id })
                .ForeignKey("dbo.Groups", t => t.Group_Id, cascadeDelete: true)
                .ForeignKey("dbo.DCREvents", t => t.DCREvent_Id, cascadeDelete: true)
                .Index(t => t.Group_Id)
                .Index(t => t.DCREvent_Id);
            
            CreateTable(
                "dbo.RoleDCREvents",
                c => new
                    {
                        Role_Id = c.Int(nullable: false),
                        DCREvent_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Role_Id, t.DCREvent_Id })
                .ForeignKey("dbo.Roles", t => t.Role_Id, cascadeDelete: true)
                .ForeignKey("dbo.DCREvents", t => t.DCREvent_Id, cascadeDelete: true)
                .Index(t => t.Role_Id)
                .Index(t => t.DCREvent_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderDetails", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "DCRGraph_Id1", "dbo.DCRGraphs");
            DropForeignKey("dbo.RoleDCREvents", "DCREvent_Id", "dbo.DCREvents");
            DropForeignKey("dbo.RoleDCREvents", "Role_Id", "dbo.Roles");
            DropForeignKey("dbo.GroupDCREvents", "DCREvent_Id", "dbo.DCREvents");
            DropForeignKey("dbo.GroupDCREvents", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.EventUIElemements", "IntegerSpecifyingUIElementId", "dbo.IntegerSpecifyingUIElements");
            DropForeignKey("dbo.EventUIElemements", "DCREventId", "dbo.DCREvents");
            DropForeignKey("dbo.DCREventDCRGraphs", "DCRGraph_Id", "dbo.DCRGraphs");
            DropForeignKey("dbo.DCREventDCRGraphs", "DCREvent_Id", "dbo.DCREvents");
            DropForeignKey("dbo.DCREvents", "DCREvent_Id11", "dbo.DCREvents");
            DropForeignKey("dbo.DCREvents", "DCREvent_Id10", "dbo.DCREvents");
            DropForeignKey("dbo.DCREvents", "DCREvent_Id9", "dbo.DCREvents");
            DropForeignKey("dbo.DCREvents", "DCREvent_Id8", "dbo.DCREvents");
            DropForeignKey("dbo.DCREvents", "DCREvent_Id7", "dbo.DCREvents");
            DropForeignKey("dbo.DCREvents", "DCREvent_Id6", "dbo.DCREvents");
            DropForeignKey("dbo.DCREvents", "DCREvent_Id5", "dbo.DCREvents");
            DropForeignKey("dbo.DCREvents", "DCREvent_Id4", "dbo.DCREvents");
            DropForeignKey("dbo.DCREvents", "DCREvent_Id3", "dbo.DCREvents");
            DropForeignKey("dbo.DCREvents", "DCREvent_Id2", "dbo.DCREvents");
            DropForeignKey("dbo.DCREvents", "DCREvent_Id1", "dbo.DCREvents");
            DropForeignKey("dbo.DCREvents", "DCREvent_Id", "dbo.DCREvents");
            DropForeignKey("dbo.Orders", "Customer_Id1", "dbo.Customers");
            DropForeignKey("dbo.OrderDetails", "ItemId", "dbo.Items");
            DropForeignKey("dbo.Items", "CategoryId", "dbo.Categories");
            DropIndex("dbo.RoleDCREvents", new[] { "DCREvent_Id" });
            DropIndex("dbo.RoleDCREvents", new[] { "Role_Id" });
            DropIndex("dbo.GroupDCREvents", new[] { "DCREvent_Id" });
            DropIndex("dbo.GroupDCREvents", new[] { "Group_Id" });
            DropIndex("dbo.DCREventDCRGraphs", new[] { "DCRGraph_Id" });
            DropIndex("dbo.DCREventDCRGraphs", new[] { "DCREvent_Id" });
            DropIndex("dbo.EventUIElemements", new[] { "DCREventId" });
            DropIndex("dbo.EventUIElemements", new[] { "IntegerSpecifyingUIElementId" });
            DropIndex("dbo.DCREvents", new[] { "DCREvent_Id11" });
            DropIndex("dbo.DCREvents", new[] { "DCREvent_Id10" });
            DropIndex("dbo.DCREvents", new[] { "DCREvent_Id9" });
            DropIndex("dbo.DCREvents", new[] { "DCREvent_Id8" });
            DropIndex("dbo.DCREvents", new[] { "DCREvent_Id7" });
            DropIndex("dbo.DCREvents", new[] { "DCREvent_Id6" });
            DropIndex("dbo.DCREvents", new[] { "DCREvent_Id5" });
            DropIndex("dbo.DCREvents", new[] { "DCREvent_Id4" });
            DropIndex("dbo.DCREvents", new[] { "DCREvent_Id3" });
            DropIndex("dbo.DCREvents", new[] { "DCREvent_Id2" });
            DropIndex("dbo.DCREvents", new[] { "DCREvent_Id1" });
            DropIndex("dbo.DCREvents", new[] { "DCREvent_Id" });
            DropIndex("dbo.Orders", new[] { "DCRGraph_Id1" });
            DropIndex("dbo.Orders", new[] { "Customer_Id1" });
            DropIndex("dbo.OrderDetails", new[] { "OrderId" });
            DropIndex("dbo.OrderDetails", new[] { "ItemId" });
            DropIndex("dbo.Items", new[] { "CategoryId" });
            DropTable("dbo.RoleDCREvents");
            DropTable("dbo.GroupDCREvents");
            DropTable("dbo.DCREventDCRGraphs");
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
