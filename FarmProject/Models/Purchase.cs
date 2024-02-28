using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace FarmProject.Models
{
    public class Purchase
    {
        public int? PurchaseId { get; set; }
        public DateTime PurchaseDate { get; set; } = DateTime.Now;
        public string? PurchaseCode { get; set; }
        public int SupplierId { get; set; }
        public int EId { get; set; }
        public int? CompanyId { get; set; }
        public decimal TotalPurchase { get; set; }
        public decimal? DeliveryCharge { get; set; } = 0;
        public decimal? ExtraCost { get; set; } = 0;
        public string? PurchaseDescription { get; set; }
        public string? AddedBy { get; set; } = "appUser";
        public DateTime? AddedDate { get; set; } = DateTime.Now;
        public string? AddedPc { get; set; } = "Default App User";
        public string? UpdatedBy { get; set; } = "appUser";
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
        public string? UpDatedPc { get; set; } = "Default App User";
    }
}
