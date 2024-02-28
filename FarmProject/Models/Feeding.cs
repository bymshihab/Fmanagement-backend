using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace FarmProject.Models
{
    public class Feeding
    {
        public int? FeedId { get; set; }
        public DateTime FeedIngDate { get; set; } = DateTime.Now;
        public string? FeedingCode { get; set; }
        public int? EId { get; set; }
        public int AnimalId { get; set; }
        public decimal? TotalQTY { get; set; }
        public decimal TotalCost { get; set; }
        public int? CompanyId { get; set; }
        public string? AddedBy { get; set; } = "appUser";
        public DateTime? AddedDate { get; set; } = DateTime.Now;
        public string? AddedPc { get; set; } = "Default AppUser";
        public string? UpdatedBy { get; set; } = "appUser";
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
        public string? UpDatedPc { get; set; } = "Default App User";
    }
}
