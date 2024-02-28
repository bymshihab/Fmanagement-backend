using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace FarmProject.Models
{
    public class GrainFeedChart
    {
        public int FeedChartId { get; set; }
        public int GrainMasterId { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public decimal Qty { get; set; }
        public int UomId { get; set; }
        public decimal TotalPrice { get; set; }
        public string? AddedBy { get; set; } = "appUser";
        public DateTime? AddedDate { get; set; } = DateTime.Now;
        public string? AddedPc { get; set; } = "Default App User";
        public string? UpdatedBy { get; set; } = "appUser";
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
        public string? UpDatedPc { get; set; } = "Default App User";
    }
}
