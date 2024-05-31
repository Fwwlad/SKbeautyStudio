
namespace SKbeautyStudio.Db
{
    public class Appointments
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int ServiceId { get; set; }
        public int EmployeeId { get; set; }
        private DateTime _startDateTime;
        public DateTime StartDateTime
        {
            get => _startDateTime;//.AddHours(3);
            set => _startDateTime = value;
        }
        private DateTime _endDateTime;
        public DateTime EndDateTime
        {
            get => _endDateTime;//.AddHours(3);
            set => _endDateTime = value;
        }
        public int StatusId { get; set; }
        public int Price { get; set; }
        public Clients? Client { get; set; }
        public Services? Service { get; set; }
        public StatusesOfAppointments? Status { get; set; }
        public Employees? Employee { get; set; }
    }
}
