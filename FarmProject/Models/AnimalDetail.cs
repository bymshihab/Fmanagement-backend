namespace FarmProject.Models
{
    public class AnimalDetail
    {
        public int AnimalId { get; set; }
        public string AnimalImage { get; set; }
        public string AnimalName { get; set; }
        public string AnimalTagNo { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ShedId { get; set; }
        public string ShedName { get; set; }
        public decimal Weight { get; set; }
        public int GenderId { get; set; }
        public string GenderType { get; set; }
        public int? MilkId { get; set; }
        public string? MilkType { get; set; }
        public bool IsDead { get; set; }
        public bool IsSold { get; set; }
        public bool IsVaccinated { get; set; }
        public bool Status { get; set; }
        public DateTime? DOB { get; set; }
        public string QRCodeData { get; set; }
        public string QRCodeImageBase64 { get; set; }
    }
}
