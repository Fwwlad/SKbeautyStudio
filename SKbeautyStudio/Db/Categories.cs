using Microsoft.Build.Framework;

namespace SKbeautyStudio.Db
{
    public class Categories
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        
        public string? UIColor { get; set; }

        public ICollection<Services>? Services { get; set; }
    }
}
