using FarmProject.ConnectionString;
using FarmProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.Drawing;
using System.Security.Cryptography;

namespace FarmProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly string ConnectionString;
        private readonly IWebHostEnvironment _environment;

        public EmployeeController(Connection connection, IWebHostEnvironment environment)
        {
            ConnectionString = connection.Cnstr;
            _environment = environment;
        }


        [HttpPost("CreateEmployee")]
        public async Task<IActionResult> CreateEmployee([FromForm] Employee employee)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    if (employee.ImageFile is null || employee.ImageFile.Length == 0)
                    {
                        return BadRequest("Uploaded file is empty");
                    }

                    var uploadsFolderPath = Path.Combine(_environment.ContentRootPath, "Images", "EmployeeImage");
                    if (!Directory.Exists(uploadsFolderPath))
                    {
                        Directory.CreateDirectory(uploadsFolderPath);
                    }

                    var imageName = $"{employee.FirstName}-{employee.MiddleName}-{employee.LastName}";
                    var fileExtension = Path.GetExtension(employee.ImageFile.FileName);

                    var imageFileName = GetUniqueFileName(uploadsFolderPath, imageName, fileExtension);
                    var imagePath = Path.Combine(uploadsFolderPath, imageFileName);


                    using (var stream = System.IO.File.Create(imagePath))
                    {
                        await employee.ImageFile.CopyToAsync(stream);
                    }

                    string systemCode = string.Empty;
                    SqlCommand cmdSP = new SqlCommand("spMakeSystemCode", con);
                    {
                        cmdSP.CommandType = CommandType.StoredProcedure;
                        cmdSP.Parameters.AddWithValue("@TableName", "Employees");
                        cmdSP.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"));
                        cmdSP.Parameters.AddWithValue("@AddNumber", 1);
                        systemCode = cmdSP.ExecuteScalar()?.ToString();
                    }

                    employee.EmployeeCode = systemCode.Split('%')[1];

                    using (SqlCommand insertCmd = new SqlCommand("INSERT INTO Employees (EmployeeCode, FirstName, LastName, MiddleName, Salary, DOB, HireDate, PhoneNumber, Email, NID, Description, JobTitle, Status, EmployeeImage,CompanyId, AddedBy, AddedDate, AddedPc, UpdatedBy, UpdatedDate, UpdatedPc) VALUES (@EmployeeCode, @FirstName, @LastName, @MiddleName, @Salary, @DOB, @HireDate, @PhoneNumber, @Email, @NID, @Description, @JobTitle, @Status, @EmployeeImage,@CompanyId, @AddedBy, @AddedDate, @AddedPc, @UpdatedBy, @UpdatedDate, @UpdatedPc)", con))
                    {
                        insertCmd.Parameters.AddWithValue("@EmployeeCode", employee.EmployeeCode);
                        insertCmd.Parameters.AddWithValue("@FirstName", employee.FirstName ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@MiddleName", employee.MiddleName ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@LastName", employee.LastName ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@Salary", employee.Salary);
                        insertCmd.Parameters.AddWithValue("@DOB", employee.DOB ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@HireDate", employee.HireDate != null ? (object)employee.HireDate : DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@PhoneNumber", employee.PhoneNumber ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@Email", employee.Email ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@NID", employee.NID ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@Description", employee.Description ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@JobTitle", employee.JobTitle ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@Status", employee.Status);
                        object logoValue = (object)imageFileName ?? DBNull.Value;
                        insertCmd.Parameters.AddWithValue("@EmployeeImage", logoValue);
                        insertCmd.Parameters.AddWithValue("@CompanyId", employee.CompanyId);
                        insertCmd.Parameters.AddWithValue("@AddedBy", employee.AddedBy ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@AddedDate", DateTime.Now);
                        insertCmd.Parameters.AddWithValue("@AddedPc", employee.AddedPc ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@UpdatedBy", employee.UpdatedBy ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@UpdatedDate", employee.UpdatedDate != null ? (object)employee.UpdatedDate : DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@UpdatedPc", employee.UpDatedPc ?? (object)DBNull.Value);

                        insertCmd.ExecuteNonQuery();
                    }

                    return Ok(new { message = "Employee created successfully" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the request", error = ex.Message });
            }
        }

        [HttpPut("UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployee([FromForm] Employee employee)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();

                    if (employee.EId == null)
                    {
                        return BadRequest();
                    }

                    SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Employees WHERE EId = @EId", con);
                    if (employee.EId != null)
                    {
                        checkCmd.Parameters.AddWithValue("@EId", employee.EId);
                    }
                    else
                    {
                        return BadRequest();
                    }

                    int animalCount = (int)checkCmd.ExecuteScalar();

                    if (animalCount == 0)
                    {
                        return NotFound();
                    }

                    var uploadsFolderPath = Path.Combine(_environment.ContentRootPath, "Images", "EmployeeImage");

                    if (!Directory.Exists(uploadsFolderPath))
                    {
                        Directory.CreateDirectory(uploadsFolderPath);
                    }

                    string existingImageFileName;
                    SqlCommand getImageCmd = new SqlCommand("SELECT EmployeeImage FROM Employees WHERE EId = @EId", con);
                    getImageCmd.Parameters.AddWithValue("@EId", employee.EId);
                    existingImageFileName = getImageCmd.ExecuteScalar() as string;

                    // Check if ImageFile is provided
                    if (employee.ImageFile != null)
                    {
                        // Delete existing image if it exists
                        if (!string.IsNullOrEmpty(existingImageFileName))
                        {
                            var existingImagePath = Path.Combine(uploadsFolderPath, existingImageFileName);
                            System.IO.File.Delete(existingImagePath);
                        }

                        // Save the new image
                        var imageName = $"{employee.FirstName}-{employee.MiddleName}-{employee.LastName}";
                        var fileExtension = Path.GetExtension(employee.ImageFile.FileName);
                        var newImageFileName = GetUniqueFileName(uploadsFolderPath, imageName, fileExtension);

                        var imagePath = Path.Combine(uploadsFolderPath, newImageFileName);
                        using (var stream = System.IO.File.Create(imagePath))
                        {
                            await employee.ImageFile.CopyToAsync(stream);
                        }

                        // Update the database with the new image
                        using (SqlCommand updateCmd = new SqlCommand("UPDATE Employees SET FirstName = @FirstName, LastName = @LastName, MiddleName = @MiddleName, Salary = @Salary, DOB = @DOB, HireDate = @HireDate, PhoneNumber = @PhoneNumber, Email = @Email, NID = @NID, Description = @Description, JobTitle = @JobTitle, Status = @Status, EmployeeImage = @EmployeeImage, UpdatedBy = @UpdatedBy, UpdatedDate = @UpdatedDate, UpdatedPc = @UpdatedPc WHERE EId = @EId", con))
                        {
                            updateCmd.Parameters.AddWithValue("@EId", employee.EId);
                            updateCmd.Parameters.AddWithValue("@FirstName", employee.FirstName ?? (object)DBNull.Value);
                            updateCmd.Parameters.AddWithValue("@MiddleName", employee.MiddleName ?? (object)DBNull.Value);
                            updateCmd.Parameters.AddWithValue("@LastName", employee.LastName ?? (object)DBNull.Value);
                            updateCmd.Parameters.AddWithValue("@Salary", employee.Salary);
                            updateCmd.Parameters.AddWithValue("@DOB", employee.DOB ?? (object)DBNull.Value);
                            updateCmd.Parameters.AddWithValue("@HireDate", employee.HireDate != null ? (object)employee.HireDate : DBNull.Value);
                            updateCmd.Parameters.AddWithValue("@PhoneNumber", employee.PhoneNumber ?? (object)DBNull.Value);
                            updateCmd.Parameters.AddWithValue("@Email", employee.Email ?? (object)DBNull.Value);
                            updateCmd.Parameters.AddWithValue("@NID", employee.NID ?? (object)DBNull.Value);
                            updateCmd.Parameters.AddWithValue("@Description", employee.Description ?? (object)DBNull.Value);
                            updateCmd.Parameters.AddWithValue("@JobTitle", employee.JobTitle ?? (object)DBNull.Value);

                            object logoValue = (object)newImageFileName ?? DBNull.Value;
                            updateCmd.Parameters.AddWithValue("@EmployeeImage", logoValue);
                            updateCmd.Parameters.AddWithValue("@Status", employee.Status);
                            updateCmd.Parameters.AddWithValue("@UpdatedBy", employee.UpdatedBy ?? (object)DBNull.Value);
                            updateCmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                            updateCmd.Parameters.AddWithValue("@UpdatedPc", employee.UpDatedPc ?? (object)DBNull.Value);

                            updateCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(existingImageFileName))
                        {
                            return BadRequest();
                        }

                        using (SqlCommand updateCmd = new SqlCommand("UPDATE Employees SET FirstName = @FirstName, LastName = @LastName, MiddleName = @MiddleName, Salary = @Salary, DOB = @DOB, HireDate = @HireDate, PhoneNumber = @PhoneNumber, Email = @Email, NID = @NID, Description = @Description, JobTitle = @JobTitle, Status = @Status, UpdatedBy = @UpdatedBy, UpdatedDate = @UpdatedDate, UpdatedPc = @UpdatedPc WHERE EId = @EId", con))
                        {
                            updateCmd.Parameters.AddWithValue("@EId", employee.EId);
                            updateCmd.Parameters.AddWithValue("@FirstName", employee.FirstName ?? (object)DBNull.Value);
                            updateCmd.Parameters.AddWithValue("@MiddleName", employee.MiddleName ?? (object)DBNull.Value);
                            updateCmd.Parameters.AddWithValue("@LastName", employee.LastName ?? (object)DBNull.Value);
                            updateCmd.Parameters.AddWithValue("@Salary", employee.Salary);
                            updateCmd.Parameters.AddWithValue("@DOB", employee.DOB ?? (object)DBNull.Value);
                            updateCmd.Parameters.AddWithValue("@HireDate", employee.HireDate != null ? (object)employee.HireDate : DBNull.Value);
                            updateCmd.Parameters.AddWithValue("@PhoneNumber", employee.PhoneNumber ?? (object)DBNull.Value);
                            updateCmd.Parameters.AddWithValue("@Email", employee.Email ?? (object)DBNull.Value);
                            updateCmd.Parameters.AddWithValue("@NID", employee.NID ?? (object)DBNull.Value);
                            updateCmd.Parameters.AddWithValue("@Description", employee.Description ?? (object)DBNull.Value);
                            updateCmd.Parameters.AddWithValue("@JobTitle", employee.JobTitle ?? (object)DBNull.Value);
                            updateCmd.Parameters.AddWithValue("@Status", employee.Status);
                            updateCmd.Parameters.AddWithValue("@UpdatedBy", employee.UpdatedBy ?? (object)DBNull.Value);
                            updateCmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                            updateCmd.Parameters.AddWithValue("@UpdatedPc", employee.UpDatedPc ?? (object)DBNull.Value);

                            updateCmd.ExecuteNonQuery();
                        }
                    }
                    return Ok(new { message = "Employee Updated successfully" });
                }
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }


        [HttpDelete("DeleteEmployee/{EId}")]
        public IActionResult DeleteEmployee(int EId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();

                    SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Employees WHERE EId = @EId", con);
                    checkCmd.Parameters.AddWithValue("@EId", EId);

                    int animalCount = (int)checkCmd.ExecuteScalar();

                    if (animalCount == 0)
                    {
                        return NotFound(new { message = $"Employee with ID {EId} not found" });
                    }

                    string imageFileName;
                    SqlCommand getImageCmd = new SqlCommand("SELECT EmployeeImage FROM Employees WHERE EId = @EId", con);
                    getImageCmd.Parameters.AddWithValue("@EId", EId);
                    imageFileName = getImageCmd.ExecuteScalar() as string;

                    using (SqlCommand deleteCmd = new SqlCommand("spDeleteEmployee", con))
                    {
                        deleteCmd.CommandType = CommandType.StoredProcedure;
                        deleteCmd.Parameters.AddWithValue("@EId", EId);
                        deleteCmd.ExecuteNonQuery();
                    }

                    if (!string.IsNullOrEmpty(imageFileName))
                    {
                        var uploadsFolderPath = Path.Combine(_environment.ContentRootPath, "Images", "EmployeeImage");
                        var imagePath = Path.Combine(uploadsFolderPath, imageFileName);

                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                            return Ok(new { message = $"Employee with ID {EId} and associated image deleted successfully" });
                        }
                    }

                    return Ok(new { message = $"Employee with ID {EId} deleted successfully" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the request", error = ex.Message });
            }
        }

        [HttpGet("GetEmployee")]
        public IActionResult GetEmployee(int EId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();

                    SqlCommand getAnimalCmd = new SqlCommand("GetEmployeeById", con);
                    getAnimalCmd.CommandType = CommandType.StoredProcedure;
                    getAnimalCmd.Parameters.AddWithValue("@EId", EId);

                    SqlDataReader reader = getAnimalCmd.ExecuteReader();

                    if (reader.Read())
                    {
                        EmployeeGet animal = new EmployeeGet
                        {
                            //AnimalId = (int)reader["AnimalId"],
                            EmployeeCode = reader["EmployeeCode"].ToString(),
                            FirstName = reader["FirstName"].ToString(),
                            MiddleName = reader["MiddleName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Salary = (decimal)reader["Salary"],
                            PhoneNumber = int.TryParse(reader["PhoneNumber"].ToString(), out int phoneNumber) ? phoneNumber : (int?)null,
                            DOB = reader["DOB"] != DBNull.Value ? (DateTime?)reader["DOB"] : null,
                            HireDate = reader["HireDate"] != DBNull.Value ? (DateTime?)reader["HireDate"] : null,
                            Email = reader["Email"].ToString(),
                            Description = reader["Description"].ToString(),
                            EmployeeImage = reader["EmployeeImage"].ToString(),
                            Status = (bool)reader["Status"]  
                        };

                        return Ok(animal);
                    }
                    else
                    {
                        return NotFound(new { message = $"Animal with ID {EId} not found" });
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the request", error = ex.Message });
            }
        }

        [HttpGet("GetActiveEmployees")]
        public IActionResult GetActiveEmployees(int CompanyId)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("GetEmployeesByCompanyId", connection))
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
        private string GetUniqueFileName(string directory, string Name, string extension)
        {
            string uniqueFileName = $"{Name}{extension}";
            string filePath = Path.Combine(directory, uniqueFileName);

            int counter = 1;
            while (System.IO.File.Exists(filePath))
            {
                uniqueFileName = $"{Name}_{counter}{extension}";
                filePath = Path.Combine(directory, uniqueFileName);
                counter++;
            }

            return uniqueFileName;
        }
    }
}
