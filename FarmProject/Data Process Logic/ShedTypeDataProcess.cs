using FarmProject.ConnectionString;
using FarmProject.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Data.SqlClient;

namespace FarmProject.Data_Process_Logic
{
        public class ShedTypeDataProcess
        {
            private readonly string ConnectionString;

            public ShedTypeDataProcess(Connection connection)
            {
                ConnectionString = connection.Cnstr;
            }

        public int CreateShedType(ShedTypes shedType)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();

                    SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM ShedTypes WHERE shedTypeName = @shedTypeName", con);
                    checkCmd.Parameters.AddWithValue("@shedTypeName", shedType.shedTypeName);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count != 0)
                    {
                        return -1;
                    }
                    else
                    {
                        using (SqlCommand insertCmd = new SqlCommand("INSERT INTO ShedTypes (shedTypeName, ShedTypeDescription, Status, CompanyId, AddedBy, AddedDate, AddedPc, UpdatedBy, UpdatedDate, UpdatedPc) VALUES (@shedTypeName, @ShedTypeDescription, @Status, @CompanyId, @AddedBy, @AddedDate, @AddedPc, @UpdatedBy, @UpdatedDate, @UpdatedPc)", con))
                        {
                            insertCmd.Parameters.AddWithValue("@shedTypeName", shedType.shedTypeName);
                            insertCmd.Parameters.AddWithValue("@ShedTypeDescription", shedType.ShedTypeDescription ?? (object)DBNull.Value);
                            insertCmd.Parameters.AddWithValue("@Status", shedType.Status);
                            insertCmd.Parameters.AddWithValue("@CompanyId", shedType.CompanyId);
                            insertCmd.Parameters.AddWithValue("@AddedBy", shedType.AddedBy ?? (object)DBNull.Value);
                            insertCmd.Parameters.AddWithValue("@AddedDate", DateTime.Now);
                            insertCmd.Parameters.AddWithValue("@AddedPc", shedType.AddedPc ?? (object)DBNull.Value);
                            insertCmd.Parameters.AddWithValue("@UpdatedBy", shedType.UpdatedBy ?? (object)DBNull.Value);
                            insertCmd.Parameters.AddWithValue("@UpdatedDate", shedType.UpdatedDate ?? (object)DBNull.Value);
                            insertCmd.Parameters.AddWithValue("@UpdatedPc", shedType.UpDatedPc ?? (object)DBNull.Value);

                            insertCmd.ExecuteNonQuery();
                            return 1;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public int UpdateShedType(ShedTypes shedType)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();

                    SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM ShedTypes WHERE shedTypeId = @shedTypeId", con);
                    checkCmd.Parameters.AddWithValue("@shedTypeId", shedType.shedTypeId);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count == 0)
                    {
                        return -1;
                    }

                    using (SqlCommand updateCmd = new SqlCommand("UPDATE ShedTypes SET shedTypeName = @shedTypeName, ShedTypeDescription = @ShedTypeDescription, Status = @Status, UpdatedBy = @UpdatedBy, UpdatedDate = @UpdatedDate, UpdatedPc = @UpdatedPc WHERE ShedTypeId = @ShedTypeId", con))
                    {
                        updateCmd.Parameters.AddWithValue("@ShedTypeId", shedType.shedTypeId);
                        updateCmd.Parameters.AddWithValue("@shedTypeName", shedType.shedTypeName);
                        updateCmd.Parameters.AddWithValue("@ShedTypeDescription", shedType.ShedTypeDescription ?? (object)DBNull.Value);
                        updateCmd.Parameters.AddWithValue("@Status", shedType.Status);
                        updateCmd.Parameters.AddWithValue("@UpdatedBy", shedType.UpdatedBy ?? (object)DBNull.Value);
                        updateCmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                        updateCmd.Parameters.AddWithValue("@UpdatedPc", shedType.UpDatedPc ?? (object)DBNull.Value);

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

        public int DeleteShedType(int ShedTypeId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand command = new SqlCommand("DELETE ShedTypes WHERE ShedTypeId = @ShedTypeId", connection);
                    command.CommandType = CommandType.Text;

                    command.Parameters.AddWithValue("@ShedTypeId", ShedTypeId);

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
        public List<ShedTypes> GetShedType()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand getCmd = new SqlCommand("SELECT ShedTypeId, shedTypeName, ShedTypeDescription, Status FROM ShedTypes", connection);
                    connection.Open();
                    SqlDataReader reader = getCmd.ExecuteReader();
                    List<ShedTypes> shedTypes = new List<ShedTypes>();

                    while (reader.Read())
                    {
                        ShedTypes shedType = new ShedTypes
                        {
                            shedTypeId = Convert.ToInt32(reader["ShedTypeId"]),
                            shedTypeName = reader["shedTypeName"].ToString(),
                            ShedTypeDescription = reader["ShedTypeDescription"].ToString(),
                            Status = reader["Status"] != DBNull.Value ? Convert.ToBoolean(reader["Status"]) : false,
                        };
                        shedTypes.Add(shedType);
                    }

                    connection.Close();

                    return shedTypes;
                }
            }
            catch (Exception ex)
            {
                return new List<ShedTypes>();
            }
        }
    }
}
