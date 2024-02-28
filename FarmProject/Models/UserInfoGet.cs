namespace FarmProject.Models
{
    public class UserInfoGet
    {
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int? EmployeeId { get; set; }
        public string? Email { get; set; }
        public bool? IsAdmin { get; set; } = false;
        public bool? IsAudit { get; set; } = false;
        public bool? IsActive { get; set; } = true;
    }
}
