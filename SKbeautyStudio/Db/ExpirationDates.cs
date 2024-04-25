namespace SKbeautyStudio.Db
{
    public class ExpirationDates
    {
        public int Id { get; set; }
        public int MaterialId {get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? DisposalDate { get; set; }
        public Materials? Material { get; set; }    
    }
}
