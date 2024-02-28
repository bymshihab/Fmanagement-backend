using FarmProject.Models;

namespace FarmProject.Manager.Supplier
{
    public interface InterfaceSupplier
    {
        bool CreateSupplier(Suppliers suppliers);
        bool UpdateSupplier(Suppliers suppliers);
        bool DeleteSupplier(int SupplierId);
        List<Suppliers> GetSupplier(int CompanyId);
    }
}
