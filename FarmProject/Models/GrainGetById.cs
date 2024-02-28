namespace FarmProject.Models
{
    public class GrainGetById
    {
        public int? GrainMasterId { get; set; }
        public string? GrainCode { get; set; }
        public DateTime MakingDate { get; set; } = DateTime.Now;
        public decimal TotalQty { get; set; }
        public decimal TotalPrice { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int? AnimalId { get; set; }
        public string AnimalName { get; set; }
        public List<GrainDetailsById> GrainDetails { get; set; }

        public GrainGetById()

        {
            GrainDetails = new List<GrainDetailsById>();
        }
    }
}
