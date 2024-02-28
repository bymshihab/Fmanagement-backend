using FarmProject.ConnectionString;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using FarmProject.Models;
using System.ComponentModel.Design;
using Microsoft.AspNetCore.Mvc;

namespace FarmProject.Data_Process_Logic
{
    public class UserPermissionDataProcess
    {
        private readonly string ConnectionString;

        public UserPermissionDataProcess(Connection connection)
        {
            ConnectionString = connection.Cnstr;
        }

        public void InsertOrUpdateUserPermission([FromForm]IEnumerable<userpermissionPOST> userpost)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();



                SqlCommand cmddd = new SqlCommand("SELECT COUNT(*) FROM MenuPrivileges WHERE UserCode = @userCode;", conn);
                cmddd.Parameters.AddWithValue("@userCode", userpost.First().UserCode);
                int existingUserCount = (int)cmddd.ExecuteScalar();





                if (existingUserCount > 0)
                {
                    SqlCommand cmdddd = new SqlCommand("DELETE FROM MenuPrivileges WHERE UserCode = @userCode;", conn);
                    cmdddd.Parameters.AddWithValue("@userCode", userpost.First().UserCode);
                    cmdddd.ExecuteNonQuery();
                }




                foreach (userpermissionPOST item in userpost)
                {
                    SqlCommand cmd = new SqlCommand("spInsertUserPermission", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userCode", item.UserCode);
                    cmd.Parameters.AddWithValue("@MenuId", item.MenuId);
                    cmd.Parameters.AddWithValue("@CompanyId", item.CompanyId);
                    cmd.Parameters.AddWithValue("@CanView", item.CanView);
                    cmd.Parameters.AddWithValue("@CanAdd", item.CanAdd);
                    cmd.Parameters.AddWithValue("@CanModify", item.CanModify);
                    cmd.Parameters.AddWithValue("@CanDelete", item.CanDelete);
                    cmd.Parameters.AddWithValue("@AddedBy", "user Code");
                    cmd.Parameters.AddWithValue("@DateAdded", DateTime.Now);
                    cmd.Parameters.AddWithValue("@AddedPC", "0.0.0.0");
                    cmd.Parameters.AddWithValue("@UpdatedBy", "user Code");
                    cmd.Parameters.AddWithValue("@DateUpdated", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UpdatedPC", "0.0.0.0");

                    cmd.ExecuteNonQuery();
                }

                Console.WriteLine("User permissions inserted or updated successfully.");
            }
        }
        public IEnumerable<UserInfoAndCompany> GetAllUserInfo(int companyId)
        {
            List<UserInfoAndCompany> uplist = new List<UserInfoAndCompany>();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("GetAllUserInfo", conn);

                if (companyId > 0)
                {
                    cmd.Parameters.AddWithValue("@CompanyId", companyId);
                }

                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    UserInfoAndCompany userinfo = new UserInfoAndCompany();
                    userinfo.UserCode = reader["UserCode"].ToString();
                    userinfo.UserName = reader["UserName"].ToString();
                    userinfo.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                    userinfo.CompanyName = reader["CompanyName"].ToString();
                    uplist.Add(userinfo);
                }
                conn.Close();
            }

            return uplist;
        }
        public IEnumerable<allMenuList> GetallMenuList()
        {
            List<allMenuList> uplist = new List<allMenuList>();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("GetAllMenu", conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    allMenuList up = new allMenuList();
                    up.ModuleName = reader["ModuleName"].ToString();
                    up.MenuId = int.Parse(reader["MenuId"].ToString());
                    up.MenuName = reader["MenuName"].ToString();
                    uplist.Add(up);
                }
                conn.Close();
            }

            return uplist;
        }

        public IEnumerable<MenuByUserCodeAndCompany> GetMenuByUserCode(string UserCode, int CompanyId)
        {
            List<MenuByUserCodeAndCompany> ReportMenulist = new List<MenuByUserCodeAndCompany>();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("GetPermissionMenuByUserCodeAndCompanyId", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserCode", UserCode);
                cmd.Parameters.AddWithValue("@CompanyId", CompanyId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    MenuByUserCodeAndCompany ReportMenu = new MenuByUserCodeAndCompany();
                    ReportMenu.UserCode = reader["UserCode"].ToString();
                    int menuId;
                    if (int.TryParse(reader["MenuId"].ToString(), out menuId))
                    {
                        ReportMenu.MenuId = menuId;
                    }
                    bool canView;
                    if (bool.TryParse(reader["CanView"].ToString(), out canView))
                    {
                        ReportMenu.CanView = canView;
                    }
                    bool canAdd;
                    if (bool.TryParse(reader["CanAdd"].ToString(), out canAdd))
                    {
                        ReportMenu.CanAdd = canAdd;
                    }
                    bool canModify;
                    if (bool.TryParse(reader["CanModify"].ToString(), out canModify))
                    {
                        ReportMenu.CanModify = canModify;
                    }
                    bool canDelete;
                    if (bool.TryParse(reader["CanDelete"].ToString(), out canDelete))
                    {
                        ReportMenu.CanDelete = canDelete;
                    }
                    ReportMenulist.Add(ReportMenu);
                }
                conn.Close();
            }

            return ReportMenulist;
        }

    }
}
