using System.Data.Entity;

namespace INFM201.Models
{
    public class RendevousResturantContext : DbContext
    {
        public RendevousResturantContext() : base("RendevousResturantContext")
        {
        }

        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Staff> Staff { get; set; }

        public DbSet<Confirmation> Confirmation { get; set; }
        public DbSet<Takeaway> Takeaway { get; set; }

        public DbSet<Table> Tables { get; set; } // Add the DbSet for Table

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Reservation>()
                .HasOptional(r => r.Confirmation)
                .WithRequired(c => c.Reservation)
                .WillCascadeOnDelete(false);  // Prevent cascading delete of Confirmation when Reservation is deleted

            base.OnModelCreating(modelBuilder);
        }


    }
}