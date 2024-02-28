namespace FarmProject.Models
{
    public class SaleDetailsById
    {
        public int? SaleId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int UomId { get; set; }
        public string UomName { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Qty { get; set; }
        public decimal GrossTotal { get; set; }
        public decimal DiscountAmt { get; set; }
        public decimal DiscountPct { get; set; }
        public decimal VatAmt { get; set; }
        public decimal VatPct { get; set; }
        public decimal NetTotal { get; set; }
    }
}
