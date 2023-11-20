using Microsoft.EntityFrameworkCore;

namespace SKbeautyStudio.Db
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<PhotosOfEmployee> PhotosOfEmployees { get; set; }
        public DbSet<Appointments> Appointments { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Services> Services { get; set; }
        public DbSet<StatusesOfAppointments> StatusesOfAppointments { get; set; }
        public DbSet<Categories> Categories { get; set; }


    }
}
