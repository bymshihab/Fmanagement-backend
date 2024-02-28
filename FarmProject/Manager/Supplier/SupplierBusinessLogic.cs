using FarmProject.Data_Process_Logic;
using FarmProject.Models;

namespace FarmProject.Manager.Supplier
{
    public class SupplierBusinessLogic : InterfaceSupplier
    {
        private readonly SuppliersDataProcess suppliersDataProcess;

        public SupplierBusinessLogic(SuppliersDataProcess suppliersDataProcess)
        {
            this.suppliersDataProcess = suppliersDataProcess;
        }

        public bool CreateSupplier(Suppliers suppliers)
        {
            int res = suppliersDataProcess.CreateSupplier(suppliers);
            return res > 0;
        }

        public bool UpdateSupplier(Suppliers suppliers)
        {
            int res = suppliersDataProcess.UpdateSupplier(suppliers);
            return res > 0;
        }
        public bool DeleteSupplier(int SupplierId)
        {
            int isDeleted = suppliersDataProcess.DeleteSupplier(SupplierId);
            return isDeleted > 0;
        }

        public List<Suppliers> GetSupplier(int CompanyId)
        {
            List<Suppliers> suppliers = suppliersDataProcess.GetSupplier(CompanyId);
            return suppliers;
        }
    }
}
