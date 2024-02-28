using FarmProject.Data_Process_Logic;
using FarmProject.Models;

namespace FarmProject.Manager.Stock
{
    public class StockBusinessLogic: InterfaceStock
    {
        private readonly StockDataProcess stockDataProcess;

        public StockBusinessLogic(StockDataProcess stockDataProcess)
        {
            this.stockDataProcess = stockDataProcess;
        }

        public List<Dictionary<string, object>> GetFeedStock(int CompanyId)
        {
            var res = stockDataProcess.GetFeedStock(CompanyId);
            return res;
        }
        public List<Dictionary<string, object>> GetAnimalSummary(int CompanyId)
        {
            var res = stockDataProcess.GetAnimalSummary(CompanyId);
            return res;
        }

        public List<Dictionary<string, object>> GetMedicineStock(int CompanyId)
        {
            var res = stockDataProcess.GetMedicineStock(CompanyId);
            return res;
        }

        public List<Dictionary<string, object>> GetVaccineStock(int CompanyId)
        {
            var res = stockDataProcess.GetVaccineStock(CompanyId);
            return res;
        }
        public List<Dictionary<string, object>> GetSemenStock(int CompanyId)
        {
            var res = stockDataProcess.GetSemenStock(CompanyId);
            return res;
        }
    }
}
