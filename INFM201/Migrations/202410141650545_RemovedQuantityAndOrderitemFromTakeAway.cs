namespace INFM201.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedQuantityAndOrderitemFromTakeAway : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Takeaways", "Quantity");
            DropColumn("dbo.Takeaways", "OrderItem");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Takeaways", "OrderItem", c => c.String());
            AddColumn("dbo.Takeaways", "Quantity", c => c.Int(nullable: false));
        }
    }
}
