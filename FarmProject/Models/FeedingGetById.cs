namespace FarmProject.Models
{
    public class FeedingGetById
    {
        public int? FeedId { get; set; }
        public DateTime FeedIngDate { get; set; } = DateTime.Now;
        public string? FeedingCode { get; set; }
        public int? EId { get; set; }
        public string? EmployeeName { get; set; }
        public int AnimalId { get; set; }
        public string? AnimalName { get; set; }
        public decimal? TotalQTY { get; set; }
        public decimal? TotalCost { get; set; }
        public List<FeedingDetailsGetById> FeedingDetails { get; set; }
        public FeedingGetById()
        {
            FeedingDetails = new List<FeedingDetailsGetById>();
        }
    }
}
