using FarmProject.Models;
using System.ComponentModel.Design;

namespace FarmProject.Manager.UserParmissions
{
    public interface InterfaceUserPermission
    {
        void InsertUserPermission(IEnumerable<userpermissionPOST> userpost);

        IEnumerable<UserInfoAndCompany> GetAllUserInfo(int CompanyId);
        IEnumerable<allMenuList> GetallMenuList();
        IEnumerable<MenuByUserCodeAndCompany> GetMenuByUserCode(string UserCode, int CompanyId);
    }
}
