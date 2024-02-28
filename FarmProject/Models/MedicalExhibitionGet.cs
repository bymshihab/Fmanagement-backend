namespace FarmProject.Models
{
    public class MedicalExhibitionGet
    {
        public int? MedicalId { get; set; }
        public string? MedicalCode { get; set; }
        public DateTime ExhibitionDate { get; set; }
        public string AnimalName { get; set; }
        public string? OutsiderName { get; set; }
        public string EmployeeName { get; set; }
        public int CompanyId { get; set; }
    }
}
