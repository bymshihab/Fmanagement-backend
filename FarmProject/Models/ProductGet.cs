namespace FarmProject.Models
{
    public class ProductGet
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public bool Status { get; set; } = true;
        public double? Price { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int UomId { get; set; }
        public string UomName { get; set; }
    }
}
