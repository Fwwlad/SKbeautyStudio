namespace SKbeautyStudio.Db
{
    public class Categories
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }

        public ICollection<Services> Services { get; set; }
    }
}
