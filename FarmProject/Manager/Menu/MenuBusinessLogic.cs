using FarmProject.Data_Process_Logic;
using FarmProject.Models;

namespace FarmProject.Manager.Menu
{
    public class MenuBusinessLogic : InterfaceMenu
    {
        private readonly MenuDataProcess menuDataProcess;

        public MenuBusinessLogic(MenuDataProcess menuDataProcess)
        {
            this.menuDataProcess = menuDataProcess;
        }

        public int InsertMenu(Menus menu)
        {
            int res = menuDataProcess.InsertMenu(menu);
            return res;
        }
        public int UpdateMenu(Menus updatedMenu)
        {
            int res = menuDataProcess.UpdateMenu(updatedMenu);
            return res;
        }

        public int DeleteMenu(int menuId)
        {
            int res = menuDataProcess.DeleteMenu(menuId);
            return res;
        }
        public List<GetSPAllMenu> GetSpAllMenu()
        {
            var res = menuDataProcess.GetSpAllMenu();
            return res;
        }
        public List<Dictionary<string, object>> GetAllMenu()
        {
            var res = menuDataProcess.GetAllMenu();
            return res;
        }
    }
}
