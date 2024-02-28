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
    public class GrainFeedMasterController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public GrainFeedMasterController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        GrainDetailData GrainData = new GrainDetailData();

        [HttpPost]
        public IActionResult InsertGrain([FromForm] GrainFeedMaster grainFeedMaster, [FromForm] string data)
        {
            List<GrainFeedChart> grainFeedCharts = null;

            try
            {
                grainFeedCharts = JsonConvert.DeserializeObject<List<GrainFeedChart>>(data);
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
                        int lastGrainMasterId = 0;
                        SqlCommand getLastGrainMasterIdCmd = new SqlCommand("SELECT MAX(GrainMasterId) FROM GrainFeedMasters;", con, transaction);
                        object result = getLastGrainMasterIdCmd.ExecuteScalar();

                        if (result != DBNull.Value && result != null)
                        {
                            lastGrainMasterId = Convert.ToInt32(result);
                        }
                        grainFeedMaster.GrainMasterId = lastGrainMasterId + 1;

                        string systemCode = string.Empty;
                        using (SqlCommand makeSystemCodeCmd = new SqlCommand("spMakeSystemCode", con, transaction))
                        {
                            makeSystemCodeCmd.CommandType = CommandType.StoredProcedure;
                            makeSystemCodeCmd.Parameters.AddWithValue("@TableName", "GrainFeedMasters");
                            makeSystemCodeCmd.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"));
                            makeSystemCodeCmd.Parameters.AddWithValue("@AddNumber", 1);
                            systemCode = makeSystemCodeCmd.ExecuteScalar()?.ToString();
                        }

                        int GrainMasterId = int.Parse(systemCode.Split('%')[0]);
                        string GrainCode = systemCode.Split('%')[1];

                        using (SqlCommand masterCmd = new SqlCommand("INSERT INTO GrainFeedMasters (GrainMasterId, MakingDate, GrainCode, ProductId, AnimalId, TotalQty, TotalPrice, CompanyId, AddedBy, AddedDate, AddedPc, UpdatedBy, UpdatedDate, UpdatedPc) VALUES (@GrainMasterId, @MakingDate, @GrainCode, @ProductId, @AnimalId, @TotalQty, @TotalPrice, @CompanyId, @AddedBy, @AddedDate, @AddedPc, @UpdatedBy, @UpdatedDate, @UpdatedPc);", con, transaction))
                        {
                            masterCmd.Parameters.AddWithValue("@GrainMasterId", GrainMasterId);
                            masterCmd.Parameters.AddWithValue("@MakingDate", DateTime.Now);
                            masterCmd.Parameters.AddWithValue("@GrainCode", GrainCode);
                            masterCmd.Parameters.AddWithValue("@ProductId", grainFeedMaster.ProductId);
                            masterCmd.Parameters.AddWithValue("@AnimalId", grainFeedMaster.AnimalId);
                            masterCmd.Parameters.AddWithValue("@TotalQty", grainFeedMaster.TotalQty);
                            masterCmd.Parameters.AddWithValue("@TotalPrice", grainFeedMaster.TotalPrice);
                            masterCmd.Parameters.AddWithValue("@CompanyId", grainFeedMaster.CompanyId ?? (object)DBNull.Value);
                            masterCmd.Parameters.AddWithValue("@AddedBy", grainFeedMaster.AddedBy ?? (object)DBNull.Value);
                            masterCmd.Parameters.AddWithValue("@AddedDate", DateTime.Now);
                            masterCmd.Parameters.AddWithValue("@AddedPc", grainFeedMaster.AddedPc ?? (object)DBNull.Value);
                            masterCmd.Parameters.AddWithValue("@UpdatedBy", grainFeedMaster.UpdatedBy ?? (object)DBNull.Value);
                            masterCmd.Parameters.AddWithValue("@UpdatedDate", grainFeedMaster.UpdatedDate ?? (object)DBNull.Value);
                            masterCmd.Parameters.AddWithValue("@UpDatedPc", grainFeedMaster.UpDatedPc ?? (object)DBNull.Value);

                            masterCmd.ExecuteNonQuery();
                        }

                        GrainData.InsertGrainDetails(grainFeedCharts, con, transaction, GrainMasterId);

                        transaction.Commit();

                        return Ok(new { message = "Grain Data inserted successfully", GrainMasterId });
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

        [HttpPut("UpdateGrain")]
        public IActionResult GrainUpdate(int GrainMasterId, [FromForm] GrainFeedMaster grainFeedMaster, [FromForm] string data)
        {
            List<GrainFeedChart> grainFeedCharts = null;

            try
            {
                grainFeedCharts = JsonConvert.DeserializeObject<List<GrainFeedChart>>(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during JSON deserialization: {ex.Message}");
                return BadRequest(new { message = "Error during JSON deserialization" });
            }
            SqlConnection con = new SqlConnection(_connectionString);
            try
            {
                //SqlCommand updateCmd = new SqlCommand("UPDATE GrainFeedMasters SET MakingDate = @MakingDate, ProductId = @ProductId, AnimalId = @AnimalId, TotalQty = @TotalQty, TotalPrice = @TotalPrice UpdatedBy = @UpdatedBy, UpdatedDate = @UpdatedDate, UpdatedPc = @UpdatedPc WHERE GrainMasterId = @GrainMasterId", con);
                SqlCommand updateCmd = new SqlCommand("UPDATE GrainFeedMasters SET MakingDate = @MakingDate, ProductId = @ProductId, AnimalId = @AnimalId, TotalQty = @TotalQty, TotalPrice = @TotalPrice, UpdatedBy = @UpdatedBy, UpdatedDate = @UpdatedDate, UpdatedPc = @UpdatedPc WHERE GrainMasterId = @GrainMasterId", con);

                updateCmd.Parameters.AddWithValue("@GrainMasterId", GrainMasterId);
                updateCmd.Parameters.AddWithValue("@MakingDate", DateTime.Now);
                updateCmd.Parameters.AddWithValue("@ProductId", grainFeedMaster.ProductId);
                updateCmd.Parameters.AddWithValue("@AnimalId", grainFeedMaster.AnimalId);
                updateCmd.Parameters.AddWithValue("@TotalQty", grainFeedMaster.TotalQty);
                updateCmd.Parameters.AddWithValue("@TotalPrice", grainFeedMaster.TotalPrice);
                updateCmd.Parameters.AddWithValue("@UpdatedBy", grainFeedMaster.UpdatedBy ?? (object)DBNull.Value);
                updateCmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                updateCmd.Parameters.AddWithValue("@UpDatedPc", grainFeedMaster.UpDatedPc ?? (object)DBNull.Value);

                con.Open();
                updateCmd.ExecuteNonQuery();
                con.Close();

                GrainData.UpdateGrainDetails(grainFeedCharts, con, GrainMasterId);

                return Ok(new { message = "Grain Data Updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpDelete("{GrainMasterId}")]
        public IActionResult DeleteGrain(int GrainMasterId)
        {
            SqlConnection con = new SqlConnection(_connectionString);
            try
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM GrainFeedMasters WHERE GrainMasterId = @GrainMasterId;", con);
                cmd.Parameters.AddWithValue("@GrainMasterId", GrainMasterId);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                GrainData.DeleteAllDetails(con, GrainMasterId);
                return Ok(new { message = "Grain Data Deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest($"Could not delete {ex.Message}");
            }
        }

        [HttpGet("GetFeedingData")]
        public IActionResult GetFeedingData(int CompanyId)
        {
            List<GrainGet> grainList = new List<GrainGet>();

            SqlConnection con = new SqlConnection(_connectionString);

            try
            {
                SqlCommand cmd = new SqlCommand("GetActiveGrainFeedMasters", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@CompanyId", CompanyId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    GrainGet master = new GrainGet();
                    master.GrainMasterId = Convert.ToInt32(reader["GrainMasterId"].ToString());
                    master.MakingDate = (DateTime)reader["MakingDate"];
                    master.GrainCode = reader["GrainCode"].ToString();
                    master.AnimalName = reader["AnimalName"].ToString();
                    master.ProductName = reader["ProductName"].ToString();
                    master.TotalQty = (decimal)reader["TotalQty"];
                    master.TotalPrice = (decimal)reader["TotalPrice"];
                    grainList.Add(master);
                }

                con.Close();
                return Ok(grainList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("{GrainMasterId}")]
        public IActionResult GetOnePurchaseData(int GrainMasterId)
        {
            GrainGetById master = null;

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("GetAllGrainFeedMasters", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@GrainMasterId", GrainMasterId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                master = new GrainGetById
                                {
                                    GrainMasterId = Convert.ToInt32(reader["GrainMasterId"]),
                                    MakingDate = (DateTime)reader["MakingDate"],
                                    GrainCode = reader["GrainCode"].ToString(),
                                    ProductName = reader["ProductName"].ToString(),
                                    ProductId = Convert.ToInt32(reader["ProductId"]),
                                    AnimalId = Convert.ToInt32(reader["AnimalId"]),
                                    AnimalName = reader["AnimalName"].ToString(),
                                    TotalQty = (decimal)reader["TotalQty"],
                                    TotalPrice =(decimal)reader["TotalPrice"],
                                };
                            }
                        }
                    }

                    if (master != null)
                    {
                        List<GrainDetailsById> gainDetailsList = new List<GrainDetailsById>();
                        GrainData.GetAllDetails(con, GrainMasterId, gainDetailsList);
                        master.GrainDetails = gainDetailsList;
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
