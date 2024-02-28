using FarmProject.ConnectionString;
using FarmProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace FarmProject.Data_Process_Logic
{
    public class MenuDataProcess
    {
        private readonly string ConnectionString;

        public MenuDataProcess(Connection connection)
        {
            ConnectionString = connection.Cnstr;
        }
        public int InsertMenu(Menus menu)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand getLastMenuIdCmd = new SqlCommand("SELECT MAX(MenuId) FROM Menu;", connection);
                connection.Open();

                // Handle DBNull when the result is null
                object result = getLastMenuIdCmd.ExecuteScalar();
                int lastMenuId = result == DBNull.Value ? 0 : Convert.ToInt32(result);

                menu.MenuId = lastMenuId + 1;

                SqlCommand command = new SqlCommand("InsertMenu", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@MenuId", menu.MenuId);
                command.Parameters.AddWithValue("@ModuleId", menu.ModuleId);
                command.Parameters.AddWithValue("@MenuName", menu.menuName);
                command.Parameters.AddWithValue("@ParentId", menu.ParentId);
                command.Parameters.AddWithValue("@PageName", menu.PageName ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@TabCaption", menu.TabCaption ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@NavigateUrl", menu.NavigateUrl ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@TabWidth", menu.TabWidth ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@PageHeight", menu.PageHeight ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@IsVisible", menu.IsVisible);
                command.Parameters.AddWithValue("@SeqNo", menu.SeqNo);

                int res = command.ExecuteNonQuery();
                connection.Close();

                return res;
            }
        }


        public int UpdateMenu([FromForm] Menus updatedMenu)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("UpdateMenu", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@MenuId", updatedMenu.MenuId);
                command.Parameters.AddWithValue("@ModuleId", updatedMenu.ModuleId);
                command.Parameters.AddWithValue("@MenuName", updatedMenu.menuName);
                command.Parameters.AddWithValue("@ParentId", updatedMenu.ParentId);
                command.Parameters.AddWithValue("@PageName", updatedMenu.PageName ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@TabCaption", updatedMenu.TabCaption ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@NavigateUrl", updatedMenu.NavigateUrl ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@TabWidth", updatedMenu.TabWidth ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@PageHeight", updatedMenu.PageHeight ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@IsVisible", updatedMenu.IsVisible);
                command.Parameters.AddWithValue("@SeqNo", updatedMenu.SeqNo);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                connection.Close();

                return rowsAffected;
            }
        }

        public int DeleteMenu(int menuId)
        {

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("DeleteMenu", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@MenuId", menuId);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                connection.Close();

                return rowsAffected;
            }
        }
        public List<GetSPAllMenu> GetSpAllMenu()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SpGetAllMenu", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<GetSPAllMenu> menuList = new List<GetSPAllMenu>();
                        while (reader.Read())
                        {
                            GetSPAllMenu menu = new GetSPAllMenu
                            {

                                menuId = reader["MenuId"] != DBNull.Value ? Convert.ToInt32(reader["MenuId"]) : 0,
                                moduleId = reader["ModuleName"].ToString(),
                                menuName = reader["MenuName"].ToString(),
                                parentId = reader["ParentName"].ToString(),
                                pageName = reader["PageName"].ToString(),
                                tabCaption = reader["TabCaption"].ToString(),
                                navigateUrl = reader["NavigateUrl"].ToString(),
                                tabWidth = reader["TabWidth"] != DBNull.Value ? Convert.ToInt32(reader["TabWidth"]) : 0,
                                pageHeight = reader["PageHeight"] != DBNull.Value ? Convert.ToInt32(reader["PageHeight"]) : 0,
                                isVisible = reader["IsVisible"] != DBNull.Value && Convert.ToBoolean(reader["IsVisible"]),
                                seqNo = reader["SeqNo"] != DBNull.Value ? Convert.ToInt32(reader["SeqNo"]) : 0

                            };
                            menuList.Add(menu);
                        }
                        return menuList;
                    }
                }
            }
        }
        public List<Dictionary<string, object>> GetAllMenu()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("GetAllMenu2", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        var result = new List<Dictionary<string, object>>();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            var rowData = new Dictionary<string, object>();
                            foreach (DataColumn column in dataTable.Columns)
                            {
                                rowData[column.ColumnName] = row[column];
                            }
                            result.Add(rowData);
                        }

                        return result;
                    }
                }
            }
        }

    }
}
