namespace FarmProject.Models
{
    public class GrainGet
    {
        public int? GrainMasterId { get; set; }
        public string? GrainCode { get; set; }
        public DateTime MakingDate { get; set; } = DateTime.Now;
        public string ProductName { get; set; }
        public string AnimalName { get; set; }
        public decimal TotalQty { get; set; }
        public decimal TotalPrice { get; set; }

    }
}
