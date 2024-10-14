namespace INFM201.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SoftDeleteTakeAway : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Takeaways", "IsDelete", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Takeaways", "IsDelete");
        }
    }
}
