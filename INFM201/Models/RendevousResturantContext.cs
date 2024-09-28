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


    }
}