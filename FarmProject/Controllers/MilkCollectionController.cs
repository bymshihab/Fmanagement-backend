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
    public class MilkCollectionController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public MilkCollectionController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        MilkCollectionDetailData MilkCollectionData = new MilkCollectionDetailData();

        [HttpPost]
        public IActionResult InsertMilkCollection([FromForm] MilkCollection milkCollection, [FromForm] string data)
        {
            List<MilkCollectionDetail> milkCollectionDetails = null;

            try
            {
                milkCollectionDetails = JsonConvert.DeserializeObject<List<MilkCollectionDetail>>(data);
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
                        int lastMilkCollectionId = 0;
                        SqlCommand getLastMilkCollectionIdCmd = new SqlCommand("SELECT MAX(MilkCollectionId) FROM MilkCollections;", con, transaction);
                        object result = getLastMilkCollectionIdCmd.ExecuteScalar();

                        if (result != DBNull.Value && result != null)
                        {
                            lastMilkCollectionId = Convert.ToInt32(result);
                        }
                        milkCollection.MilkCollectionId = lastMilkCollectionId + 1;

                        string systemCode = string.Empty;
                        using (SqlCommand makeSystemCodeCmd = new SqlCommand("spMakeSystemCode", con, transaction))
                        {
                            makeSystemCodeCmd.CommandType = CommandType.StoredProcedure;
                            makeSystemCodeCmd.Parameters.AddWithValue("@TableName", "MilkCollections");
                            makeSystemCodeCmd.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"));
                            makeSystemCodeCmd.Parameters.AddWithValue("@AddNumber", 1);
                            systemCode = makeSystemCodeCmd.ExecuteScalar()?.ToString();
                        }

                        int MilkCollectionId = int.Parse(systemCode.Split('%')[0]);
                        string MilkCollectionCode = systemCode.Split('%')[1];

                        using (SqlCommand masterCmd = new SqlCommand("INSERT INTO MilkCollections (MilkCollectionId, MilkCollectionCode, MilkCollectionDate, EId, MilkCollectionDesc, CompanyId, AddedBy, AddedDate, AddedPc, UpdatedBy, UpdatedDate, UpDatedPc) VALUES (@MilkCollectionId, @MilkCollectionCode, @MilkCollectionDate, @EId, @MilkCollectionDesc, @CompanyId, @AddedBy, @AddedDate, @AddedPc, @UpdatedBy, @UpdatedDate, @UpDatedPc);", con, transaction))
                        {
                            masterCmd.Parameters.AddWithValue("@MilkCollectionId", MilkCollectionId);
                            masterCmd.Parameters.AddWithValue("@MilkCollectionCode", MilkCollectionCode);
                            masterCmd.Parameters.AddWithValue("@MilkCollectionDate", DateTime.Now);
                            masterCmd.Parameters.AddWithValue("@EId", milkCollection.EId);
                            masterCmd.Parameters.AddWithValue("@MilkCollectionDesc", milkCollection.MilkCollectionDesc ?? (object)DBNull.Value);
                            masterCmd.Parameters.AddWithValue("@CompanyId", milkCollection.CompanyId);
                            masterCmd.Parameters.AddWithValue("@AddedBy", milkCollection.AddedBy ?? (object)DBNull.Value);
                            masterCmd.Parameters.AddWithValue("@AddedDate", DateTime.Now);
                            masterCmd.Parameters.AddWithValue("@AddedPc", milkCollection.AddedPc ?? (object)DBNull.Value);
                            masterCmd.Parameters.AddWithValue("@UpdatedBy", milkCollection.UpdatedBy ?? (object)DBNull.Value);
                            masterCmd.Parameters.AddWithValue("@UpdatedDate", milkCollection.UpdatedDate ?? (object)DBNull.Value);
                            masterCmd.Parameters.AddWithValue("@UpDatedPc", milkCollection.UpDatedPc ?? (object)DBNull.Value);

                            masterCmd.ExecuteNonQuery();
                        }

                        MilkCollectionData.InsertMilkCollectionDetails(milkCollectionDetails, con, transaction, MilkCollectionId);

                        transaction.Commit();

                        return Ok(new { message = "MilkCollection Data inserted successfully", MilkCollectionId });
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
        [HttpPut("UpdateMilkCollection")]
        public IActionResult MilkCollectionUpdate([FromForm] MilkCollection milkCollection, [FromForm] string data)
        {
            List<MilkCollectionDetail> milkCollectionDetails = null;

            try
            {
                milkCollectionDetails = JsonConvert.DeserializeObject<List<MilkCollectionDetail>>(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during JSON deserialization: {ex.Message}");
                return BadRequest(new { message = "Error during JSON deserialization" });
            }
            SqlConnection con = new SqlConnection(_connectionString);
            try
            {

                SqlCommand updateCmd = new SqlCommand("UPDATE MilkCollections SET MilkCollectionDate = @MilkCollectionDate, EId = @EId, MilkCollectionDesc = @MilkCollectionDesc, UpdatedBy = @UpdatedBy, UpdatedDate = @UpdatedDate, UpdatedPc = @UpdatedPc WHERE MilkCollectionId = @MilkCollectionId", con);

                updateCmd.Parameters.AddWithValue("@MilkCollectionId", milkCollection.MilkCollectionId);
                updateCmd.Parameters.AddWithValue("@MilkCollectionDate", DateTime.Now);
                updateCmd.Parameters.AddWithValue("@EId", milkCollection.EId);
                updateCmd.Parameters.AddWithValue("@MilkCollectionDesc", milkCollection.MilkCollectionDesc);

                updateCmd.Parameters.AddWithValue("@UpdatedBy", milkCollection.UpdatedBy ?? (object)DBNull.Value);
                updateCmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                updateCmd.Parameters.AddWithValue("@UpDatedPc", milkCollection.UpDatedPc ?? (object)DBNull.Value);

                con.Open();
                updateCmd.ExecuteNonQuery();
                con.Close();

                MilkCollectionData.UpdateMilkCollection(milkCollectionDetails, con, (int)milkCollection.MilkCollectionId);

                return Ok(new { message = "MilkCollection Data Updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpDelete("{GrainMasterId}")]
        public IActionResult DeleteMilkCollection(int MilkCollectionId)
        {
            SqlConnection con = new SqlConnection(_connectionString);
            try
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM MilkCollections WHERE MilkCollectionId = @MilkCollectionId;", con);
                cmd.Parameters.AddWithValue("@MilkCollectionId", MilkCollectionId);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                MilkCollectionData.DeleteAllDetails(con, MilkCollectionId);
                return Ok(new { message = "MilkCollection Data Deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest($"Could not delete {ex.Message}");
            }
        }
        [HttpGet("MilkCollectionData")]
        public IActionResult GetMilkCollectionData(int CompanyId)
        {
            List<MilkCollectionGet> grainList = new List<MilkCollectionGet>();

            SqlConnection con = new SqlConnection(_connectionString);

            try
            {
                SqlCommand cmd = new SqlCommand("GetActiveMilkCollections", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@CompanyId", CompanyId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    MilkCollectionGet master = new MilkCollectionGet();
                    master.MilkCollectionId = Convert.ToInt32(reader["MilkCollectionId"].ToString());
                    master.MilkCollectionDate = (DateTime)reader["MilkCollectionDate"];
                    master.MilkCollectionCode = reader["MilkCollectionCode"].ToString();
                    master.EmployeeName = reader["EmployeeName"].ToString();
                    master.MilkCollectionDesc = reader["MilkCollectionDesc"].ToString();
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


        [HttpGet("{MilkCollectionId}")]
        public IActionResult GetOneMilkCollectionData(int MilkCollectionId)
        {
            MilkCollectionById master = null;

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("GetAllMilkCollections", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MilkCollectionId", MilkCollectionId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                master = new MilkCollectionById
                                {
                                    MilkCollectionId = Convert.ToInt32(reader["MilkCollectionId"]),
                                    MilkCollectionDate = (DateTime)reader["MilkCollectionDate"],
                                    MilkCollectionCode = reader["MilkCollectionCode"].ToString(),
                                    EmployeeName = reader["EmployeeName"].ToString(),
                                    EId = Convert.ToInt32(reader["EId"]),
                                    MilkCollectionDesc = reader["MilkCollectionDesc"].ToString(),

                                };
                            }
                        }
                    }

                    if (master != null)
                    {
                        List<MilkCollectionDetailById> DetailsList = new List<MilkCollectionDetailById>();
                        MilkCollectionData.GetAllDetails(con, MilkCollectionId, DetailsList);
                        master.MilkCollectionDetails = DetailsList;
                    }
                }

                return master != null ? Ok(master) : NotFound();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetOneMilkCollectionData: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                return StatusCode(500, new { message = "An error occurred while processing the request" });
            }
        }
    }
}
