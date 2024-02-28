using System.ComponentModel.DataAnnotations.Schema;

namespace FarmProject.Models
{
    public class Medicine
    {
        public int? MedicineId { get; set; }
        public int? MedicalId { get; set; }
        public int? ProductId { get; set; }
        public decimal day { get; set; }
        public string? time { get; set; }
        public decimal? Qty { get; set; }
        public int? UomId { get; set; }
        public string? AddedBy { get; set; } = "appUser";
        public DateTime? AddedDate { get; set; } = DateTime.Now;
        public string? AddedPc { get; set; } = "Default App User";
        public string? UpdatedBy { get; set; } = "appUser";
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
        public string? UpDatedPc { get; set; } = "Default App User";
    }
}
