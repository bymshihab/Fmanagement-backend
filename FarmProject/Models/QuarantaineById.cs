namespace FarmProject.Models
{
    public class QuarantaineById
    {
        public DateTime? StartDate { get; set; } = DateTime.Now;
        public DateTime? EndDate { get; set; }
        public int? ShedId { get; set; }
        public string ShedName { get; set; }
        public int? MedicalId { get; set; }
    }
}
