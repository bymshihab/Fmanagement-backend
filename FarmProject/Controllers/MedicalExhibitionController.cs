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
    public class MedicalExhibitionController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public MedicalExhibitionController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        MedicalExhibitionData medicalExhibitionData = new MedicalExhibitionData();

        [HttpPost]
        public IActionResult InsertMedicalExhibition([FromForm] MedicalExhibition medicalExhibition, [FromForm] string? data)
        {
            MedicalExhibitionList medicalExhibitionLists = JsonConvert.DeserializeObject<MedicalExhibitionList>(data);

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    try
                    {
                        int lastMedicalId = 0;
                        SqlCommand getLastMedicalIdCmd = new SqlCommand("SELECT MAX(MedicalId) FROM MedicineExhibitions;", con, transaction);
                        object result = getLastMedicalIdCmd.ExecuteScalar();

                        if (result != DBNull.Value && result != null)
                        {
                            lastMedicalId = Convert.ToInt32(result);
                        }
                        medicalExhibition.MedicalId = lastMedicalId + 1;

                        string systemCode = string.Empty;
                        using (SqlCommand makeSystemCodeCmd = new SqlCommand("spMakeSystemCode", con, transaction))
                        {
                            makeSystemCodeCmd.CommandType = CommandType.StoredProcedure;
                            makeSystemCodeCmd.Parameters.AddWithValue("@TableName", "MedicineExhibitions");
                            makeSystemCodeCmd.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"));
                            makeSystemCodeCmd.Parameters.AddWithValue("@AddNumber", 1);
                            systemCode = makeSystemCodeCmd.ExecuteScalar()?.ToString();
                        }

                        int MedicalId = int.Parse(systemCode.Split('%')[0]);
                        string MedicalCode = systemCode.Split('%')[1];

                        using (SqlCommand masterCmd = new SqlCommand("INSERT INTO MedicineExhibitions (MedicalId, MedicalCode, ExhibitionDate, AnimalId, CompanyId,OutsiderId, EId, IsNew, IsSick, AddedBy, AddedDate, AddedPc, UpdatedBy, UpdatedDate, UpDatedPc) VALUES (@MedicalId, @MedicalCode, @ExhibitionDate, @AnimalId, @CompanyId,@OutsiderId, @EId, @IsNew, @IsSick, @AddedBy, @AddedDate, @AddedPc, @UpdatedBy, @UpdatedDate, @UpDatedPc);", con, transaction))
                        {
                            masterCmd.Parameters.AddWithValue("@MedicalId", MedicalId);
                            masterCmd.Parameters.AddWithValue("@MedicalCode", MedicalCode);
                            masterCmd.Parameters.AddWithValue("@ExhibitionDate", DateTime.Now);
                            masterCmd.Parameters.AddWithValue("@AnimalId", medicalExhibition.AnimalId);
                            masterCmd.Parameters.AddWithValue("@CompanyId", medicalExhibition.CompanyId);
                            masterCmd.Parameters.AddWithValue("@OutsiderId", medicalExhibition.OutsiderId);
                            masterCmd.Parameters.AddWithValue("@EId", medicalExhibition.EId);
                            masterCmd.Parameters.AddWithValue("@IsNew", medicalExhibition.IsNew);
                            masterCmd.Parameters.AddWithValue("@IsSick", medicalExhibition.IsSick);

                            masterCmd.Parameters.AddWithValue("@AddedBy", medicalExhibition.AddedBy ?? (object)DBNull.Value);
                            masterCmd.Parameters.AddWithValue("@AddedDate", DateTime.Now);
                            masterCmd.Parameters.AddWithValue("@AddedPc", medicalExhibition.AddedPc ?? (object)DBNull.Value);
                            masterCmd.Parameters.AddWithValue("@UpdatedBy", medicalExhibition.UpdatedBy ?? (object)DBNull.Value);
                            masterCmd.Parameters.AddWithValue("@UpdatedDate", medicalExhibition.UpdatedDate ?? (object)DBNull.Value);
                            masterCmd.Parameters.AddWithValue("@UpDatedPc", medicalExhibition.UpDatedPc ?? (object)DBNull.Value);

                            masterCmd.ExecuteNonQuery();
                        }

                        medicalExhibitionData.InsertMedicineDetails(medicalExhibitionLists.medicines, con, transaction, MedicalId);
                        medicalExhibitionData.InsertQuarantaineDetails(medicalExhibitionLists.quarantaines, con, transaction, MedicalId);


                        transaction.Commit();

                        return Ok(new { message = "MedicalExhibition Data inserted successfully", MedicalId });
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
        public IActionResult MilkCollectionUpdate(int MedicalId, [FromForm] MedicalExhibition medicalExhibition, [FromForm] string data)
        {
            MedicalExhibitionList medicalExhibitionLists = JsonConvert.DeserializeObject<MedicalExhibitionList>(data);

            SqlConnection con = new SqlConnection(_connectionString);
            try
            {

                SqlCommand updateCmd = new SqlCommand("UPDATE MedicineExhibitions SET ExhibitionDate = @ExhibitionDate, EId = @EId, AnimalId = @AnimalId, OutsiderId = @OutsiderId, IsNew = @IsNew, IsSick = @IsSick, UpdatedBy = @UpdatedBy, UpdatedDate = @UpdatedDate, UpdatedPc = @UpdatedPc WHERE MedicalId = @MedicalId", con);

                updateCmd.Parameters.AddWithValue("@MedicalId", MedicalId);
                updateCmd.Parameters.AddWithValue("@ExhibitionDate", DateTime.Now);
                updateCmd.Parameters.AddWithValue("@EId", medicalExhibition.EId);
                updateCmd.Parameters.AddWithValue("@AnimalId", medicalExhibition.AnimalId);
                updateCmd.Parameters.AddWithValue("@OutsiderId", medicalExhibition.OutsiderId);
                updateCmd.Parameters.AddWithValue("@IsNew", medicalExhibition.IsNew);
                updateCmd.Parameters.AddWithValue("@IsSick", medicalExhibition.IsSick);
                updateCmd.Parameters.AddWithValue("@UpdatedBy", medicalExhibition.UpdatedBy ?? (object)DBNull.Value);
                updateCmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                updateCmd.Parameters.AddWithValue("@UpDatedPc", medicalExhibition.UpDatedPc ?? (object)DBNull.Value);

                con.Open();
                updateCmd.ExecuteNonQuery();
                con.Close();

                medicalExhibitionData.UpdateMedicineDetails(medicalExhibitionLists.medicines, con, MedicalId);
                medicalExhibitionData.UpdateQuarantaineDetails(medicalExhibitionLists.quarantaines, con, MedicalId);

                return Ok(new { message = "MedicalExhibition Data Updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpDelete("{MedicalId}")]
        public IActionResult DeleteMilkCollection(int MedicalId)
        {
            SqlConnection con = new SqlConnection(_connectionString);
            try
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM MedicineExhibitions WHERE MedicalId = @MedicalId;", con);
                cmd.Parameters.AddWithValue("@MedicalId", MedicalId);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                medicalExhibitionData.DeleteAllMedicines(con, MedicalId);
                medicalExhibitionData.DeleteAllQuarantaines(con, MedicalId);
                return Ok(new { message = "MedicalExhibition Data Deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest($"Could not delete {ex.Message}");
            }
        }
        [HttpGet("MedicalExhibitionData")]
        public IActionResult GetMedicalExhibitionData(int CompanyId)
        {
            List<MedicalExhibitionGet> MedicalExhibitionList = new List<MedicalExhibitionGet>();

            SqlConnection con = new SqlConnection(_connectionString);

            try
            {
                SqlCommand cmd = new SqlCommand("GetAllMedicineExhibitions", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@CompanyId", CompanyId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    MedicalExhibitionGet master = new MedicalExhibitionGet();
                    master.MedicalId = Convert.ToInt32(reader["MedicalId"].ToString());
                    master.ExhibitionDate = (DateTime)reader["ExhibitionDate"];
                    master.MedicalCode = reader["MedicalCode"].ToString();
                    master.AnimalName = reader["AnimalName"].ToString();
                    master.OutsiderName = reader["OutsiderName"].ToString();
                    master.EmployeeName = reader["EmployeeName"].ToString();

                    MedicalExhibitionList.Add(master);
                }

                con.Close();
                return Ok(MedicalExhibitionList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{MedicalId}")]
        public IActionResult GetOneMedicalExhibitionData(int MedicalId)
        {
            MedicalExhibitionById master = null;

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("GetMedicineExhibitionsById", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MedicalId", MedicalId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                master = new MedicalExhibitionById
                                {
                                    MedicalId = Convert.ToInt32(reader["MedicalId"]),
                                    ExhibitionDate = (DateTime)reader["ExhibitionDate"],
                                    MedicalCode = reader["MedicalCode"].ToString(),
                                    EmployeeName = reader["EmployeeName"].ToString(),
                                    EId = Convert.ToInt32(reader["EId"]),
                                    AnimalName = reader["AnimalName"].ToString(),
                                    AnimalId = Convert.ToInt32(reader["AnimalId"]),
                                    OutsiderName = reader["OutsiderName"].ToString(),
                                    OutsiderId = Convert.ToInt32(reader["OutsiderId"]),
                                    IsNew = reader["IsNew"] != DBNull.Value ? Convert.ToBoolean(reader["IsNew"]) : false,
                                    IsSick = reader["IsSick"] != DBNull.Value ? Convert.ToBoolean(reader["IsSick"]) : false,

                                };
                            }
                        }
                    }

                    if (master != null)
                    {
                        List<MedicineById> MedicineList = new List<MedicineById>();
                        medicalExhibitionData.GetAllMedicineDetails(con, MedicalId, MedicineList);
                        master.MedicineDetailsById = MedicineList;
                        List<QuarantaineById> QuarantaineList = new List<QuarantaineById>();
                        medicalExhibitionData.GetAllQuarantaineDetails(con, MedicalId, QuarantaineList);
                        master.QuarantaineDetailsById = QuarantaineList;
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
