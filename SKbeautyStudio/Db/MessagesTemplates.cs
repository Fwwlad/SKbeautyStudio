using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace SKbeautyStudio.Db
{
    public class MessagesTemplates
    {
        public int Id { get; set; }
        public int CategoriesId { get; set; }
        public string Text { get; set; } = "";
        public bool Before { get; set; }
        public int? HoursCount { get; set; }
        public TimeSpan? TimeStamp { get; set; }
        public Categories? Category { get; set; }
    }
}
