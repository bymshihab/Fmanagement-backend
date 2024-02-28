using FarmProject.Models;
namespace FarmProject.Manager.Module
{
    public interface InterfaceModule
    {
        bool CreateModule(Moduless module);
        bool UpdateModuless(Moduless moduless);
        bool DeleteModule(int ModuleId);
        List<Moduless> getData();
    }
}
