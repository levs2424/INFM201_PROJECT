namespace INFM201.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedOrderItemModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderItems",
                c => new
                    {
                        OrderItemID = c.Int(nullable: false, identity: true),
                        ItemName = c.String(),
                        Quantity = c.Int(nullable: false),
                        ItemPrice = c.Double(nullable: false),
                        Takeaway_TakeawayID = c.Int(),
                    })
                .PrimaryKey(t => t.OrderItemID)
                .ForeignKey("dbo.Takeaways", t => t.Takeaway_TakeawayID)
                .Index(t => t.Takeaway_TakeawayID);
            
            AlterColumn("dbo.Takeaways", "OrderItem", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderItems", "Takeaway_TakeawayID", "dbo.Takeaways");
            DropIndex("dbo.OrderItems", new[] { "Takeaway_TakeawayID" });
            AlterColumn("dbo.Takeaways", "OrderItem", c => c.String(nullable: false));
            DropTable("dbo.OrderItems");
        }
    }
}
