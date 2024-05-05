using Microsoft.EntityFrameworkCore;

namespace SKbeautyStudio.Db
{
    [PrimaryKey(nameof(Login))]
    public class EmployeesPasswords
    {
        public string Login { get; set; } = "";
        public int EmployeeId { get; set; }
        public string Password { get; set; } = "";
        public Employees? Employee { get; set; }
    }
}
