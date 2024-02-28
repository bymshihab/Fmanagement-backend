using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace FarmProject.Models
{
    public class MilkCollection
    {
        public int? MilkCollectionId { get; set; }
        public string? MilkCollectionCode { get; set; }
        public DateTime MilkCollectionDate { get; set; } = DateTime.Now;
        public int EId { get; set; }
        public string? MilkCollectionDesc { get; set; }
        public int? CompanyId { get; set; }
        public string? AddedBy { get; set; } = "appUser";
        public DateTime? AddedDate { get; set; } = DateTime.Now;
        public string? AddedPc { get; set; } = "Default App User";
        public string? UpdatedBy { get; set; } = "appUser";
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
        public string? UpDatedPc { get; set; } = "Default App User";
    }
}
