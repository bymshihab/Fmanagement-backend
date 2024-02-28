using FarmProject.Data_Process_Logic;
using FarmProject.Models;

namespace FarmProject.Manager.UserParmissions
{
    public class UserPermissionBusinessLogic: InterfaceUserPermission
    {

        private readonly UserPermissionDataProcess userPermissionDataProcess;

        public UserPermissionBusinessLogic(UserPermissionDataProcess userPermissionDataProcess)
        {
            this.userPermissionDataProcess = userPermissionDataProcess;
        }

        public void InsertUserPermission(IEnumerable<userpermissionPOST> userpost)
        {
            userPermissionDataProcess.InsertOrUpdateUserPermission(userpost);
        }
        public IEnumerable<UserInfoAndCompany> GetAllUserInfo(int CompanyId)
        {
            var resultlist = userPermissionDataProcess.GetAllUserInfo(CompanyId);
            return (IEnumerable<UserInfoAndCompany>)resultlist;
        }
        public IEnumerable<allMenuList> GetallMenuList()
        {
            var resultlist = userPermissionDataProcess.GetallMenuList();
            return (IEnumerable<allMenuList>)resultlist;
        }

        public IEnumerable<MenuByUserCodeAndCompany> GetMenuByUserCode(string UserCode, int CompanyId)
        {
            var resultlist = userPermissionDataProcess.GetMenuByUserCode(UserCode, CompanyId);
            return (IEnumerable<MenuByUserCodeAndCompany>)resultlist;
        }

    }
}
