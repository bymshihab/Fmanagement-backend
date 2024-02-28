using FarmProject.ConnectionString;
using FarmProject.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace FarmProject.Data_Process_Logic
{
    public class ModuleDataProcess
    {
        private readonly string ConnectionString;

        public ModuleDataProcess(Connection connection)
        {
            ConnectionString = connection.Cnstr;
        }

        public int CreateModule(Moduless module)
        {
            try
            {
                // Ensure SeqNo is set
                module.SeqNo = module.SeqNo ?? 1;

                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();

                    SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Module WHERE ModuleName = @ModuleName", con);
                    checkCmd.Parameters.AddWithValue("@ModuleName", module.ModuleName);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count != 0)
                    {
                        return -1;
                    }
                    else
                    {
                        using (SqlCommand insertModuleCmd = new SqlCommand("INSERT INTO Module (ModuleName, Description, IsDefault, IsActive, SeqNo, AddedBy, AddedDate, AddedPc, UpdatedBy, UpdatedDate, UpDatedPc) " +
                            "VALUES (@ModuleName, @Description, @IsDefault, @IsActive, @SeqNo, @AddedBy, @AddedDate, @AddedPc, @UpdatedBy, @UpdatedDate, @UpDatedPc);", con))
                        {
                            insertModuleCmd.Parameters.AddWithValue("@ModuleName", module.ModuleName);
                            insertModuleCmd.Parameters.AddWithValue("@Description", module.Description);
                            insertModuleCmd.Parameters.AddWithValue("@IsDefault", module.IsDefault);
                            insertModuleCmd.Parameters.AddWithValue("@IsActive", module.IsActive);
                            insertModuleCmd.Parameters.AddWithValue("@SeqNo", module.SeqNo.HasValue ? (object)module.SeqNo.Value : DBNull.Value);
                            insertModuleCmd.Parameters.AddWithValue("@AddedBy", module.AddedBy ?? (object)DBNull.Value);
                            insertModuleCmd.Parameters.AddWithValue("@AddedDate", DateTime.Now);
                            insertModuleCmd.Parameters.AddWithValue("@AddedPc", module.AddedPc ?? (object)DBNull.Value);
                            insertModuleCmd.Parameters.AddWithValue("@UpdatedBy", module.UpdatedBy ?? (object)DBNull.Value);
                            insertModuleCmd.Parameters.AddWithValue("@UpdatedDate", module.UpdatedDate != null ? (object)module.UpdatedDate : DBNull.Value);
                            insertModuleCmd.Parameters.AddWithValue("@UpDatedPc", module.UpDatedPc ?? (object)DBNull.Value);

                            insertModuleCmd.ExecuteNonQuery();
                            return 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }

                return 0;
            }
        }

        public int UpdateModuless(Moduless moduless)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();

                    SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Module WHERE ModuleId = @ModuleId", con);
                    checkCmd.Parameters.AddWithValue("@ModuleId", moduless.ModuleId);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count == 0)
                    {
                        return -1;
                    }

                    using (SqlCommand updateCmd = new SqlCommand("UPDATE Module SET ModuleName = @ModuleName, Description = @Description, IsDefault = @IsDefault, IsActive = @IsActive, SeqNo = @SeqNo, UpdatedBy = @UpdatedBy, UpdatedDate = @UpdatedDate, UpDatedPc = @UpDatedPc WHERE ModuleId = @ModuleId", con))
                    {
                        updateCmd.Parameters.AddWithValue("@ModuleId", moduless.ModuleId);
                        updateCmd.Parameters.AddWithValue("@ModuleName", moduless.ModuleName);
                        updateCmd.Parameters.AddWithValue("@Description", moduless.Description);
                        updateCmd.Parameters.AddWithValue("@IsDefault", moduless.IsDefault);
                        updateCmd.Parameters.AddWithValue("@IsActive", moduless.IsActive);
                        updateCmd.Parameters.AddWithValue("@SeqNo", moduless.SeqNo);
                        updateCmd.Parameters.AddWithValue("@UpdatedBy", moduless.UpdatedBy ?? (object)DBNull.Value);
                        updateCmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                        updateCmd.Parameters.AddWithValue("@UpDatedPc", moduless.UpDatedPc ?? (object)DBNull.Value);

                        updateCmd.ExecuteNonQuery();
                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }

                return 0;
            }
        }

        public List<Moduless> getData()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand getModuleCmd = new SqlCommand("SELECT * FROM Module where IsActive=1", connection);

                connection.Open();
                SqlDataReader reader = getModuleCmd.ExecuteReader();

                List<Moduless> modules = new List<Moduless>();

                while (reader.Read())
                {
                    Moduless module = new Moduless
                    {
                        ModuleId = reader.GetInt32(reader.GetOrdinal("ModuleId")),
                        ModuleName = reader.GetString(reader.GetOrdinal("ModuleName")),
                        Description = reader.GetString(reader.GetOrdinal("Description")),
                        IsDefault = reader.GetBoolean(reader.GetOrdinal("IsDefault")),
                        IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                        SeqNo = reader.GetInt32(reader.GetOrdinal("SeqNo"))
                    };

                    modules.Add(module);
                }

                connection.Close();
                return modules;
            }
        }



        public int DeleteModule(int ModuleId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand command = new SqlCommand("DELETE FROM Module WHERE ModuleId = @ModuleId ", connection);
                    command.CommandType = CommandType.Text;

                    command.Parameters.AddWithValue("@ModuleId", ModuleId);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();
                    if (rowsAffected > 0)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
