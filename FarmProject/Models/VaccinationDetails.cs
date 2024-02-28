namespace FarmProject.Models
{
    public class VaccinationDetails
    {
        public int VaccinationId { get; set; }
        public DateTime VDate { get; set; }
        public string AnimalName { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string EmployeeName { get; set; }
        public DateTime ExpDate { get; set; }
        public string Outsider { get; set; }
        public bool Status { get; set; } = false;
    }
}
