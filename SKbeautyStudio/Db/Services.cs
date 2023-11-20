using Microsoft.Build.Framework;

namespace SKbeautyStudio.Db
{
    public class Services
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BaseTimeMinutes { get; set; }
        public int BaseCost { get; set; }
        public int CategoryId { get; set; }
        public Categories Category { get; set; }
        
    }
}
