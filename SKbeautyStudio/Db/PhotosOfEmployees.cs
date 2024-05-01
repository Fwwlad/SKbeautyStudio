using Newtonsoft.Json.Linq;
using System.Text;

namespace SKbeautyStudio.Db
{
    public class PhotosOfEmployees
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Source { get; set; } = "";
    }
}
