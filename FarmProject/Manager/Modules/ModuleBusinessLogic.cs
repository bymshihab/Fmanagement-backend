using FarmProject.Data_Process_Logic;
using FarmProject.Models;

namespace FarmProject.Manager.Module
{
    public class ModuleBusinessLogic : InterfaceModule
    {
        private readonly ModuleDataProcess moduleDataProcess;

        public ModuleBusinessLogic(ModuleDataProcess moduleDataProcess)
        {
            this.moduleDataProcess = moduleDataProcess;
        }

        public bool CreateModule(Moduless module)
        {
            int res = moduleDataProcess.CreateModule(module);
            return res > 0;
        }
        public bool DeleteModule(int ModuleId)
        {
            int isDeleted = moduleDataProcess.DeleteModule(ModuleId);
            return isDeleted > 0;
        }

        public bool UpdateModuless(Moduless moduless)
        {
            int res = moduleDataProcess.UpdateModuless(moduless);
            return res > 0;
        }
        public List<Moduless> getData()
        {
            List<Moduless> moduleModels = new List<Moduless>();
            moduleModels = moduleDataProcess.getData();
            if (moduleModels.Count > 0)
            {
                return moduleModels;
            }
            else
            {
                return moduleModels;
            }
        }

    }
}
