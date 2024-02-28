namespace FarmProject.Manager.Stock
{
    public interface InterfaceStock
    {
        List<Dictionary<string, object>> GetFeedStock(int CompanyId);
        List<Dictionary<string, object>> GetAnimalSummary(int CompanyId);
        List<Dictionary<string, object>> GetMedicineStock(int CompanyId);
        List<Dictionary<string, object>> GetVaccineStock(int CompanyId);
        List<Dictionary<string, object>> GetSemenStock(int CompanyId);
    }
}
