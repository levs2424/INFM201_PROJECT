namespace INFM201.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class News : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Staffs", "IsManager");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Staffs", "IsManager", c => c.Boolean(nullable: false));
        }
    }
}
