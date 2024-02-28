namespace FarmProject.Models
{
    public class MenuByUserCodeAndCompany
    {
        public string UserCode { get; set; }
        public int MenuId { get; set; }
        public bool CanView { get; set; }
        public bool CanAdd { get; set; }
        public bool CanModify { get; set; }
        public bool CanDelete { get; set; }
        public int CompanyId { get; set; }
    }
}
