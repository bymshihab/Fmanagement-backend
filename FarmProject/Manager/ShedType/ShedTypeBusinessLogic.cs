using FarmProject.Data_Process_Logic;
using FarmProject.Models;

namespace FarmProject.Manager.ShedType
{
    public class ShedTypeBusinessLogic : InterfaceShedType
    {
        private readonly ShedTypeDataProcess shedTypeDataProcess;

        public ShedTypeBusinessLogic(ShedTypeDataProcess shedTypeDataProcess)
        {
            this.shedTypeDataProcess = shedTypeDataProcess;
        }

        public bool CreateShedType(ShedTypes shedType)
        {
            int res = shedTypeDataProcess.CreateShedType(shedType);
            return res > 0;
        }
        public bool UpdateShedType(ShedTypes shedType)
        {
            int res = shedTypeDataProcess.UpdateShedType(shedType);
            return res > 0;
        }
        public bool DeleteShedType(int ShedTypeId)
        {
            int isDeleted = shedTypeDataProcess.DeleteShedType(ShedTypeId);
            return isDeleted > 0;
        }

        public List<ShedTypes> GetShedType()
        {
            List<ShedTypes> shedTypes = shedTypeDataProcess.GetShedType();
            return shedTypes;
        }
    }
}