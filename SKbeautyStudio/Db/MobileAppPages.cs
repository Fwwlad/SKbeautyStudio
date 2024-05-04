using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SKbeautyStudio.Db
{
    [PrimaryKey(nameof(Id))]
    public class MobileAppPages
    {
        public int Id { get; set; }
        [Required]
        public string NameEN { get; set; } = "";
        [Required]
        public string NameRU { get; set; } = "";

    }
}
