namespace FarmProject.Models
{
    public class MedicineById
    {
        public int? MedicalId { get; set; }
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal day { get; set; }
        public string? time { get; set; }
        public decimal? Qty { get; set; }
        public int? UomId { get; set; }
        public string UomName { get; set;}
    }
}
