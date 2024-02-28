using FarmProject.Models;

namespace FarmProject.Manager.User
{
    public interface InterfaceUser
    {
        bool CreateUser(UserInfo user);
        bool UpdateUser(UserInfo user);
        void DeleteUser(string userCode);
        List<UserInfoGet> GetAllUserInfo(int CompanyId);
        UserInfoPro GetUserInfoByUserCode(string userCode);
        UpdatePassword UpdatePassword(UpdatePassword user);
    }
}
