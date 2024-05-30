namespace SKbeautyStudio.Db
{
    public class Employees
    {
        public int Id { get; set; }
        public string Surname { get; set; } = "";
        public string Name { get; set; } = "";
        public string? Patronymic { get; set; }
        public string? Phone { get; set; }
        public DateTime? Birthday { get; set; }
        public DateTime DateOfHire { get; set; }
        public string Gender { get; set; } = "";
        public string? Email { get; set; }
        public double SalaryPercent { get; set; }
        public ICollection<EmployeesMobileAppPages>? EmployeeMobileAppPages { get; set; }
        public ICollection<EmployeesJobTitles>? AvailableCategories { get; set; }
    }
}
