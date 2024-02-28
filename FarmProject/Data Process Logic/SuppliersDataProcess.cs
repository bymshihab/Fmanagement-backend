using FarmProject.ConnectionString;
using FarmProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace FarmProject.Data_Process_Logic
{
    public class SuppliersDataProcess
    {

        private readonly string ConnectionString;

        public SuppliersDataProcess(Connection connection)
        {
            ConnectionString = connection.Cnstr;
        }
        public int CreateSupplier(Suppliers suppliers)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();

                    SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Suppliers WHERE SupplierName = @SupplierName", con);
                    checkCmd.Parameters.AddWithValue("@SupplierName", suppliers.SupplierName);
                    int count = (int)checkCmd.ExecuteScalar();
                    if (count == 0)
                    {
                        return -1;
                    }
                    else
                    {
                        using (SqlCommand insertCmd = new SqlCommand("INSERT INTO Suppliers (SupplierName, PhoneNo, Email, Address, Status,CompanyId, AddedBy, AddedDate, AddedPc, UpdatedBy, UpdatedDate, UpdatedPc) VALUES (@SupplierName, @PhoneNo, @Email, @Address, @Status,@CompanyId, @AddedBy, @AddedDate, @AddedPc, @UpdatedBy, @UpdatedDate, @UpdatedPc)", con))
                        {
                            insertCmd.Parameters.AddWithValue("@SupplierName", suppliers.SupplierName);
                            insertCmd.Parameters.AddWithValue("@PhoneNo", suppliers.PhoneNo ?? (object)DBNull.Value);
                            insertCmd.Parameters.AddWithValue("@Email", suppliers.Email ?? (object)DBNull.Value);
                            insertCmd.Parameters.AddWithValue("@Address", suppliers.Address ?? (object)DBNull.Value);
                            insertCmd.Parameters.AddWithValue("@Status", suppliers.Status);
                            insertCmd.Parameters.AddWithValue("@CompanyId", suppliers.CompanyId);
                            insertCmd.Parameters.AddWithValue("@AddedBy", suppliers.AddedBy);
                            insertCmd.Parameters.AddWithValue("@AddedDate", suppliers.AddedDate);
                            insertCmd.Parameters.AddWithValue("@AddedPc", suppliers.AddedPc);
                            insertCmd.Parameters.AddWithValue("@UpdatedBy", suppliers.UpdatedBy);
                            insertCmd.Parameters.AddWithValue("@UpdatedDate", suppliers.UpdatedDate);
                            insertCmd.Parameters.AddWithValue("@UpdatedPc", suppliers.UpDatedPc);

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
        public int UpdateSupplier(Suppliers suppliers)
        {
            try
            {

                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    using (SqlCommand updateCmd = new SqlCommand("UPDATE Suppliers SET SupplierName = @SupplierName, PhoneNo = @PhoneNo, Email = @Email, Address = @Address, Status = @Status, UpdatedBy = @UpdatedBy, UpdatedDate = @UpdatedDate, UpdatedPc = @UpdatedPc WHERE SupplierId = @SupplierId", con))
                    {
                        updateCmd.Parameters.AddWithValue("@SupplierId", suppliers.SupplierId);
                        updateCmd.Parameters.AddWithValue("@SupplierName", suppliers.SupplierName);
                        updateCmd.Parameters.AddWithValue("@PhoneNo", suppliers.PhoneNo ?? (object)DBNull.Value);
                        updateCmd.Parameters.AddWithValue("@Email", suppliers.Email ?? (object)DBNull.Value);
                        updateCmd.Parameters.AddWithValue("@Address", suppliers.Address);
                        updateCmd.Parameters.AddWithValue("@Status", suppliers.Status);
                        updateCmd.Parameters.AddWithValue("@UpdatedBy", suppliers.UpdatedBy);
                        updateCmd.Parameters.AddWithValue("@UpdatedDate", suppliers.UpdatedDate);
                        updateCmd.Parameters.AddWithValue("@UpdatedPc", suppliers.UpDatedPc);

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
        public int DeleteSupplier(int SupplierId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand command = new SqlCommand("DELETE FROM Suppliers WHERE SupplierId = @SupplierId", connection);
                    command.CommandType = CommandType.Text;

                    command.Parameters.AddWithValue("@SupplierId", SupplierId);

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
        public List<Suppliers> GetSupplier(int CompanyId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand getSupplierCmd = new SqlCommand("SELECT SupplierId, SupplierName, PhoneNo, Email, Address, Status FROM Suppliers Where CompanyId = @CompanyId", connection);
                    getSupplierCmd.Parameters.AddWithValue("@CompanyId", CompanyId);
                    connection.Open();
                    SqlDataReader reader = getSupplierCmd.ExecuteReader();
                    List<Suppliers> suppliers = new List<Suppliers>();
                    while (reader.Read())
                    {
                        Suppliers supplier = new Suppliers
                        {
                            SupplierId = Convert.ToInt32(reader["SupplierId"]),
                            SupplierName = reader["SupplierName"].ToString(),
                            PhoneNo = reader["PhoneNo"].ToString(),
                            Email = reader["Email"].ToString(),
                            Address = reader["Address"].ToString(),
                            Status = reader["Status"] != DBNull.Value ? Convert.ToBoolean(reader["Status"]) : false,
                        };
                        suppliers.Add(supplier);
                    }

                    connection.Close();
                    return suppliers;
                }
            }
            catch (Exception ex)
            {
                return new List<Suppliers>();
            }
        }
    }
}
