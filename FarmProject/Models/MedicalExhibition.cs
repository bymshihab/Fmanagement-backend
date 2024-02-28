using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace FarmProject.Models
{
    public class MedicalExhibition
    {
        public int? MedicalId { get; set; }
        public string? MedicalCode { get; set; }
        public DateTime? ExhibitionDate { get; set; }
        public int? AnimalId { get; set; }
        public int? CompanyId { get; set; }
        public int? OutsiderId { get; set; }
        public int? EId { get; set; }
        public Boolean? IsNew { get; set; } = true;
        public Boolean? IsSick { get; set; } = false;
        public string? AddedBy { get; set; } = "appUser";
        public DateTime? AddedDate { get; set; } = DateTime.Now;
        public string? AddedPc { get; set; } = "Default App User";
        public string? UpdatedBy { get; set; } = "appUser";
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
        public string? UpDatedPc { get; set; } = "Default App User";
    }
}
