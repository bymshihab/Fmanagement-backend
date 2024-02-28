using FarmProject.Models;

namespace FarmProject.Manager.ShedType
{
    public interface InterfaceShedType
    {
        bool CreateShedType(ShedTypes shedType);
        bool UpdateShedType(ShedTypes shedType);
        bool DeleteShedType(int ShedTypeId);
        List<ShedTypes> GetShedType();
    }
}
