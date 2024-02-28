namespace FarmProject.Models
{
    public class FeedingGet
    {
        public int FeedId { get; set; }
        public DateTime FeedIngDate { get; set; } = DateTime.Now;
        public string? FeedingCode { get; set; }
        public string? AnimalName { get; set; }
        public string? EmployeeName { get; set; }
        public decimal? TotalQTY { get; set; }
        public decimal? TotalCost { get; set;}
    }
}
