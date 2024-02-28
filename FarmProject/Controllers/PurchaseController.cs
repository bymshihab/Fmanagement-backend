using FarmProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using FarmProject.DetailData;
using System.Transactions;
using System.ComponentModel.Design;
using System.Diagnostics.Metrics;

namespace FarmProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public PurchaseController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        PurchaseDetailData purchaseDetailData = new PurchaseDetailData();

        [HttpPost]
        public IActionResult InsertPurchase([FromForm] Purchase purchase, [FromForm] string data)
        {
            List<PurchaseDetail> purchaseDetails = null;

            try
            {
                purchaseDetails = JsonConvert.DeserializeObject<List<PurchaseDetail>>(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during JSON deserialization: {ex.Message}");
                return BadRequest(new { message = "Error during JSON deserialization" });
            }

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    try
                    {
                        int lastPurchaseId = 0;
                        SqlCommand getLastPurchaseIdCmd = new SqlCommand("SELECT MAX(PurchaseId) FROM Purchases;", con, transaction);
                        object result = getLastPurchaseIdCmd.ExecuteScalar();

                        if (result != DBNull.Value && result != null)
                        {
                            lastPurchaseId = Convert.ToInt32(result);
                        }
                        purchase.PurchaseId = lastPurchaseId + 1;

                        string systemCode = string.Empty;
                        using (SqlCommand makeSystemCodeCmd = new SqlCommand("spMakeSystemCode", con, transaction))
                        {
                            makeSystemCodeCmd.CommandType = CommandType.StoredProcedure;
                            makeSystemCodeCmd.Parameters.AddWithValue("@TableName", "Purchases");
                            makeSystemCodeCmd.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"));
                            makeSystemCodeCmd.Parameters.AddWithValue("@AddNumber", 1);
                            systemCode = makeSystemCodeCmd.ExecuteScalar()?.ToString();
                        }

                        int purchaseId = int.Parse(systemCode.Split('%')[0]);
                        string purchaseCode = systemCode.Split('%')[1];

                        using (SqlCommand masterCmd = new SqlCommand("INSERT INTO Purchases (PurchaseId, PurchaseDate, PurchaseCode, SupplierId, EId, CompanyId, TotalPurchase, DeliveryCharge, ExtraCost, PurchaseDescription, AddedBy, AddedDate, AddedPc, UpdatedBy, UpdatedDate, UpdatedPc) VALUES (@PurchaseId, @PurchaseDate, @PurchaseCode, @SupplierId, @EId, @CompanyId, @TotalPurchase, @DeliveryCharge, @ExtraCost, @PurchaseDescription, @AddedBy, @AddedDate, @AddedPc, @UpdatedBy, @UpdatedDate, @UpdatedPc);", con, transaction))
                        {
                            masterCmd.Parameters.AddWithValue("@PurchaseId", purchaseId);
                            masterCmd.Parameters.AddWithValue("@PurchaseDate", DateTime.Now);
                            masterCmd.Parameters.AddWithValue("@PurchaseCode", purchaseCode);
                            masterCmd.Parameters.AddWithValue("@SupplierId", purchase.SupplierId);
                            masterCmd.Parameters.AddWithValue("@EId", purchase.EId);
                            masterCmd.Parameters.AddWithValue("@CompanyId", purchase.CompanyId ?? (object)DBNull.Value);
                            masterCmd.Parameters.AddWithValue("@TotalPurchase", purchase.TotalPurchase);
                            masterCmd.Parameters.AddWithValue("@DeliveryCharge", purchase.DeliveryCharge ?? (object)DBNull.Value);
                            masterCmd.Parameters.AddWithValue("@ExtraCost", purchase.ExtraCost ?? (object)DBNull.Value);
                            masterCmd.Parameters.AddWithValue("@PurchaseDescription", purchase.PurchaseDescription ?? (object)DBNull.Value);
                            masterCmd.Parameters.AddWithValue("@AddedBy", purchase.AddedBy ?? (object)DBNull.Value);
                            masterCmd.Parameters.AddWithValue("@AddedDate", DateTime.Now);
                            masterCmd.Parameters.AddWithValue("@AddedPc", purchase.AddedPc ?? (object)DBNull.Value);
                            masterCmd.Parameters.AddWithValue("@UpdatedBy", purchase.UpdatedBy ?? (object)DBNull.Value);
                            masterCmd.Parameters.AddWithValue("@UpdatedDate", purchase.UpdatedDate ?? (object)DBNull.Value);
                            masterCmd.Parameters.AddWithValue("@UpDatedPc", purchase.UpDatedPc ?? (object)DBNull.Value);

                            masterCmd.ExecuteNonQuery();
                        }

                        purchaseDetailData.InsertPurchaseDetails(purchaseDetails, con, transaction, purchaseId);

                        transaction.Commit();

                        return Ok(new { message = "Purchase Data inserted successfully", purchaseId });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        return StatusCode(500, new { message = "An error occurred while processing the request", error = ex.Message });
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }
        }
        [HttpPut("UpdatePurchase")]
        public IActionResult PurchaseDataUpdate([FromForm] Purchase purchase, [FromForm] string data)
        {
            List<PurchaseDetail> purchaseDetails = null;

            try
            {
                purchaseDetails = JsonConvert.DeserializeObject<List<PurchaseDetail>>(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during JSON deserialization: {ex.Message}");
                return BadRequest(new { message = "Error during JSON deserialization" });
            }
            SqlConnection con = new SqlConnection(_connectionString);
            try
            {
                SqlCommand updatePurchaseCmd = new SqlCommand("UPDATE Purchases SET PurchaseDate = @PurchaseDate, SupplierId = @SupplierId, EId = @EId, TotalPurchase = @TotalPurchase, DeliveryCharge = @DeliveryCharge, ExtraCost = @ExtraCost, PurchaseDescription = @PurchaseDescription, UpdatedBy = @UpdatedBy, UpdatedDate = @UpdatedDate, UpdatedPc = @UpdatedPc WHERE PurchaseId = @PurchaseId", con);

                updatePurchaseCmd.Parameters.AddWithValue("@PurchaseId", purchase.PurchaseId);
                updatePurchaseCmd.Parameters.AddWithValue("@PurchaseDate", DateTime.Now);
                updatePurchaseCmd.Parameters.AddWithValue("@SupplierId", purchase.SupplierId);
                updatePurchaseCmd.Parameters.AddWithValue("@EId", purchase.EId);
                updatePurchaseCmd.Parameters.AddWithValue("@TotalPurchase", purchase.TotalPurchase);
                updatePurchaseCmd.Parameters.AddWithValue("@DeliveryCharge", purchase.DeliveryCharge ?? 0);
                updatePurchaseCmd.Parameters.AddWithValue("@ExtraCost", purchase.ExtraCost ?? 0);
                updatePurchaseCmd.Parameters.AddWithValue("@PurchaseDescription", purchase.PurchaseDescription ?? (object)DBNull.Value);
                updatePurchaseCmd.Parameters.AddWithValue("@UpdatedBy", purchase.UpdatedBy ?? (object)DBNull.Value);
                updatePurchaseCmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                updatePurchaseCmd.Parameters.AddWithValue("@UpDatedPc", purchase.UpDatedPc ?? (object)DBNull.Value);

                con.Open();
                updatePurchaseCmd.ExecuteNonQuery();
                con.Close();

                purchaseDetailData.UpdatePurchaseDetails(purchaseDetails, con, (int)purchase.PurchaseId);


                return Ok(new { message = "Purchase Data Updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("{purchaseId}")]
        public IActionResult DeletePurchase(int purchaseId)
        {
            SqlConnection con = new SqlConnection(_connectionString);
            try
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Purchases WHERE purchaseId = @purchaseId;", con);
                cmd.Parameters.AddWithValue("@purchaseId", purchaseId);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                purchaseDetailData.DeleteAllDetails(con, purchaseId);
                return Ok(new { message = "Purchase Data Deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest($"Could not delete {ex.Message}");
            }
        }
        [HttpGet("GetPurchaseData")]
        public IActionResult GetPurchaseData(int CompanyId)
        {
            List<PurchaseGet> purchaseList = new List<PurchaseGet>();

            SqlConnection con = new SqlConnection(_connectionString);

            try
            {
                SqlCommand cmd = new SqlCommand("GetActivePurchases", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@CompanyId", CompanyId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    PurchaseGet master = new PurchaseGet();
                    master.PurchaseId = Convert.ToInt32(reader["PurchaseId"].ToString());
                    master.PurchaseDate = (DateTime)reader["PurchaseDate"];
                    master.PurchaseCode = reader["PurchaseCode"].ToString();
                    master.PurchaseDescription = reader["PurchaseDescription"].ToString();
                    master.SupplierName = reader["SupplierName"].ToString();
                    master.EmployeeName = reader["EmployeeName"].ToString();
                    master.TotalPurchase = (Decimal)reader["TotalPurchase"];

                    purchaseList.Add(master);
                }

                con.Close();
                return Ok(purchaseList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{PurchaseId}")]
        public IActionResult GetOnePurchaseData(int PurchaseId)
        {
            PurchaseGetById master = null;

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("GetAllPurchases", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PurchaseId", PurchaseId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                master = new PurchaseGetById
                                {
                                    PurchaseId = Convert.ToInt32(reader["PurchaseId"]),
                                    //PurchaseDate = (DateTime)reader["PurchaseDate"],
                                    PurchaseDate = reader["PurchaseDate"] != DBNull.Value ? (DateTime)reader["PurchaseDate"] : default(DateTime),
                                    PurchaseCode = reader["PurchaseCode"].ToString(),
                                    PurchaseDescription = reader["PurchaseDescription"].ToString(),
                                    SupplierName = reader["SupplierName"].ToString(),
                                    SupplierId = Convert.ToInt32(reader["SupplierId"]),
                                    EId = Convert.ToInt32(reader["EId"]),
                                    EmployeeName = reader["EmployeeName"].ToString(),
                                    TotalPurchase = (decimal)reader["TotalPurchase"],
                                    DeliveryCharge = Convert.IsDBNull(reader["DeliveryCharge"]) ? 0 : (decimal)reader["DeliveryCharge"],
                                    ExtraCost = Convert.IsDBNull(reader["ExtraCost"]) ? 0 : (decimal)reader["ExtraCost"],
                                };
                            }
                        }
                    }

                    if (master != null)
                    {
                        List<GetPurchaseDetailsById> purchaseDetailsList = new List<GetPurchaseDetailsById>();
                        purchaseDetailData.GetAllDetails(con, PurchaseId, purchaseDetailsList);
                        master.PurchaseDetails = purchaseDetailsList;
                    }
                }

                return master != null ? Ok(master) : NotFound();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetOnePurchaseData: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                return StatusCode(500, new { message = "An error occurred while processing the request" });
            }
        }
        [HttpGet("GetCategoryAndProduct")]
        public IActionResult GetActiveAnimalsForMilkCollection(int CompanyId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("CatAndProd", connection))
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

                        return Ok(result);
                    }
                }
            }
        }

    }
}
