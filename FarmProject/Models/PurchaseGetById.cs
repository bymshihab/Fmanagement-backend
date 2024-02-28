namespace FarmProject.Models
{
    public class PurchaseGetById
    {

            public int? PurchaseId { get; set; }
            public DateTime PurchaseDate { get; set; } = DateTime.Now;
            public string? PurchaseCode { get; set; }
            public string? PurchaseDescription { get; set; }
            public string? SupplierName { get; set; }
            public int? SupplierId { get; set; }
            public int? EId { get; set; }
            public string? EmployeeName { get; set; }
            public decimal TotalPurchase { get; set; }
            public decimal? DeliveryCharge { get; set; } = 0;
            public decimal? ExtraCost { get; set; } = 0;
            public List<GetPurchaseDetailsById> PurchaseDetails { get; set; }

            public PurchaseGetById()

            {
            PurchaseDetails = new List<GetPurchaseDetailsById>();
            }
    }
}
