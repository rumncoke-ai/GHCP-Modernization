using System.Data.Entity.Migrations;

namespace LegacyShop.Data.Migrations
{
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 500)
                    })
                .PrimaryKey(t => t.CategoryId);

            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        Description = c.String(maxLength: 1000),
                        Price = c.Decimal(nullable: false, storeType: "money"),
                        StockQuantity = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CategoryId = c.Int(nullable: false)
                    })
                .PrimaryKey(t => t.ProductId)
                .ForeignKey("dbo.Categories", t => t.CategoryId)
                .Index(t => t.CategoryId);

            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 75),
                        LastName = c.String(nullable: false, maxLength: 75),
                        Email = c.String(nullable: false, maxLength: 200),
                        Phone = c.String(maxLength: 30),
                        Address = c.String(maxLength: 250),
                        City = c.String(maxLength: 100),
                        PostalCode = c.String(maxLength: 20)
                    })
                .PrimaryKey(t => t.CustomerId);

            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderId = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                        Status = c.String(nullable: false, maxLength: 30),
                        TotalAmount = c.Decimal(nullable: false, storeType: "money")
                    })
                .PrimaryKey(t => t.OrderId)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .Index(t => t.CustomerId);

            CreateTable(
                "dbo.OrderItems",
                c => new
                    {
                        OrderItemId = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, storeType: "money")
                    })
                .PrimaryKey(t => t.OrderItemId)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .Index(t => t.OrderId)
                .Index(t => t.ProductId);
        }

        public override void Down()
        {
            DropForeignKey("dbo.OrderItems", "ProductId", "dbo.Products");
            DropForeignKey("dbo.OrderItems", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Products", "CategoryId", "dbo.Categories");
            DropIndex("dbo.OrderItems", new[] { "ProductId" });
            DropIndex("dbo.OrderItems", new[] { "OrderId" });
            DropIndex("dbo.Orders", new[] { "CustomerId" });
            DropIndex("dbo.Products", new[] { "CategoryId" });
            DropTable("dbo.OrderItems");
            DropTable("dbo.Orders");
            DropTable("dbo.Customers");
            DropTable("dbo.Products");
            DropTable("dbo.Categories");
        }
    }
}
