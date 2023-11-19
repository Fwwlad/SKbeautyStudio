using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Text;

namespace SKbeautyStudio.Db
{
    public class PhotosOfEmployee
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Source { get; set; }
    }
}
