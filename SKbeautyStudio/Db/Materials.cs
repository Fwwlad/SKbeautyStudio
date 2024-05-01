using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SKbeautyStudio.Db
{
    public class Materials
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";
        public string Color { get; set; } = "";
        public ICollection<ExpirationDates>? ExpirationDates { get; set; }
    }
}
