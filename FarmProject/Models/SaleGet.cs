namespace FarmProject.Models
{
    public class SaleGet
    {
        public int? SaleId { get; set; }
        public DateTime SaleDate { get; set; } = DateTime.Now;
        public string? SaleCode { get; set; }
        public string? SaleDescription { get; set; }
        public string? CustomerName { get; set; }
        public string? EmployeeName { get; set; }
        public decimal TotalSale { get; set; }
        public int? CompanyId { get; set; }
    }
}
