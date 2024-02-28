namespace FarmProject.Models
{
    public class SaleGetById
    {
        public int? SaleId { get; set; }
        public DateTime SaleDate { get; set; } = DateTime.Now;
        public string? SaleCode { get; set; }
        public string? SaleDescription { get; set; }
        public string? CustomerName { get; set; }
        public int? CustomerId { get; set; }
        public int? EId { get; set; }
        public string? EmployeeName { get; set; }
        public decimal TotalSale { get; set; }
        public decimal DeliveryCharge { get; set; }
        public decimal ExtraCost { get; set; }  
        public DateTime? DeliveryDate { get; set; }
        public List<SaleDetailsById> SaleDetails { get; set; }
        public SaleGetById()
        {
            SaleDetails = new List<SaleDetailsById>();
        }
    }
}
