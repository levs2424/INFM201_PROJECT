namespace INFM201.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Roleidentity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Staffs", "IsManager", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Staffs", "IsManager");
        }
    }
}
