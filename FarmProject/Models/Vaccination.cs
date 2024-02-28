using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace FarmProject.Models
{
    public class Vaccination
    {
        public int VaccinationId { get; set; }
        public DateTime VDate { get; set; }
        public int AnimalId { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int? EId { get; set; }
        public DateTime ExpDate { get; set; }
        public int? CompanyId { get; set; }
        public int? OutsiderId { get; set; }
        public bool Status { get; set; } = true;
        public string? AddedBy { get; set; } = "appUser";
        public DateTime? AddedDate { get; set; } = DateTime.Now;
        public string? AddedPc { get; set; } = "Default App User";
        public string? UpdatedBy { get; set; } = "appUser";
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
        public string? UpDatedPc { get; set; } = "Default App User";
    }
}
