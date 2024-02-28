using FarmProject.Data_Process_Logic;
using FarmProject.Models;

namespace FarmProject.Manager.User
{
    public class UserBusinessLogic: InterfaceUser
    {
        private readonly UserDataProcess userDataProcess;

        public UserBusinessLogic(UserDataProcess userDataProcess)
        {
            this.userDataProcess = userDataProcess;
        }

        public bool CreateUser(UserInfo user)
        {
            int res = userDataProcess.CreateUser(user);
            return res > 0;
        }

        public bool UpdateUser(UserInfo user)
        {
            int res = userDataProcess.UpdateUser(user);
            return res > 0;
        }
        public void DeleteUser(string userCode)
        {
            bool isDeleted = userDataProcess.DeleteUser(userCode);

        }
        public  List<UserInfoGet> GetAllUserInfo(int CompanyId)
        {
            List<UserInfoGet> res = userDataProcess.GetAllUserInfo(CompanyId);
            return res;
        }
        public UserInfoPro GetUserInfoByUserCode(string userCode)
        { 
           UserInfoPro res = userDataProcess.GetUserInfoByUserCode(userCode);
            return res;
        }
        public UpdatePassword UpdatePassword(UpdatePassword user)
        {
            UpdatePassword res = userDataProcess.UpdatePassword(user);
            return res;
        }
    }
}
