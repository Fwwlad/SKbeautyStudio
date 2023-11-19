using Newtonsoft.Json;

namespace SKbeautyStudio.Db
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? Patonymic { get; set; }
        public DateTime Birthday { get; set; }
        public string Gender { get; set;}
        public string Phone { get; set; }
        public string? Notes { get; set; }
        public ICollection<Appointments> Appointments { get; set; }
        
    }
}
