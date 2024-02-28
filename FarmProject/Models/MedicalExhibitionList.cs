namespace FarmProject.Models
{
    public class MedicalExhibitionList
    {
        public List<Medicine> medicines { get; set; }
        public List<Quarantaine> quarantaines { get; set; }

        public MedicalExhibitionList()
        {
            medicines = new List<Medicine>();
            quarantaines = new List<Quarantaine>();
        }
    }
}
