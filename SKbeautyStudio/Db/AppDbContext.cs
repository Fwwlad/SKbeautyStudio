using Microsoft.EntityFrameworkCore;
using SKbeautyStudio.Db;

namespace SKbeautyStudio.Db
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Appointments> Appointments { get; set; }
        public DbSet<Clients> Clients { get; set; }
        public DbSet<Services> Services { get; set; }
        public DbSet<StatusesOfAppointments> StatusesOfAppointments { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<SKbeautyStudio.Db.Employees> Employees { get; set; }
        public DbSet<SKbeautyStudio.Db.PhotosOfEmployees> photosOfEmployees { get; set; }

    }
}
