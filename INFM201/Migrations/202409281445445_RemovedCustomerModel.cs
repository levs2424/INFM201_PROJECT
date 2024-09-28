namespace INFM201.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedCustomerModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderItems", "Takeaway_TakeawayID", "dbo.Takeaways");
            DropIndex("dbo.OrderItems", new[] { "Takeaway_TakeawayID" });
            AddColumn("dbo.Takeaways", "Fullnames", c => c.String(nullable: false));
            AddColumn("dbo.Takeaways", "Email", c => c.String(nullable: false));
            AddColumn("dbo.Takeaways", "Quantity", c => c.Int(nullable: false));
            AddColumn("dbo.Takeaways", "ItemPrice", c => c.Double(nullable: false));
            AddColumn("dbo.Reservations", "Fullnames", c => c.String(nullable: false));
            AddColumn("dbo.Reservations", "Email", c => c.String(nullable: false));
            DropColumn("dbo.Takeaways", "CustomerID");
            DropColumn("dbo.Reservations", "CustomerID");
            DropColumn("dbo.Reservations", "StaffID");
            DropTable("dbo.Customers");
            DropTable("dbo.OrderItems");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.OrderItems",
                c => new
                    {
                        OrderItemID = c.Int(nullable: false, identity: true),
                        Item_name = c.String(),
                        Quantity = c.Int(nullable: false),
                        ItemPrice = c.Double(nullable: false),
                        Takeaway_TakeawayID = c.Int(),
                    })
                .PrimaryKey(t => t.OrderItemID);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerID = c.Int(nullable: false, identity: true),
                        Fullnames = c.String(nullable: false),
                        Email = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CustomerID);
            
            AddColumn("dbo.Reservations", "StaffID", c => c.Int(nullable: false));
            AddColumn("dbo.Reservations", "CustomerID", c => c.Int(nullable: false));
            AddColumn("dbo.Takeaways", "CustomerID", c => c.Int(nullable: false));
            DropColumn("dbo.Reservations", "Email");
            DropColumn("dbo.Reservations", "Fullnames");
            DropColumn("dbo.Takeaways", "ItemPrice");
            DropColumn("dbo.Takeaways", "Quantity");
            DropColumn("dbo.Takeaways", "Email");
            DropColumn("dbo.Takeaways", "Fullnames");
            CreateIndex("dbo.OrderItems", "Takeaway_TakeawayID");
            AddForeignKey("dbo.OrderItems", "Takeaway_TakeawayID", "dbo.Takeaways", "TakeawayID");
        }
    }
}
