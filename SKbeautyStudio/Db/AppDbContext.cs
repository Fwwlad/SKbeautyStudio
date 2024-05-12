using Microsoft.EntityFrameworkCore;
using SKbeautyStudio.Db;
using System.Reflection.Metadata;

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
        public DbSet<Employees> Employees { get; set; }
        public DbSet<EmployeesPasswords> EmployeesPasswords { get; set; }
        public DbSet<PhotosOfEmployees> photosOfEmployees { get; set; }
        public DbSet<EmployeesMobileAppPages> EmployeesMobileAppPages { get; set; }
        public DbSet<EmployeesJobTitles> EmployeesJobTitles { get; set; }
        public DbSet<MobileAppPages> MobileAppPages { get; set; }
        public DbSet<Materials> Materials { get; set; }
        public DbSet<ExpirationDates> ExpirationDates { get; set; }
        public DbSet<MessagesTemplates> MessagesTemplates { get; set; }
        
    }
}
