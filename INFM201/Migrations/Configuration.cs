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
            //  This method will be called after migrating to the latest version.
            var staff1 = new Staff();
            staff1.EmployeeID = 1;
            staff1.Password = "1234";

            context.Staff.Add(staff1);
            context.SaveChanges();

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
