namespace INFM201.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedStaff : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reservations", "StaffId", "dbo.Staffs");
            DropForeignKey("dbo.Takeaways", "StaffId", "dbo.Staffs");
            DropIndex("dbo.Reservations", new[] { "StaffId" });
            DropIndex("dbo.Takeaways", new[] { "StaffId" });
            DropColumn("dbo.Reservations", "StaffId");
            DropColumn("dbo.Takeaways", "StaffId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Takeaways", "StaffId", c => c.Int());
            AddColumn("dbo.Reservations", "StaffId", c => c.Int());
            CreateIndex("dbo.Takeaways", "StaffId");
            CreateIndex("dbo.Reservations", "StaffId");
            AddForeignKey("dbo.Takeaways", "StaffId", "dbo.Staffs", "StaffId");
            AddForeignKey("dbo.Reservations", "StaffId", "dbo.Staffs", "StaffId");
        }
    }
}
