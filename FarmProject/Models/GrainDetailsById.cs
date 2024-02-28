namespace FarmProject.Models
{
    public class GrainDetailsById
    {
        public int GrainMasterId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public decimal Qty { get; set; }
        public int UomId { get; set; }
        public string UomName { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
