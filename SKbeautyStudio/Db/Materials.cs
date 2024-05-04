using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SKbeautyStudio.Db
{
    public class Materials
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";
        public string Color { get; set; } = "";
        public int Number { get; set; } = -1;
        public ICollection<ExpirationDates>? ExpirationDates { get; set; }
    }
}
