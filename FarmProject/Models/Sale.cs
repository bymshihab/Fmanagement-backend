namespace FarmProject.Models
{
    public class Sale
    {
        public int? SaleId { get; set; }
        public DateTime SaleDate { get; set; }= DateTime.Now;
        public string? SaleCode { get; set; }
        public int CustomerId { get; set; }
        public int EId { get; set; }
        public string? SaleDescription { get; set; }
        public decimal? DeliveryCharge { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public decimal? ExtraCost { get; set; }
        public decimal TotalSale { get; set; }
        public string? AddedBy { get; set; } = "appUser";
        public DateTime? AddedDate { get; set; } = DateTime.Now;
        public string? AddedPc { get; set; } = "Default App User";
        public string? UpdatedBy { get; set; } = "appUser";
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
        public string? UpDatedPc { get; set; } = "Default App User";
        public int? CompanyId { get; set; }
    }
}
