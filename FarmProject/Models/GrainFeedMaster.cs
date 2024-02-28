using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace FarmProject.Models
{
    public class GrainFeedMaster
    {
        public int? GrainMasterId { get; set; }
        public string? GrainCode { get; set; }
        public DateTime MakingDate { get; set; } = DateTime.Now;
        public decimal TotalQty { get; set; }
        public decimal TotalPrice { get; set; }
        public int ProductId { get; set; }
        public int? AnimalId { get; set; }
        public int? CompanyId { get;  set; }
        public string? AddedBy { get; set; } = "appUser";
        public DateTime? AddedDate { get; set; } = DateTime.Now;
        public string? AddedPc { get; set; } = "Default App User";
        public string? UpdatedBy { get; set; } = "appUser";
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
        public string? UpDatedPc { get; set; } = "Default App User";

    }
}
