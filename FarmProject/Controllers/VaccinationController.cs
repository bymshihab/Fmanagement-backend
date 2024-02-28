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
    public class VaccinationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public VaccinationController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }
        [HttpPost("CreateVaccination")]
        public IActionResult CreateVaccination([FromForm] Vaccination vaccination)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();

                    using (SqlCommand insertCmd = new SqlCommand("INSERT INTO Vaccinations (VDate, AnimalId,ProductId,Price,EId,ExpDate,CompanyId,OutsiderId, Status, AddedBy, AddedDate, AddedPc, UpdatedBy, UpdatedDate, UpDatedPc) VALUES (@VDate, @AnimalId,@ProductId,@Price,@EId,@ExpDate,@CompanyId,@OutsiderId, @Status, @AddedBy, @AddedDate, @AddedPc, @UpdatedBy, @UpdatedDate, @UpDatedPc)", con))
                    {
                        insertCmd.Parameters.AddWithValue("@VDate", vaccination.VDate);
                        insertCmd.Parameters.AddWithValue("@AnimalId", vaccination.AnimalId);
                        insertCmd.Parameters.AddWithValue("@ProductId", vaccination.ProductId);
                        insertCmd.Parameters.AddWithValue("@Price", vaccination.Price);
                        insertCmd.Parameters.AddWithValue("@EId", vaccination.EId);
                        insertCmd.Parameters.AddWithValue("@ExpDate", vaccination.ExpDate);
                        insertCmd.Parameters.AddWithValue("@CompanyId", vaccination.CompanyId);
                        insertCmd.Parameters.AddWithValue("@OutsiderId", vaccination.OutsiderId);
                        insertCmd.Parameters.AddWithValue("@Status", vaccination.Status);
                        insertCmd.Parameters.AddWithValue("@AddedBy", vaccination.AddedBy);
                        insertCmd.Parameters.AddWithValue("@AddedDate", vaccination.AddedDate ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@AddedPc", vaccination.AddedPc);
                        insertCmd.Parameters.AddWithValue("@UpdatedBy", vaccination.UpdatedBy ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@UpdatedDate", vaccination.UpdatedDate ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@UpDatedPc", vaccination.UpDatedPc ?? (object)DBNull.Value);
                        insertCmd.ExecuteNonQuery();
                    }
                    con.Close();
                    return Ok(new { message = "Vaccination created successfully" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the request", error = ex.Message });
            }
        }
        [HttpPut("UpdateVaccination")]
        public IActionResult UpdateVaccination([FromForm] Vaccination vaccination)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();

                    // Check if the vaccination with the given id exists
                    SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Vaccinations WHERE VaccinationId = @VaccinationId", con);
                    checkCmd.Parameters.AddWithValue("@VaccinationId", vaccination.VaccinationId);

                    int vaccinationCount = (int)checkCmd.ExecuteScalar();

                    if (vaccinationCount == 0)
                    {
                        return NotFound(new { message = $"Vaccination with ID {vaccination.VaccinationId} not found" });
                    }

                    using (SqlCommand updateCmd = new SqlCommand("UPDATE Vaccinations SET VDate = @VDate, AnimalId = @AnimalId, ProductId = @ProductId, Price = @Price, EId = @EId, ExpDate = @ExpDate, OutsiderId = @OutsiderId, Status = @Status, UpdatedBy = @UpdatedBy, UpdatedDate = @UpdatedDate, UpDatedPc = @UpDatedPc WHERE VaccinationId = @VaccinationId", con))
                    {
                        updateCmd.Parameters.AddWithValue("@VaccinationId", vaccination.VaccinationId);
                        updateCmd.Parameters.AddWithValue("@VDate", vaccination.VDate);
                        updateCmd.Parameters.AddWithValue("@AnimalId", vaccination.AnimalId);
                        updateCmd.Parameters.AddWithValue("@ProductId", vaccination.ProductId);
                        updateCmd.Parameters.AddWithValue("@Price", vaccination.Price);
                        updateCmd.Parameters.AddWithValue("@EId", vaccination.EId);
                        updateCmd.Parameters.AddWithValue("@ExpDate", vaccination.ExpDate);
                        updateCmd.Parameters.AddWithValue("@OutsiderId", vaccination.OutsiderId);
                        updateCmd.Parameters.AddWithValue("@Status", vaccination.Status);
                        updateCmd.Parameters.AddWithValue("@UpdatedBy", vaccination.UpdatedBy ?? (object)DBNull.Value);
                        updateCmd.Parameters.AddWithValue("@UpdatedDate", vaccination.UpdatedDate ?? (object)DBNull.Value);
                        updateCmd.Parameters.AddWithValue("@UpDatedPc", vaccination.UpDatedPc ?? (object)DBNull.Value);
                        updateCmd.ExecuteNonQuery();
                    }

                    return Ok(new { message = $"Vaccination with ID {vaccination.VaccinationId} updated successfully" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the request", error = ex.Message });
            }
        }
        [HttpDelete("DeleteVaccination")]
        public IActionResult DeleteVaccination(int VaccinationId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();

                    SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Vaccinations WHERE VaccinationId = @VaccinationId", con);
                    checkCmd.Parameters.AddWithValue("@VaccinationId", VaccinationId);

                    int vaccinationCount = (int)checkCmd.ExecuteScalar();

                    if (vaccinationCount == 0)
                    {
                        return NotFound(new { message = $"Vaccination with ID {VaccinationId} not found" });
                    }

                    using (SqlCommand deleteCmd = new SqlCommand("DELETE FROM Vaccinations WHERE VaccinationId = @VaccinationId", con))
                    {
                        deleteCmd.Parameters.AddWithValue("@VaccinationId", VaccinationId);
                        deleteCmd.ExecuteNonQuery();
                    }

                    return Ok(new { message = $"Vaccination with ID {VaccinationId} deleted successfully" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the request", error = ex.Message });
            }
        }
        [HttpGet("GetVaccination")]
        public IActionResult GetVaccination(int CompanyId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();

                    SqlCommand getVaccinationCmd = new SqlCommand("GetAllVaccination", con);
                    getVaccinationCmd.Parameters.AddWithValue("@CompanyId", CompanyId);
                    getVaccinationCmd.CommandType = CommandType.StoredProcedure;

                    SqlDataReader reader = getVaccinationCmd.ExecuteReader();

                    List<VaccinationDetails> vaccinations = new List<VaccinationDetails>();

                    while (reader.Read())
                    {
                        VaccinationDetails vaccination = new VaccinationDetails
                        {
                            VaccinationId = (int)reader["VaccinationId"],
                            VDate = (DateTime)reader["VDate"],
                            AnimalName = reader["AnimalName"].ToString(),
                            ProductName = reader["ProductName"].ToString(),
                            Price = (decimal)reader["Price"],
                            EmployeeName = reader["EmployeeName"].ToString(),
                            ExpDate = (DateTime)reader["ExpDate"],
                            Outsider = reader["Outsider"].ToString(),
                            Status = reader["Status"] != DBNull.Value ? Convert.ToBoolean(reader["Status"]) : false,
                        };

                        vaccinations.Add(vaccination);
                    }

                    if (vaccinations.Count > 0)
                    {
                        return Ok(vaccinations);
                    }
                    else
                    {
                        return NotFound(new { message = "No vaccinations found" });
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the request", error = ex.Message });
            }
        }
        [HttpGet("Vaccine")]
        public IActionResult GetProductsByVaccine(int CompanyId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("GetProductsByVaccine", connection))
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
