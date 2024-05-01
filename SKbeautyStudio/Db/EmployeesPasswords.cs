namespace SKbeautyStudio.Db
{
    public class EmployeesPasswords
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Password { get; set; }
        public Employees Employee { get; set; }
    }
}
