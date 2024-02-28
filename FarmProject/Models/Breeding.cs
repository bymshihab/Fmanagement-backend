namespace FarmProject.Models
{
    public class Breeding
    {
        public int BreadingId { get; set; }
        public int EId { get; set; }
        public int ProductId { get; set; }
        public decimal? Price { get; set; }
        public decimal? SemenPer { get; set; }
        public DateTime? SemenDate { get; set; }= DateTime.Now;
        public DateTime? DeliveryDate { get; set; }
        public int AnimalID { get; set; }
        public int OutsiderId { get; set; }
        public int? CompanyId { get; set; }
        public bool? Status { get; set; } = true;
        public string? AddedBy { get; set; } = "appUser";
        public DateTime? AddedDate { get; set; } = DateTime.Now;
        public string? AddedPc { get; set; } = "Default App User";
        public string? UpdatedBy { get; set; } = "appUser";
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
        public string? UpDatedPc { get; set; } = "Default App User";
    }
}
