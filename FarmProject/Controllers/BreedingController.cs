using FarmProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;

namespace FarmProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BreedingController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public BreedingController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }
        [HttpPost("CreateBreeding")]
        public IActionResult CreateBreeding(Breeding breeding)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();

                    using (SqlCommand insertCmd = new SqlCommand("INSERT INTO Breadings (EId, ProductId, Price, SemenPer, SemenDate, DeliveryDate, AnimalID, OutsiderId, CompanyId, Status, AddedBy, AddedDate, AddedPc, UpdatedBy, UpdatedDate, UpDatedPc) VALUES (@EId, @ProductId, @Price, @SemenPer, @SemenDate, @DeliveryDate, @AnimalID, @OutsiderId, @CompanyId, @Status, @AddedBy, @AddedDate, @AddedPc, @UpdatedBy, @UpdatedDate, @UpDatedPc)", con))
                    {
                        insertCmd.Parameters.AddWithValue("@EId", breeding.EId);
                        insertCmd.Parameters.AddWithValue("@ProductId", breeding.ProductId);
                        insertCmd.Parameters.AddWithValue("@Price", breeding.Price ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@SemenPer", breeding.SemenPer ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@SemenDate", breeding.SemenDate ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@DeliveryDate", breeding.DeliveryDate ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@AnimalID", breeding.AnimalID);
                        insertCmd.Parameters.AddWithValue("@OutsiderId", breeding.OutsiderId);
                        insertCmd.Parameters.AddWithValue("@CompanyId", breeding.CompanyId);
                        insertCmd.Parameters.AddWithValue("@Status", breeding.Status);
                        insertCmd.Parameters.AddWithValue("@AddedBy", breeding.AddedBy ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@AddedDate", breeding.AddedDate ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@AddedPc", breeding.AddedPc ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@UpdatedBy", breeding.UpdatedBy ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@UpdatedDate", breeding.UpdatedDate ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@UpDatedPc", breeding.UpDatedPc ?? (object)DBNull.Value);
                        insertCmd.ExecuteNonQuery();
                    }

                    return Ok(new { message = "Breeding created successfully" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the request", error = ex.Message });
            }
        }
        [HttpPut("UpdateBreeding")]
        public IActionResult UpdateBreeding([FromForm] Breeding breeding)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();

                    SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Breadings WHERE BreadingId = @BreadingId", con);
                    checkCmd.Parameters.AddWithValue("@BreadingId", breeding.BreadingId);

                    int breedingCount = (int)checkCmd.ExecuteScalar();

                    if (breedingCount == 0)
                    {
                        return NotFound(new { message = $"Breeding with ID {breeding.BreadingId} not found" });
                    }
                    using (SqlCommand updateCmd = new SqlCommand("UPDATE Breadings SET EId = @EId, ProductId = @ProductId, Price = @Price, SemenPer = @SemenPer, SemenDate = @SemenDate, DeliveryDate = @DeliveryDate, AnimalID = @AnimalID, OutsiderId = @OutsiderId, Status = @Status, UpdatedBy = @UpdatedBy, UpdatedDate = @UpdatedDate, UpDatedPc = @UpDatedPc WHERE BreadingId = @BreadingId", con))
                    {
                        updateCmd.Parameters.AddWithValue("@BreadingId", breeding.BreadingId);
                        updateCmd.Parameters.AddWithValue("@EId", breeding.EId);
                        updateCmd.Parameters.AddWithValue("@ProductId", breeding.ProductId);
                        updateCmd.Parameters.AddWithValue("@Price", breeding.Price ?? (object)DBNull.Value);
                        updateCmd.Parameters.AddWithValue("@SemenPer", breeding.SemenPer ?? (object)DBNull.Value);
                        updateCmd.Parameters.AddWithValue("@SemenDate", breeding.SemenDate ?? (object)DBNull.Value);
                        updateCmd.Parameters.AddWithValue("@DeliveryDate", breeding.DeliveryDate ?? (object)DBNull.Value);
                        updateCmd.Parameters.AddWithValue("@AnimalID", breeding.AnimalID);
                        updateCmd.Parameters.AddWithValue("@OutsiderId", breeding.OutsiderId);
                        updateCmd.Parameters.AddWithValue("@Status", breeding.Status);
                        updateCmd.Parameters.AddWithValue("@UpdatedBy", breeding.UpdatedBy ?? (object)DBNull.Value);
                        updateCmd.Parameters.AddWithValue("@UpdatedDate", breeding.UpdatedDate ?? (object)DBNull.Value);
                        updateCmd.Parameters.AddWithValue("@UpDatedPc", breeding.UpDatedPc ?? (object)DBNull.Value);
                        updateCmd.ExecuteNonQuery();
                    }
                    return Ok(new { message = $"Breeding with ID {breeding.BreadingId} updated successfully" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the request", error = ex.Message });
            }
        }
        [HttpDelete("DeleteBreeding/{id}")]
        public IActionResult DeleteBreeding(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Breadings WHERE BreadingId = @BreadingId", con);
                    checkCmd.Parameters.AddWithValue("@BreadingId", id);

                    int breedingCount = (int)checkCmd.ExecuteScalar();

                    if (breedingCount == 0)
                    {
                        return NotFound(new { message = $"Breeding with ID {id} not found" });
                    }
                    using (SqlCommand deleteCmd = new SqlCommand("DELETE FROM Breadings WHERE BreadingId = @BreadingId", con))
                    {
                        deleteCmd.Parameters.AddWithValue("@BreadingId", id);
                        deleteCmd.ExecuteNonQuery();
                    }
                    return Ok(new { message = $"Breeding with ID {id} deleted successfully" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the request", error = ex.Message });
            }
        }
        [HttpGet("GetBreeding")]
        public IActionResult GetBreeding( int CompanyId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();

                    SqlCommand getBreedingCmd = new SqlCommand("GetAllBreadings", con);
                    getBreedingCmd.Parameters.AddWithValue("@CompanyId", CompanyId);
                    getBreedingCmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = getBreedingCmd.ExecuteReader();

                    List<BreedingDetails> breedings = new List<BreedingDetails>();

                    while (reader.Read())
                    {
                        BreedingDetails breeding = new BreedingDetails
                        {
                            BreadingId = (int)reader["BreadingId"],
                            EmployeeName = reader["EmployeeName"].ToString(),
                            ProductName = reader["ProductName"].ToString(),
                            Price = reader["Price"].ToString(),
                            SemenPer = reader["SemenPer"].ToString(),
                            SemenDate = reader["SemenDate"] != DBNull.Value ? (DateTime)reader["SemenDate"] : default(DateTime),
                            DeliveryDate = reader["DeliveryDate"] != DBNull.Value ? (DateTime)reader["DeliveryDate"] : default(DateTime),
                            AnimalName = reader["AnimalName"].ToString(),
                            Status = reader["Status"] != DBNull.Value ? Convert.ToBoolean(reader["Status"]) : false,
                            Outsider = reader["Outsider"].ToString()
                        };

                        breedings.Add(breeding);
                    }

                    if (breedings.Count > 0)
                    {
                        return Ok(breedings);
                    }
                    else
                    {
                        return NotFound(new { message = "No breedings found" });
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the request", error = ex.Message });
            }
        }

        [HttpGet("Semen")]
        public IActionResult GetActiveSemen(int CompanyId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("GetProductsByCategory", connection))
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
