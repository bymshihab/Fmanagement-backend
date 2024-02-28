using FarmProject.Data_Process_Logic;
using FarmProject.Models;

namespace FarmProject.Manager.Uom
{
    public class UomBusinessLogic : InterfaceUom
    {
        private readonly UomDataProcess uomDataProcess;

        public UomBusinessLogic(UomDataProcess uomDataProcess)
        {
            this.uomDataProcess = uomDataProcess;
        }

        public bool CreateUom(FarmProject.Models.Uom uom)
        {
            int res = uomDataProcess.CreateUom(uom);
            return res > 0;
        }

        public bool UpdateUom(FarmProject.Models.Uom uom)
        {
            int res = uomDataProcess.UpdateUom(uom);
            return res > 0;
        }
        public bool DeleteUom(int UomId)
        {
            int isDeleted = uomDataProcess.DeleteUom(UomId);
            return isDeleted > 0;
        }

        public List<FarmProject.Models.Uom> GetUoms(int CompanyId)
        {
            List<FarmProject.Models.Uom> uoms = uomDataProcess.GetUoms(CompanyId);
            return uoms;
        }

    }
}
