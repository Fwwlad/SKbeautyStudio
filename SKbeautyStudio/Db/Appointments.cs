
namespace SKbeautyStudio.Db
{
    public class Appointments
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int EmployeeId { get; set; }
        public int ServiceId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int StatusId { get; set; }
        public int Price { get; set; }
        public Clients? Client { get; set; }
        public Services? Service { get; set; }
        public StatusesOfAppointments? Status { get; set; }

    }
}
