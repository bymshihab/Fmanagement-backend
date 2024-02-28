using FarmProject.ConnectionString;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace FarmProject.Data_Process_Logic
{
    public class StockDataProcess
    {

        private readonly string ConnectionString;

        public StockDataProcess(Connection connection)
        {
            ConnectionString = connection.Cnstr;
        }

        public List<Dictionary<string, object>> GetFeedStock(int CompanyId)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("GetFeedStock", connection))
                {
                    command.Parameters.AddWithValue("@CompanyId", CompanyId);
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

        public List<Dictionary<string, object>> GetAnimalSummary(int CompanyId)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("GetAnimalSummaryWithAvailability", connection))
                {
                    command.Parameters.AddWithValue("@CompanyId", CompanyId);
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

        [HttpGet("GetMedicineStock")]
        public List<Dictionary<string, object>> GetMedicineStock(int CompanyId)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("GetMedicineStock", connection))
                {
                    command.Parameters.AddWithValue("@CompanyId", CompanyId);
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

        public List<Dictionary<string, object>> GetVaccineStock(int CompanyId)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("GetVaccineStock", connection))
                {
                    command.Parameters.AddWithValue("@CompanyId", CompanyId);
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
        public List<Dictionary<string, object>> GetSemenStock(int CompanyId)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("GetSemenStock", connection))
                {
                    command.Parameters.AddWithValue("@CompanyId", CompanyId);
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
