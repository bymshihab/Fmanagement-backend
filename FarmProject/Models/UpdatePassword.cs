namespace FarmProject.Models
{
    public class UpdatePassword
    {
        public string UserCode { get; set; }
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
    }
}
