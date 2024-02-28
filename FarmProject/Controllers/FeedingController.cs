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
    public class FeedingController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public FeedingController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        FeedingDetailData feedingData = new FeedingDetailData();

        [HttpPost]
        public IActionResult InsertFeeding([FromForm] Feeding feeding, [FromForm] string data)
        {
            List<FeedingDetail> feedingDetails = null;

            try
            {
                feedingDetails = JsonConvert.DeserializeObject<List<FeedingDetail>>(data);
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
                        int lastFeedId = 0;
                        SqlCommand getLastFeedIdCmd = new SqlCommand("SELECT MAX(FeedId) FROM Feedings;", con, transaction);
                        object result = getLastFeedIdCmd.ExecuteScalar();

                        if (result != DBNull.Value && result != null)
                        {
                            lastFeedId = Convert.ToInt32(result);
                        }
                        feeding.FeedId = lastFeedId + 1;

                        string systemCode = string.Empty;
                        using (SqlCommand makeSystemCodeCmd = new SqlCommand("spMakeSystemCode", con, transaction))
                        {
                            makeSystemCodeCmd.CommandType = CommandType.StoredProcedure;
                            makeSystemCodeCmd.Parameters.AddWithValue("@TableName", "Feedings");
                            makeSystemCodeCmd.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"));
                            makeSystemCodeCmd.Parameters.AddWithValue("@AddNumber", 1);
                            systemCode = makeSystemCodeCmd.ExecuteScalar()?.ToString();
                        }

                        int FeedId = int.Parse(systemCode.Split('%')[0]);
                        string FeedingCode = systemCode.Split('%')[1];

                        using (SqlCommand masterCmd = new SqlCommand("INSERT INTO Feedings (FeedId, FeedIngDate, FeedingCode, EId, AnimalId, TotalQTY,CompanyId, TotalCost, AddedBy, AddedDate, AddedPc, UpdatedBy, UpdatedDate, UpdatedPc) VALUES (@FeedId, @FeedIngDate, @FeedingCode, @EId, @AnimalId, @TotalQTY,@CompanyId, @TotalCost, @AddedBy, @AddedDate, @AddedPc, @UpdatedBy, @UpdatedDate, @UpdatedPc);", con, transaction))
                        {
                            masterCmd.Parameters.AddWithValue("@FeedId", FeedId);
                            masterCmd.Parameters.AddWithValue("@FeedIngDate", DateTime.Now);
                            masterCmd.Parameters.AddWithValue("@FeedingCode", FeedingCode);
                            masterCmd.Parameters.AddWithValue("@EId", feeding.EId);
                            masterCmd.Parameters.AddWithValue("@AnimalId", feeding.AnimalId);
                            masterCmd.Parameters.AddWithValue("@TotalQTY", feeding.TotalQTY ?? (object)DBNull.Value);
                            masterCmd.Parameters.AddWithValue("@CompanyId", feeding.CompanyId ?? (object)DBNull.Value);
                            masterCmd.Parameters.AddWithValue("@TotalCost", feeding.TotalCost);
                            masterCmd.Parameters.AddWithValue("@AddedBy", feeding.AddedBy ?? (object)DBNull.Value);
                            masterCmd.Parameters.AddWithValue("@AddedDate", DateTime.Now);
                            masterCmd.Parameters.AddWithValue("@AddedPc", feeding.AddedPc ?? (object)DBNull.Value);
                            masterCmd.Parameters.AddWithValue("@UpdatedBy", feeding.UpdatedBy ?? (object)DBNull.Value);
                            masterCmd.Parameters.AddWithValue("@UpdatedDate", feeding.UpdatedDate ?? (object)DBNull.Value);
                            masterCmd.Parameters.AddWithValue("@UpDatedPc", feeding.UpDatedPc ?? (object)DBNull.Value);

                            masterCmd.ExecuteNonQuery();
                        }

                        feedingData.InsertFeedingDetails(feedingDetails, con, transaction, FeedId);

                        transaction.Commit();

                        return Ok(new { message = "Feeding Data inserted successfully", FeedId });
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

        [HttpPut("UpdateFeeding")]
        public IActionResult FeedingDataUpdate([FromForm] Feeding feeding, [FromForm] string data)
        {
            List<FeedingDetail> feedingDetails = null;

            try
            {
                feedingDetails = JsonConvert.DeserializeObject<List<FeedingDetail>>(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during JSON deserialization: {ex.Message}");
                return BadRequest(new { message = "Error during JSON deserialization" });
            }
            SqlConnection con = new SqlConnection(_connectionString);
            try
            {
                SqlCommand updateCmd = new SqlCommand("UPDATE Feedings SET FeedIngDate = @FeedIngDate, EId = @EId, AnimalId = @AnimalId, TotalQTY = @TotalQTY, TotalCost=@TotalCost, UpdatedBy = @UpdatedBy, UpdatedDate = @UpdatedDate, UpdatedPc = @UpdatedPc WHERE FeedId = @FeedId", con);

                updateCmd.Parameters.AddWithValue("@FeedId", feeding.FeedId);
                updateCmd.Parameters.AddWithValue("@FeedIngDate", DateTime.Now);
                updateCmd.Parameters.AddWithValue("@EId", feeding.EId);
                updateCmd.Parameters.AddWithValue("@AnimalId", feeding.AnimalId);
                updateCmd.Parameters.AddWithValue("@TotalQTY", feeding.TotalQTY ?? (object)DBNull.Value);
                updateCmd.Parameters.AddWithValue("@TotalCost", feeding.TotalCost);
                updateCmd.Parameters.AddWithValue("@UpdatedBy", feeding.UpdatedBy ?? (object)DBNull.Value);
                updateCmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                updateCmd.Parameters.AddWithValue("@UpDatedPc", feeding.UpDatedPc ?? (object)DBNull.Value);

                con.Open();
                updateCmd.ExecuteNonQuery();
                con.Close();

                feedingData.UpdateFeedingDetails(feedingDetails, con, (int)feeding.FeedId);

                return Ok(new { message = "Feeding Data Updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpDelete("{FeedId}")]
        public IActionResult DeleteFeeding(int FeedId)
        {
            SqlConnection con = new SqlConnection(_connectionString);
            try
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Feedings WHERE FeedId = @FeedId;", con);
                cmd.Parameters.AddWithValue("@FeedId", FeedId);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                feedingData.DeleteAllDetails(con, FeedId);
                return Ok(new { message = "Feedings Data Deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest($"Could not delete {ex.Message}");
            }
        }
        [HttpGet("GetFeedingData")]
        public IActionResult GetFeedingData(int CompanyId)
        {
            List<FeedingGet> feedingList = new List<FeedingGet>();

            SqlConnection con = new SqlConnection(_connectionString);

            try
            {
                SqlCommand cmd = new SqlCommand("GetActiveFeedings", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@CompanyId", CompanyId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    FeedingGet master = new FeedingGet();
                    master.FeedId = Convert.ToInt32(reader["FeedId"].ToString());
                    master.FeedIngDate = (DateTime)reader["FeedIngDate"];
                    master.FeedingCode = reader["FeedingCode"].ToString();
                    master.AnimalName = reader["AnimalName"].ToString();
                    master.EmployeeName = reader["EmployeeName"].ToString();

                    master.TotalQTY = reader["TotalQTY"] != DBNull.Value ? (Decimal)reader["TotalQTY"] : 0m;
                    master.TotalCost = reader["TotalCost"] != DBNull.Value ? (Decimal)reader["TotalCost"] : 0m;

                    feedingList.Add(master);
                }

                con.Close();
                return Ok(feedingList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{FeedId}")]
        public IActionResult GetOneFeedingData(int FeedId)
        {
            FeedingGetById master = null;

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("GetAllFeedings", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FeedId", FeedId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Log all column names
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                Console.WriteLine($"Column Name: {reader.GetName(i)}");
                            }

                            while (reader.Read())
                            {
                                master = new FeedingGetById
                                {
                                    FeedId = Convert.ToInt32(reader["FeedId"]),
                                    FeedIngDate = (DateTime)reader["FeedIngDate"],
                                    FeedingCode = reader["FeedingCode"].ToString(),
                                    AnimalName = reader["AnimalName"].ToString(),
                                    AnimalId = Convert.ToInt32(reader["AnimalId"]),
                                    EId = Convert.ToInt32(reader["EId"]),
                                    EmployeeName = reader["EmployeeName"].ToString(),
                                    TotalQTY = (decimal)reader["TotalQTY"],
                                    TotalCost = (decimal)reader["TotalCost"],
                                };
                            }
                        }
                    }

                    if (master != null)
                    {
                        List<FeedingDetailsGetById> feedingDetailsList = new List<FeedingDetailsGetById>();
                        feedingData.GetAllDetails(con, FeedId, feedingDetailsList);
                        master.FeedingDetails = feedingDetailsList;
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
