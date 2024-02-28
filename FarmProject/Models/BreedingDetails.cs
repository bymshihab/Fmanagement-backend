namespace FarmProject.Models
{
    public class BreedingDetails
    {
        public int BreadingId { get; set; }
        public string EmployeeName { get; set; }
        public string ProductName { get; set; }
        public string Price { get; set; }
        public string SemenPer { get; set; }

        public DateTime? SemenDate { get; set; } = DateTime.Now;
        public DateTime? DeliveryDate { get; set; }
        public string AnimalName { get; set; }
        public string Outsider { get; set; }
        public bool? Status { get; set; } = true;
    }
}
