using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace FarmProject.Models
{
    public class FeedingDetail
    {
        public int FeedingDetId { get; set; }
        public int FeedId { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public decimal Qty { get; set; }
        public decimal TotalPrice { get; set; }
        public int UomId { get; set; }
        public string? AddedBy { get; set; } = "appUser";
        public DateTime? AddedDate { get; set; } = DateTime.Now;
        public string? AddedPc { get; set; } = "Default AppUser";
        public string? UpdatedBy { get; set; } = "appUser";
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
        public string? UpDatedPc { get; set; } = "Default App User";
    }
}
