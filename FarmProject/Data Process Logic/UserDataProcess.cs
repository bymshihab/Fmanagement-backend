using FarmProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using FarmProject.ConnectionString;

namespace FarmProject.Data_Process_Logic
{
    public class UserDataProcess
    {
        private readonly string ConnectionString;

        public UserDataProcess(Connection connection)
        {
            ConnectionString = connection.Cnstr;
        }

        public int CreateUser([FromForm]UserInfo user)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();

                    using (SqlCommand cmdCount = new SqlCommand("SELECT COUNT(*) FROM UserInfo WHERE UserCode = @UserCode", con))
                    {
                        cmdCount.CommandType = CommandType.Text;
                        cmdCount.Parameters.AddWithValue("@UserCode", user.UserCode);
                        int count = (int)cmdCount.ExecuteScalar();

                        if (count > 0)
                        {
                            return -1;
                        }
                    }

                    string encryptedPassword = EncryptPassword(user.Password);

                    using (SqlCommand insertCmd = new SqlCommand("INSERT INTO UserInfo (UserCode, UserName, Password, EmployeeId, Email, CompanyId, IsAdmin, IsAudit, IsActive, AddedBy, DateAdded, AddedPC, UpdatedBy, DateUpdated, UpdatedPC) VALUES (@UserCode, @UserName, @Password, @EmployeeId, @Email, @CompanyId, @IsAdmin, @IsAudit, @IsActive, @AddedBy, @DateAdded, @AddedPC, @UpdatedBy, @DateUpdated, @UpdatedPC)", con))
                        {
                            insertCmd.CommandType = CommandType.Text;

                            insertCmd.Parameters.AddWithValue("@UserCode", user.UserCode);
                            insertCmd.Parameters.AddWithValue("@UserName", (user.UserName?.Length <= 200) ? user.UserName : user.UserName?.Substring(0, 200));
                            insertCmd.Parameters.AddWithValue("@Password", encryptedPassword);
                            insertCmd.Parameters.AddWithValue("@EmployeeId", user.EmployeeId.HasValue ? user.EmployeeId.Value : (object)DBNull.Value);
                            insertCmd.Parameters.AddWithValue("@Email", (user.Email?.Length <= 100) ? user.Email : user.Email?.Substring(0, 100) ?? (object)DBNull.Value);
                            insertCmd.Parameters.AddWithValue("@CompanyId", user.CompanyId);

                            insertCmd.Parameters.AddWithValue("@IsAdmin", user.IsAdmin ?? (object)DBNull.Value);
                            insertCmd.Parameters.AddWithValue("@IsAudit", user.IsAudit ?? (object)DBNull.Value);
                            insertCmd.Parameters.AddWithValue("@IsActive", user.IsActive ?? (object)DBNull.Value);

                            insertCmd.Parameters.AddWithValue("@AddedBy", user.AddedBy ?? (object)DBNull.Value);
                            insertCmd.Parameters.AddWithValue("@DateAdded", user.DateAdded ?? (object)DBNull.Value);
                            insertCmd.Parameters.AddWithValue("@AddedPC", user.AddedPC ?? (object)DBNull.Value);
                            insertCmd.Parameters.AddWithValue("@UpdatedBy", user.UpdatedBy ?? (object)DBNull.Value);
                            insertCmd.Parameters.AddWithValue("@DateUpdated", user.DateUpdated ?? (object)DBNull.Value);
                            insertCmd.Parameters.AddWithValue("@UpdatedPC", user.UpdatedPC ?? (object)DBNull.Value);

                        insertCmd.ExecuteNonQuery();

                        return 1; 
                    }
                }
            }
            catch (Exception ex)
            {
                return 0; 
            }
        }

        public int UpdateUser(UserInfo user)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();

                    using (SqlCommand cmdCount = new SqlCommand("SELECT COUNT(*) FROM UserInfo WHERE UserCode = @UserCode", con))
                    {
                        cmdCount.CommandType = CommandType.Text;
                        cmdCount.Parameters.AddWithValue("@UserCode", user.UserCode);
                        int count = (int)cmdCount.ExecuteScalar();

                        if (count == 0)
                        {
                            return -1;
                        }
                    }

                    string encryptedPassword = EncryptPassword(user.Password);

                    using (SqlCommand updateCmd = new SqlCommand("UPDATE UserInfo SET UserName = @UserName, Password = @Password, EmployeeId = @EmployeeId, Email = @Email, CompanyId = @CompanyId, IsAdmin = @IsAdmin, IsAudit = @IsAudit, IsActive = @IsActive, UpdatedBy = @UpdatedBy, DateUpdated = @DateUpdated, UpdatedPC = @UpdatedPC WHERE UserCode = @UserCode", con))
                    {
                        updateCmd.CommandType = CommandType.Text;

                        updateCmd.Parameters.AddWithValue("@UserCode", user.UserCode);
                        updateCmd.Parameters.AddWithValue("@UserName", (user.UserName?.Length <= 200) ? user.UserName : user.UserName?.Substring(0, 200));
                        updateCmd.Parameters.AddWithValue("@Password", encryptedPassword);
                        updateCmd.Parameters.AddWithValue("@EmployeeId", user.EmployeeId.HasValue ? user.EmployeeId.Value : (object)DBNull.Value);
                        updateCmd.Parameters.AddWithValue("@Email", (user.Email?.Length <= 100) ? user.Email : user.Email?.Substring(0, 100) ?? (object)DBNull.Value);
                        updateCmd.Parameters.AddWithValue("@CompanyId", user.CompanyId);

                        updateCmd.Parameters.AddWithValue("@IsAdmin", user.IsAdmin);
                        updateCmd.Parameters.AddWithValue("@IsAudit", user.IsAudit);
                        updateCmd.Parameters.AddWithValue("@IsActive", user.IsActive);

                        updateCmd.Parameters.AddWithValue("@UpdatedBy", user.UpdatedBy ?? (object)DBNull.Value);
                        updateCmd.Parameters.AddWithValue("@DateUpdated", user.DateUpdated ?? (object)DBNull.Value);
                        updateCmd.Parameters.AddWithValue("@UpdatedPC", user.UpdatedPC ?? (object)DBNull.Value);

                        updateCmd.ExecuteNonQuery();

                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                return 0; 
            }
        }

        public bool DeleteUser(string userCode)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("spDeleteUser", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserCode", userCode);

                        con.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string LoginUser(UserLogIn user)
        {
            try
            {
                string encryptedPassword = EncryptPassword(user.Password);

                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM UserInfo WHERE UserCode = @UserCode AND password = @password AND CompanyId = @CompanyId", con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@UserCode", user.UserCode);
                        cmd.Parameters.AddWithValue("@password", encryptedPassword);
                        cmd.Parameters.AddWithValue("@CompanyId", user.CompanyId);

                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            string encryptedUserCode = reader["UserCode"].ToString();
                            return encryptedUserCode;
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<UserInfoGet> GetAllUserInfo(int CompanyId)
        {
            try
            {
                List<UserInfoGet> userList = new List<UserInfoGet>();
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("GetUserByCompanyId", con))
                    {
                        cmd.Parameters.AddWithValue("@CompanyId", CompanyId);
                        cmd.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UserInfoGet user = new UserInfoGet
                                {
                                    UserCode = reader["UserCode"].ToString(),
                                    UserName = reader["UserName"].ToString(),
                                    EmployeeId = reader["EmployeeId"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["EmployeeId"]),
                                    Email = reader["Email"].ToString(),
                                    IsAdmin = reader["IsAdmin"] == DBNull.Value ? false : Convert.ToBoolean(reader["IsAdmin"]),
                                    IsAudit = reader["IsAudit"] == DBNull.Value ? false : Convert.ToBoolean(reader["IsAudit"]),
                                    IsActive = reader["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(reader["IsActive"])
                                };
                                user.Password = DecryptPassword(reader["Password"].ToString());
                                userList.Add(user);
                            }
                        }
                    }
                }
                return userList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public UserInfoPro GetUserInfoByUserCode(string userCode)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("GetUserInfoByUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserCode", userCode);
                        connection.Open();

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            if (dataTable.Rows.Count > 0)
                            {
                                DataRow row = dataTable.Rows[0];
                                var userInfo = new UserInfoPro
                                {
                                    UserCode = row["UserCode"].ToString(),
                                    UserName = row["UserName"].ToString(),
                                    Email = row["Email"].ToString(),
                                    CompanyName = row["CompanyName"].ToString()
                                };
                                return userInfo;
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public UpdatePassword UpdatePassword(UpdatePassword user)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string encryptedOldPassword = EncryptPassword(user.oldPassword);
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM UserInfo WHERE UserCode = @UserCode AND Password = @oldPassword", connection))
                    {
                        cmd.Parameters.AddWithValue("@UserCode", user.UserCode); 
                        cmd.Parameters.AddWithValue("@oldPassword", encryptedOldPassword);
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            reader.Close();
                            string encryptedPassword = EncryptPassword(user.newPassword);

                            using (SqlCommand command = new SqlCommand("UPDATE UserInfo SET Password = @newPassword WHERE UserCode = @UserCode", connection))
                            {
                                command.Parameters.AddWithValue("@UserCode", user.UserCode); 
                                command.Parameters.AddWithValue("@newPassword", encryptedPassword);
                                int rowsAffected = command.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    connection.Close();
                                    return user;
                                }
                            }
                        }

                        connection.Close();
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static string EncryptPassword(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        private string DecryptPassword(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            try
            {
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                return cipherText;
            }
            catch (FormatException ex)
            {
                return "Decryption failed: " + ex.Message;
            }
            catch (Exception ex)
            {
                return "An error occurred during decryption: " + ex.Message;
            }
        }
    }
}
