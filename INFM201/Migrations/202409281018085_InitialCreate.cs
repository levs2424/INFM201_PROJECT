namespace INFM201.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Confirmations",
                c => new
                    {
                        ConfirmationID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Surname = c.String(),
                        EmailAddress = c.String(),
                        ConfirmEmailAddress = c.String(),
                        SpecialRequests = c.String(),
                        ReservationId = c.Int(),
                        TakeawayId = c.Int(),
                    })
                .PrimaryKey(t => t.ConfirmationID);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerID = c.Int(nullable: false, identity: true),
                        Fullnames = c.String(nullable: false),
                        Email = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CustomerID);
            
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
                .PrimaryKey(t => t.OrderItemID)
                .ForeignKey("dbo.Takeaways", t => t.Takeaway_TakeawayID)
                .Index(t => t.Takeaway_TakeawayID);
            
            CreateTable(
                "dbo.Takeaways",
                c => new
                    {
                        TakeawayID = c.Int(nullable: false, identity: true),
                        CustomerID = c.Int(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                        OrderStatus = c.Int(nullable: false),
                        TotalAmount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.TakeawayID);
            
            CreateTable(
                "dbo.Reservations",
                c => new
                    {
                        ReservationID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Time = c.Time(nullable: false, precision: 7),
                        NumberOfGuests = c.Int(nullable: false),
                        SeatingPreference = c.String(nullable: false),
                        SpecialRequests = c.String(),
                        IsConfirmed = c.Boolean(nullable: false),
                        NumberOfBookingsInside = c.Int(nullable: false),
                        NumberOfBookingsOutside = c.Int(nullable: false),
                        CustomerID = c.Int(nullable: false),
                        StaffID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ReservationID);
            
            CreateTable(
                "dbo.Staffs",
                c => new
                    {
                        StaffId = c.Int(nullable: false, identity: true),
                        EmployeeID = c.Int(nullable: false),
                        Password = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.StaffId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderItems", "Takeaway_TakeawayID", "dbo.Takeaways");
            DropIndex("dbo.OrderItems", new[] { "Takeaway_TakeawayID" });
            DropTable("dbo.Staffs");
            DropTable("dbo.Reservations");
            DropTable("dbo.Takeaways");
            DropTable("dbo.OrderItems");
            DropTable("dbo.Customers");
            DropTable("dbo.Confirmations");
        }
    }
}
