using Microsoft.EntityFrameworkCore;

namespace SKbeautyStudio.Db
{
    [PrimaryKey(nameof(EmployeeId), nameof(MobileAppPageId))]
    public class EmployeesMobileAppPages
    {
        public int EmployeeId { get; set; }
        public int MobileAppPageId { get; set; }
        public bool CanView { get; set; } = false;
        public bool CanAdd { get; set; } = false;
        public bool CanEdit { get; set; } = false; 
        public bool CanDelete { get; set; } = false;
        public Employees? Employees { get; set; }
        public MobileAppPages? MobileAppPage { get; set; }
    }
}
