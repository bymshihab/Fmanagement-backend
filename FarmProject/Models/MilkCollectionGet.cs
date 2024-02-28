namespace FarmProject.Models
{
    public class MilkCollectionGet
    {
        public int? MilkCollectionId { get; set; }
        public string? MilkCollectionCode { get; set; }
        public DateTime MilkCollectionDate { get; set; } = DateTime.Now;
        public string EmployeeName { get; set; }
        public string? MilkCollectionDesc { get; set; }
    }
}
