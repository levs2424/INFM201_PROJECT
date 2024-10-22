namespace INFM201.Migrations
{
    using INFM201.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<INFM201.Models.RendevousResturantContext>
    {
        public Configuration()
        {
            AutomaticMigrationDataLossAllowed = true;
            AutomaticMigrationsEnabled = true;
   
           ContextKey = "INFM201.Models.RendevousResturantContext";

        }

        protected override void Seed(INFM201.Models.RendevousResturantContext context)
        {
            // Seeding Tables
            context.Tables.AddOrUpdate(
                t => t.TableID, // Use TableID as the identifier
                new Table { TableID = 1, SeatingType = "Inside - Table", MaxGuests = 4, IsAvailable = true, TableNumber = "Table 1" },
                new Table { TableID = 2, SeatingType = "Inside - Table", MaxGuests = 4, IsAvailable = true, TableNumber = "Table 2" },
                new Table { TableID = 3, SeatingType = "Inside - Table", MaxGuests = 6, IsAvailable = true, TableNumber = "Table 3" },
                new Table { TableID = 4, SeatingType = "Inside - Couch/Lounge", MaxGuests = 6, IsAvailable = true, TableNumber = "Couch 1" },
                new Table { TableID = 5, SeatingType = "Outside", MaxGuests = 2, IsAvailable = true, TableNumber = "Table 5" },
                new Table { TableID = 6, SeatingType = "Outside", MaxGuests = 4, IsAvailable = true, TableNumber = "Table 6" }
            );

            context.SaveChanges(); // Save changes to the databaseavoid creating duplicate seed data.

            context.Staff.AddOrUpdate(
                s => s.EmployeeID, // Check by EmployeeID to avoid duplicates
                new Staff
                {
                  EmployeeID = 1001,
                  StaffEmail = "elishaechetty@gmail.com",
                  Password = "InitialPassword123", // You may want to hash this later
                  IsManager = true,
                  IsActive = true,
                  DateCreated = DateTime.Now
                },
                new Staff
                {
                  EmployeeID = 1002,
                  StaffEmail = "chettyelsie9@gmail.com",
                  Password = "InitialPassword123", // You may want to hash this later
                  IsManager = false,
                  IsActive = true,
                  DateCreated = DateTime.Now
                }
            );

            context.SaveChanges(); // Save changes to the database

        }
    }
}
