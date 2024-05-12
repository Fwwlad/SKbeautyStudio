using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SKbeautyStudio.Db
{
    [PrimaryKey(nameof(EmployeesId), nameof(CategoriesId))]
    public class EmployeesJobTitles
    {
        public int EmployeesId { get; set; }
        public int CategoriesId { get; set; }
        public Employees? Employees { get; set; }
        public Categories? Categories { get; set; }
    }
}
