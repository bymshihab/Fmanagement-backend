namespace FarmProject.Models
{
    public class MilkCollectionDetailById
    {
        public int MilkCollectionDetailId { get; set; }
        public int MilkCollectionId { get; set; }
        public int AnimalId { get; set; }
        public string AnimalName { get; set; }
        public int MilkId { get; set; }
        public string Milktype { get; set; }
        public decimal Qty { get; set; }
        public int UomId { get; set; }
        public string UomName { get; set; }
    }
}
