namespace FarmProject.Models
{
    public class Menus
    {
        public int? MenuId { get; set; }
        public int ModuleId { get; set; }
        public string? menuName { get; set; }
        public int? ParentId { get; set; }
        public string? PageName { get; set; }
        public string? TabCaption { get; set; }
        public string? NavigateUrl { get; set; }
        public int? TabWidth { get; set; }
        public int? PageHeight { get; set; }
        public bool? IsVisible { get; set; }
        public int? SeqNo { get; set; }
    }
}
