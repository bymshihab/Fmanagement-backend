namespace FarmProject.Models
{
    public class MedicalExhibitionById
    {
        public int? MedicalId { get; set; }
        public string? MedicalCode { get; set; }
        public DateTime ExhibitionDate { get; set; }
        public int AnimalId{ get; set; }
        public string AnimalName { get; set; }
        public int? OutsiderId { get; set; }
        public string? OutsiderName { get; set; }
        public int? EId { get; set; }
        public string EmployeeName { get; set; }
        public Boolean IsNew { get; set; } = false;
        public Boolean IsSick { get; set; } = false;
        public List<MedicineById> MedicineDetailsById { get; set; }
        public List<QuarantaineById> QuarantaineDetailsById { get; set; }

        public MedicalExhibitionById()

        {
            MedicineDetailsById = new List<MedicineById>();
            QuarantaineDetailsById = new List<QuarantaineById>();
        }
    }
}
