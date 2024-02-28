namespace FarmProject.Models
{
    public class GetSPAllMenu
    {
        public int? menuId { get; set; }
        public string moduleId { get; set; }
        public string menuName { get; set; }
        public string parentId { get; set; }
        public string pageName { get; set; }
        public string tabCaption { get; set; }
        public string navigateUrl { get; set; }
        public int tabWidth { get; set; }
        public int pageHeight { get; set; }
        public bool? isVisible { get; set; }
        public int seqNo { get; set; }
    }
}
