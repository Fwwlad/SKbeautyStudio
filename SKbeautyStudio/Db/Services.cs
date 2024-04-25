using Microsoft.Build.Framework;

namespace SKbeautyStudio.Db
{
    public class Services
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int BaseTimeMinutes { get; set; }
        [Required]
        public int BaseCost { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public Categories? Category { get; set; }
    }
}
