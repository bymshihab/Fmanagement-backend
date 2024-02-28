using FarmProject.DetailData;
using FarmProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;

namespace FarmProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public SaleController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }
        SaleDetailData SaleData = new SaleDetailData();
        [HttpPost]
        public IActionResult InsertSale([FromForm] Sale sale, [FromForm] string data)
        {
            List<SaleDetail> saleDetails = null;

            try
            {
                saleDetails = JsonConvert.DeserializeObject<List<SaleDetail>>(data);
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
                        int lastSaleId = 0;
                        SqlCommand getLastsaleIdCmd = new SqlCommand("SELECT MAX(SaleId) FROM Sale;", con, transaction);
                        object result = getLastsaleIdCmd.ExecuteScalar();

                        if (result != DBNull.Value && result != null)
                        {
                            lastSaleId = Convert.ToInt32(result);
                        }
                        sale.SaleId = lastSaleId + 1;

                        string systemCode = string.Empty;
                        using (SqlCommand makeSystemCodeCmd = new SqlCommand("spMakeSystemCode", con, transaction))
                        {
                            makeSystemCodeCmd.CommandType = CommandType.StoredProcedure;
                            makeSystemCodeCmd.Parameters.AddWithValue("@TableName", "Sale");
                            makeSystemCodeCmd.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"));
                            makeSystemCodeCmd.Parameters.AddWithValue("@AddNumber", 1);
                            systemCode = makeSystemCodeCmd.ExecuteScalar()?.ToString();
                        }

                        int SaleId = int.Parse(systemCode.Split('%')[0]);
                        string SaleCode = systemCode.Split('%')[1];

                        using (SqlCommand masterCmd = new SqlCommand("INSERT INTO Sale (SaleId, SaleDate, SaleCode, CustomerId, EId, CompanyId, TotalSale, DeliveryCharge,DeliveryDate, ExtraCost, SaleDescription, AddedBy, AddedDate, AddedPc, UpdatedBy, UpdatedDate, UpdatedPc) VALUES (@SaleId, @SaleDate, @SaleCode, @CustomerId, @EId, @CompanyId, @TotalSale, @DeliveryCharge,@DeliveryDate, @ExtraCost, @SaleDescription, @AddedBy, @AddedDate, @AddedPc, @UpdatedBy, @UpdatedDate, @UpdatedPc);", con, transaction))
                        {
                            masterCmd.Parameters.AddWithValue("@SaleId", SaleId);
                            masterCmd.Parameters.AddWithValue("@SaleDate", DateTime.Now);
                            masterCmd.Parameters.AddWithValue("@SaleCode", SaleCode);
                            masterCmd.Parameters.AddWithValue("@CustomerId", sale.CustomerId);
                            masterCmd.Parameters.AddWithValue("@EId", sale.EId);
                            masterCmd.Parameters.AddWithValue("@CompanyId", sale.CompanyId ?? (object)DBNull.Value);
                            masterCmd.Parameters.AddWithValue("@TotalSale", sale.TotalSale);
                            masterCmd.Parameters.AddWithValue("@DeliveryCharge", sale.DeliveryCharge ?? (object)DBNull.Value);
                            masterCmd.Parameters.AddWithValue("@DeliveryDate", DateTime.Now);
                            masterCmd.Parameters.AddWithValue("@ExtraCost", sale.ExtraCost ?? (object)DBNull.Value);
                            masterCmd.Parameters.AddWithValue("@SaleDescription", sale.SaleDescription ?? (object)DBNull.Value);
                            masterCmd.Parameters.AddWithValue("@AddedBy", sale.AddedBy ?? (object)DBNull.Value);
                            masterCmd.Parameters.AddWithValue("@AddedDate", DateTime.Now);
                            masterCmd.Parameters.AddWithValue("@AddedPc", sale.AddedPc ?? (object)DBNull.Value);
                            masterCmd.Parameters.AddWithValue("@UpdatedBy", sale.UpdatedBy ?? (object)DBNull.Value);
                            masterCmd.Parameters.AddWithValue("@UpdatedDate", sale.UpdatedDate ?? (object)DBNull.Value);
                            masterCmd.Parameters.AddWithValue("@UpDatedPc", sale.UpDatedPc ?? (object)DBNull.Value);

                            masterCmd.ExecuteNonQuery();
                        }

                        SaleData.InsertSaleDetails(saleDetails, con, transaction, SaleId);

                        transaction.Commit();

                        return Ok(new { message = "Sales Data inserted successfully", SaleId });
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
        [HttpPut("UpdateSales")]
        public IActionResult SaleDataUpdate([FromForm] Sale sale, [FromForm] string data)
        {
            List<SaleDetail> saleDetails = null;

            try
            {
                saleDetails = JsonConvert.DeserializeObject<List<SaleDetail>>(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during JSON deserialization: {ex.Message}");
                return BadRequest(new { message = "Error during JSON deserialization" });
            }
            SqlConnection con = new SqlConnection(_connectionString);
            try
            {
                SqlCommand updatePurchaseCmd = new SqlCommand("UPDATE Sale SET SaleDate = @SaleDate, CustomerId = @CustomerId, EId = @EId, TotalSale = @TotalSale, DeliveryCharge = @DeliveryCharge, ExtraCost = @ExtraCost, SaleDescription = @SaleDescription, UpdatedBy = @UpdatedBy, UpdatedDate = @UpdatedDate, UpdatedPc = @UpdatedPc WHERE SaleId = @SaleId", con);

                updatePurchaseCmd.Parameters.AddWithValue("@SaleId", sale.SaleId);
                updatePurchaseCmd.Parameters.AddWithValue("@SaleDate", DateTime.Now);
                updatePurchaseCmd.Parameters.AddWithValue("@CustomerId", sale.CustomerId);
                updatePurchaseCmd.Parameters.AddWithValue("@EId", sale.EId);
                updatePurchaseCmd.Parameters.AddWithValue("@TotalSale", sale.TotalSale);
                updatePurchaseCmd.Parameters.AddWithValue("@DeliveryCharge", sale.DeliveryCharge ?? (object)DBNull.Value);
                updatePurchaseCmd.Parameters.AddWithValue("@ExtraCost", sale.ExtraCost ?? (object)DBNull.Value);
                updatePurchaseCmd.Parameters.AddWithValue("@SaleDescription", sale.SaleDescription ?? (object)DBNull.Value);
                updatePurchaseCmd.Parameters.AddWithValue("@UpdatedBy", sale.UpdatedBy ?? (object)DBNull.Value);
                updatePurchaseCmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                updatePurchaseCmd.Parameters.AddWithValue("@UpDatedPc", sale.UpDatedPc ?? (object)DBNull.Value);

                con.Open();
                updatePurchaseCmd.ExecuteNonQuery();
                con.Close();

                SaleData.UpdateSaleDetails(saleDetails, con, (int)sale.SaleId);

                return Ok(new { message = "Sales Data Updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpDelete("{SaleId}")]
        public IActionResult DeleteSale(int SaleId)
        {
            SqlConnection con = new SqlConnection(_connectionString);
            try
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Sale WHERE SaleId = @SaleId;", con);
                cmd.Parameters.AddWithValue("@SaleId", SaleId);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                SaleData.DeleteAllDetails(con, SaleId);
                return Ok(new { message = "Sales Data Deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest($"Could not delete {ex.Message}");
            }
        }
        [HttpGet("GetSaleData")]
        public IActionResult GetSaleData(int CompanyId)
        {
            List<SaleGet> saleList = new List<SaleGet>();

            SqlConnection con = new SqlConnection(_connectionString);

            try
            {
                SqlCommand cmd = new SqlCommand("GetActiveSale", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@CompanyId", CompanyId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    SaleGet master = new SaleGet();
                    master.SaleId = Convert.ToInt32(reader["SaleId"].ToString());
                    master.SaleDate = (DateTime)reader["SaleDate"];
                    master.SaleCode = reader["SaleCode"].ToString();
                    master.SaleDescription = reader["SaleDescription"].ToString();
                    master.CustomerName = reader["CustomerName"].ToString();
                    master.EmployeeName = reader["EmployeeName"].ToString();
                    master.TotalSale = (Decimal)reader["TotalSale"];

                    saleList.Add(master);
                }

                con.Close();
                return Ok(saleList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{SaleId}")]
        public IActionResult GetOneSaleData(int SaleId)
        {
            SaleGetById master = null;

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("GetAllSales", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SaleId", SaleId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                master = new SaleGetById
                                {
                                    SaleId = Convert.ToInt32(reader["SaleId"]),
                                    SaleDate = (DateTime)reader["SaleDate"],
                                    SaleCode = reader["SaleCode"].ToString(),
                                    SaleDescription = reader["SaleDescription"].ToString(),
                                    CustomerName = reader["CustomerName"].ToString(),
                                    CustomerId = Convert.ToInt32(reader["CustomerId"]),
                                    EId = Convert.ToInt32(reader["EId"]),
                                    EmployeeName = reader["EmployeeName"].ToString(),
                                    TotalSale = Convert.ToInt32(reader["TotalSale"]),
                                    DeliveryCharge = Convert.IsDBNull(reader["DeliveryCharge"]) ? 0 : (decimal)reader["DeliveryCharge"],
                                    ExtraCost = Convert.IsDBNull(reader["ExtraCost"]) ? 0 : (decimal)reader["ExtraCost"],
                                    DeliveryDate = (DateTime)reader["DeliveryDate"],
                                };
                            }
                        }
                    }

                    if (master != null)
                    {
                        List<SaleDetailsById> saleDetailsList = new List<SaleDetailsById>();
                        SaleData.GetAllDetails(con, SaleId, saleDetailsList);
                        master.SaleDetails = saleDetailsList;
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
    }
}
