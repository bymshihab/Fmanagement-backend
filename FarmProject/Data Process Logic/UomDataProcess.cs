using FarmProject.ConnectionString;
using FarmProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;

namespace FarmProject.Data_Process_Logic
{
    public class UomDataProcess
    {

        private readonly string ConnectionString;

        public UomDataProcess(Connection connection)
        {
            ConnectionString = connection.Cnstr;
        }
        public int CreateUom(Uom uom)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();

                    SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Uoms WHERE UomName = @UomName", con);
                    checkCmd.Parameters.AddWithValue("@UomName", uom.UomName);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count == 0)
                    {
                        return -1;
                    }
                    else
                    {
                        using (SqlCommand insertCmd = new SqlCommand("INSERT INTO Uoms (UomName, UomDescription, Status,CompanyId, AddedBy, AddedDate, AddedPc, UpdatedBy, UpdatedDate, UpdatedPc) VALUES (@UomName, @UomDescription, @Status,@CompanyId, @AddedBy, @AddedDate, @AddedPc, @UpdatedBy, @UpdatedDate, @UpdatedPc)", con))
                        {
                            insertCmd.Parameters.AddWithValue("@UomName", uom.UomName);
                            insertCmd.Parameters.AddWithValue("@UomDescription", uom.UomDescription ?? (object)DBNull.Value);
                            insertCmd.Parameters.AddWithValue("@Status", uom.Status);
                            insertCmd.Parameters.AddWithValue("@CompanyId", uom.CompanyId);
                            insertCmd.Parameters.AddWithValue("@AddedBy", uom.AddedBy);
                            insertCmd.Parameters.AddWithValue("@AddedDate", uom.AddedDate);
                            insertCmd.Parameters.AddWithValue("@AddedPc", uom.AddedPc);
                            insertCmd.Parameters.AddWithValue("@UpdatedBy", uom.UpdatedBy);
                            insertCmd.Parameters.AddWithValue("@UpdatedDate", uom.UpdatedDate);
                            insertCmd.Parameters.AddWithValue("@UpdatedPc", uom.UpDatedPc);

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
        public int UpdateUom(Uom uom)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();

                    SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Uoms WHERE UomId = @UomId", con);
                    checkCmd.Parameters.AddWithValue("@UomId", uom.UomId);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count == 0)
                    {
                        return -1;
                    }

                    using (SqlCommand updateCmd = new SqlCommand("UPDATE Uoms SET UomName = @UomName, UomDescription = @UomDescription, Status = @Status, UpdatedBy = @UpdatedBy, UpdatedDate = @UpdatedDate, UpdatedPc = @UpdatedPc WHERE UomId = @UomId", con))
                    {
                        updateCmd.Parameters.AddWithValue("@UomId", uom.UomId);
                        updateCmd.Parameters.AddWithValue("@UomName", uom.UomName);
                        updateCmd.Parameters.AddWithValue("@UomDescription", uom.UomDescription ?? (object)DBNull.Value);
                        updateCmd.Parameters.AddWithValue("@Status", uom.Status);
                        updateCmd.Parameters.AddWithValue("@UpdatedBy", uom.UpdatedBy);
                        updateCmd.Parameters.AddWithValue("@UpdatedDate", uom.UpdatedDate);
                        updateCmd.Parameters.AddWithValue("@UpdatedPc", uom.UpDatedPc);

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
        public int DeleteUom(int UomId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand command = new SqlCommand("DELETE FROM Uoms WHERE UomId = @UomId", connection);
                    command.CommandType = CommandType.Text;

                    command.Parameters.AddWithValue("@UomId", UomId);

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
        public List<Uom> GetUoms( int CompanyId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand getUomsCmd = new SqlCommand("SELECT UomId, UomName, UomDescription, Status FROM Uoms Where CompanyId=@CompanyId", connection);
                    getUomsCmd.Parameters.AddWithValue("@CompanyId", CompanyId);
                    connection.Open();
                    SqlDataReader reader = getUomsCmd.ExecuteReader();
                    List<Uom> uoms = new List<Uom>();

                    while (reader.Read())
                    {
                        Uom uom = new Uom
                        {
                            UomId = Convert.ToInt32(reader["UomId"]),
                            UomName = reader["UomName"].ToString(),
                            UomDescription = reader["UomDescription"].ToString(),
                            Status = reader["Status"] != DBNull.Value ? Convert.ToBoolean(reader["Status"]) : false,
                        };
                        uoms.Add(uom);
                    }

                    connection.Close();

                    return uoms;
                }
            }
            catch (Exception ex)
            {
                return new List<Uom>(); 
            }
        }

    }
}
