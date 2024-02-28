namespace FarmProject.Models
{
    public class PurchaseGet
    {
        public int? PurchaseId { get; set; }
        public DateTime PurchaseDate { get; set; } = DateTime.Now;
        public string? PurchaseCode { get; set; }
        public string? PurchaseDescription { get; set; }
        public string? SupplierName { get; set; }
        public string? EmployeeName { get; set; }
        public decimal TotalPurchase { get; set; }
        public int? CompanyId { get; set; }
    }
}
