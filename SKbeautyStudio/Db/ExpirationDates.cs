using System.ComponentModel.DataAnnotations;

namespace SKbeautyStudio.Db
{
    public class ExpirationDates
    {
        public int Id { get; set; }
        public int MaterialId {get; set; }
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? PurchaseDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DisposalDate { get; set; }
        public Materials? Material { get; set; }
    }
}
