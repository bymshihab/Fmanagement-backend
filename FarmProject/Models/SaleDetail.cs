namespace FarmProject.Models
{
    public class SaleDetail
    {
        public int SaleDetId { get; set; }
        public int? SaleId { get; set; }
        //public string? BatchCode { get; set; }
        public int CategoryId { get; set; }
        public int ProductId { get; set; }
        public int UomId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Qty { get; set; }
        public decimal? GrossTotal { get; set; }
        public decimal? DiscountAmt { get; set; }
        public decimal? DiscountPct { get; set; }
        public decimal? VatAmt { get; set; }
        public decimal? VatPct { get; set; }
        public decimal NetTotal { get; set; }
        public string? AddedBy { get; set; } = "appUser";
        public DateTime? AddedDate { get; set; } = DateTime.Now;
        public string? AddedPc { get; set; } = "Default App User";
        public string? UpdatedBy { get; set; } = "appUser";
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
        public string? UpDatedPc { get; set; } = "Default App User";
    }
}
