namespace INFM201.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StaffAdminChnages : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "StaffId", c => c.Int());
            AddColumn("dbo.Staffs", "StaffEmail", c => c.String(nullable: false));
            AddColumn("dbo.Staffs", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Staffs", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Takeaways", "StaffId", c => c.Int());
            CreateIndex("dbo.Reservations", "StaffId");
            CreateIndex("dbo.Takeaways", "StaffId");
            AddForeignKey("dbo.Reservations", "StaffId", "dbo.Staffs", "StaffId", cascadeDelete: false);
            AddForeignKey("dbo.Takeaways", "StaffId", "dbo.Staffs", "StaffId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Takeaways", "StaffId", "dbo.Staffs");
            DropForeignKey("dbo.Reservations", "StaffId", "dbo.Staffs");
            DropIndex("dbo.Takeaways", new[] { "StaffId" });
            DropIndex("dbo.Reservations", new[] { "StaffId" });
            DropColumn("dbo.Takeaways", "StaffId");
            DropColumn("dbo.Staffs", "IsActive");
            DropColumn("dbo.Staffs", "DateCreated");
            DropColumn("dbo.Staffs", "StaffEmail");
            DropColumn("dbo.Reservations", "StaffId");
        }
    }
}
