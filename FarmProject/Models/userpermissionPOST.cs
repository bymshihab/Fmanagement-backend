namespace FarmProject.Models
{
    public class userpermissionPOST
    {
        public string UserCode { get; set; }
        public int MenuId { get; set; }
        public int CompanyId { get; set; }
        public bool CanView { get; set; }
        public bool CanAdd { get; set; }
        public bool CanModify { get; set; }
        public bool CanDelete { get; set; }
        public string? AddedBy { get; set; }
        public DateTime? DateAdded { get; set; }
        public string? AddedPC { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string? UpdatedPC { get; set; }
    }
}
