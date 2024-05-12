using Microsoft.Build.Framework;

namespace SKbeautyStudio.Db
{
    public class Categories
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = "";        
        public string? UIColor { get; set; }
        public string? JobName { get; set; } = "";
        public ICollection<Services>? Services { get; set; }
        public ICollection<MessagesTemplates>? MessagesTemplates { get; set; }
    }
}
