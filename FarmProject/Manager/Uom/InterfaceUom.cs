using FarmProject.Models;
namespace FarmProject.Manager.Uom
{
    public interface InterfaceUom
    {
        bool CreateUom(FarmProject.Models.Uom uom);
        bool UpdateUom(FarmProject.Models.Uom uom);
        bool DeleteUom(int UomId);
        List<FarmProject.Models.Uom> GetUoms(int CompanyId);
    }
}
