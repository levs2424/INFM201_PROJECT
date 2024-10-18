namespace INFM201.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TableReservationRelationship : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "TableID", c => c.Int(nullable: false));
            CreateIndex("dbo.Reservations", "TableID");
            AddForeignKey("dbo.Reservations", "TableID", "dbo.Tables", "TableID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reservations", "TableID", "dbo.Tables");
            DropIndex("dbo.Reservations", new[] { "TableID" });
            DropColumn("dbo.Reservations", "TableID");
        }
    }
}
