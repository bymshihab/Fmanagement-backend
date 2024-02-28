namespace FarmProject.Models
{
    public class FeedingDetailsGetById
    {
        public int FeedId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int UomId { get; set; }
        public string UomName { get; set; }
        public decimal Price { get; set; }
        public decimal Qty { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
