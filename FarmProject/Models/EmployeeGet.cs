namespace FarmProject.Models
{
    public class EmployeeGet
    {
        public string? EmployeeCode { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public decimal? Salary { get; set; }
        public DateTime? DOB { get; set; }
        public DateTime? HireDate { get; set; } = DateTime.Now;
        public int? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public long? NID { get; set; }
        public string? Description { get; set; }
        public string? JobTitle { get; set; }
        public bool Status { get; set; } = true;
        public string? EmployeeImage { get; set; }
    }
}
