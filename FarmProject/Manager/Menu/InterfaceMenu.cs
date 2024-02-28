using FarmProject.Models;

namespace FarmProject.Manager.Menu
{
    public interface InterfaceMenu
    {
        int InsertMenu(Menus menu);
        int UpdateMenu(Menus updatedMenu);
        int DeleteMenu(int menuId);
        List<GetSPAllMenu> GetSpAllMenu();
        List<Dictionary<string, object>> GetAllMenu();
    }
}
