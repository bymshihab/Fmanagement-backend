namespace FarmProject.Models
{
    public class MilkCollectionById
    {
        public int? MilkCollectionId { get; set; }
        public string? MilkCollectionCode { get; set; }
        public DateTime MilkCollectionDate { get; set; } = DateTime.Now;
        public int EId { get; set; }
        public string EmployeeName { get; set; }
        public string? MilkCollectionDesc { get; set; }
        public List<MilkCollectionDetailById> MilkCollectionDetails { get; set; }

        public MilkCollectionById()

        {
            MilkCollectionDetails = new List<MilkCollectionDetailById>();
        }
    }
}
