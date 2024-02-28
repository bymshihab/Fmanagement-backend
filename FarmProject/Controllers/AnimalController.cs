using FarmProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using FarmProject.ConnectionString;
using System.Diagnostics.Metrics;

namespace FarmProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalController : ControllerBase
    {
        private readonly string ConnectionString;
        private readonly IWebHostEnvironment _environment;

        public AnimalController(Connection connection, IWebHostEnvironment environment)
        {
            ConnectionString = connection.Cnstr;
            _environment = environment;
        }


        [HttpPost("CreateAnimal")]
        public async Task<IActionResult> CreateAnimal([FromForm] Animal animal)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    if (animal.ImageFile is null || animal.ImageFile.Length == 0)
                    {
                        return BadRequest("Uploaded file is empty");
                    }

                    var uploadsFolderPath = Path.Combine(_environment.ContentRootPath, "Images", "Uploads");
                    if (!Directory.Exists(uploadsFolderPath))
                    {
                        Directory.CreateDirectory(uploadsFolderPath);
                    }

                    var imageName = $"{animal.AnimalName}";
                    var fileExtension = Path.GetExtension(animal.ImageFile.FileName);

                    var imageFileName = GetUniqueFileName(uploadsFolderPath, imageName, fileExtension);
                    var imagePath = Path.Combine(uploadsFolderPath, imageFileName);


                    using (var stream = System.IO.File.Create(imagePath))
                    {
                        await animal.ImageFile.CopyToAsync(stream);
                    }

                    string systemCode = string.Empty;
                    SqlCommand cmdSP = new SqlCommand("spMakeSystemCode", con);
                    {
                        cmdSP.CommandType = CommandType.StoredProcedure;
                        cmdSP.Parameters.AddWithValue("@TableName", "Animals");
                        cmdSP.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"));
                        cmdSP.Parameters.AddWithValue("@AddNumber", 1);
                        systemCode = cmdSP.ExecuteScalar()?.ToString();
                    }

                    animal.AnimalTagNo = systemCode.Split('%')[1];

                    string qrCodeDataString = $"{animal.AnimalId},{animal.AnimalName},{animal.AnimalTagNo},{animal.ProductId},{animal.ShedId},{animal.IsDead},{animal.IsSold},{animal.MilkId},{animal.weight},{animal.DOB},{animal.GenderId},{animal.IsVaccinated},{animal.Status}";
                    if (!string.IsNullOrEmpty(qrCodeDataString))
                    {
                        QRCodeGenerator qrGenerator = new QRCodeGenerator();
                        QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrCodeDataString, QRCodeGenerator.ECCLevel.Q);
                        QRCode qrCode = new QRCode(qrCodeData);
                        Bitmap qrCodeImage = qrCode.GetGraphic(20);


                        using (SqlCommand insertCmd = new SqlCommand("INSERT INTO Animals (AnimalName, AnimalTagNo, ProductId, ShedId, IsDead, IsSold, IsVaccinated, QRCodeData, MilkId, Weight, DOB, GenderId, Status, CompanyId, AnimalImage, AddedBy, AddedDate, AddedPc, UpdatedBy, UpdatedDate, UpdatedPc) VALUES (@AnimalName, @AnimalTagNo, @ProductId, @ShedId, @IsDead, @IsSold, @IsVaccinated, @QRCodeData, @MilkId, @Weight, @DOB, @GenderId, @Status, @CompanyId, @AnimalImage, @AddedBy, @AddedDate, @AddedPc, @UpdatedBy, @UpdatedDate, @UpdatedPc)", con))
                        {
                            insertCmd.Parameters.AddWithValue("@AnimalName", animal.AnimalName);
                            insertCmd.Parameters.AddWithValue("@AnimalTagNo", animal.AnimalTagNo ?? (object)DBNull.Value);
                            insertCmd.Parameters.AddWithValue("@ProductId", animal.ProductId);
                            insertCmd.Parameters.AddWithValue("@ShedId", animal.ShedId);
                            insertCmd.Parameters.AddWithValue("@IsDead", animal.IsDead);
                            insertCmd.Parameters.AddWithValue("@IsSold", animal.IsSold);
                            insertCmd.Parameters.AddWithValue("@IsVaccinated", animal.IsVaccinated);
                            insertCmd.Parameters.AddWithValue("@QRCodeData", qrCodeDataString);
                            insertCmd.Parameters.AddWithValue("@MilkId", animal.MilkId != null ? (object)animal.MilkId : DBNull.Value);
                            insertCmd.Parameters.AddWithValue("@Weight", animal.weight);
                            insertCmd.Parameters.AddWithValue("@DOB", animal.DOB != null ? (object)animal.DOB : DBNull.Value);
                            insertCmd.Parameters.AddWithValue("@GenderId", animal.GenderId);
                            insertCmd.Parameters.AddWithValue("@Status", animal.Status);
                            insertCmd.Parameters.AddWithValue("@CompanyId", animal.CompanyId);

                            object logoValue = (object)imageFileName ?? DBNull.Value;
                            insertCmd.Parameters.AddWithValue("@AnimalImage", logoValue);
                            insertCmd.Parameters.AddWithValue("@AddedBy", animal.AddedBy ?? (object)DBNull.Value);
                            insertCmd.Parameters.AddWithValue("@AddedDate", animal.AddedDate != null ? (object)animal.AddedDate : DBNull.Value);
                            insertCmd.Parameters.AddWithValue("@AddedPc", animal.AddedPc ?? (object)DBNull.Value);
                            insertCmd.Parameters.AddWithValue("@UpdatedBy", animal.UpdatedBy ?? (object)DBNull.Value);
                            insertCmd.Parameters.AddWithValue("@UpdatedDate", animal.UpdatedDate != null ? (object)animal.UpdatedDate : DBNull.Value);
                            insertCmd.Parameters.AddWithValue("@UpDatedPc", animal.UpDatedPc ?? (object)DBNull.Value);

                            insertCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        return BadRequest(new { message = "QRCodeData is null or empty" });
                    }

                    return Ok(new { message = "Animal created successfully" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the request", error = ex.Message });
            }
        }


        [HttpPut("UpdateAnimal")]
        public async Task<IActionResult> UpdateAnimal([FromForm] Animal animal)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();

                    if (animal.AnimalId == null)
                    {
                        return BadRequest();
                    }

                    SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Animals WHERE AnimalId = @AnimalId", con);
                    if (animal.AnimalId != null)
                    {
                        checkCmd.Parameters.AddWithValue("@AnimalId", animal.AnimalId);
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

                    var uploadsFolderPath = Path.Combine(_environment.ContentRootPath, "Images", "Uploads");

                    if (!Directory.Exists(uploadsFolderPath))
                    {
                        Directory.CreateDirectory(uploadsFolderPath);
                    }

                    string existingImageFileName;
                    SqlCommand getImageCmd = new SqlCommand("SELECT AnimalImage FROM Animals WHERE AnimalId = @AnimalId", con);
                    getImageCmd.Parameters.AddWithValue("@AnimalId", animal.AnimalId);
                    existingImageFileName = getImageCmd.ExecuteScalar() as string;

                    // Check if ImageFile is provided
                    if (animal.ImageFile != null)
                    {
                        // Delete existing image if it exists
                        if (!string.IsNullOrEmpty(existingImageFileName))
                        {
                            var existingImagePath = Path.Combine(uploadsFolderPath, existingImageFileName);
                            System.IO.File.Delete(existingImagePath);
                        }

                        // Save the new image
                        var imageName = $"{animal.AnimalName}";
                        var fileExtension = Path.GetExtension(animal.ImageFile.FileName);
                        var newImageFileName = GetUniqueFileName(uploadsFolderPath, imageName, fileExtension);

                        var imagePath = Path.Combine(uploadsFolderPath, newImageFileName);
                        using (var stream = System.IO.File.Create(imagePath))
                        {
                            await animal.ImageFile.CopyToAsync(stream);
                        }

                        // Update the database with the new image
                        using (SqlCommand updateCmd = new SqlCommand("UPDATE Animals SET AnimalName = @AnimalName, ProductId = @ProductId, ShedId = @ShedId, IsDead = @IsDead, IsSold = @IsSold, IsVaccinated = @IsVaccinated, QRCodeData = @QRCodeData, MilkId = @MilkId, Weight = @Weight, DOB = @DOB, GenderId = @GenderId, Status = @Status, AnimalImage = @AnimalImageData, UpdatedBy = @UpdatedBy, UpdatedDate = @UpdatedDate, UpdatedPc = @UpdatedPc WHERE AnimalId = @AnimalId", con))
                        {
                            updateCmd.Parameters.AddWithValue("@AnimalId", animal.AnimalId);
                            updateCmd.Parameters.AddWithValue("@AnimalName", animal.AnimalName);
                            updateCmd.Parameters.AddWithValue("@ProductId", animal.ProductId);
                            updateCmd.Parameters.AddWithValue("@ShedId", animal.ShedId);
                            updateCmd.Parameters.AddWithValue("@IsDead", animal.IsDead);
                            updateCmd.Parameters.AddWithValue("@IsSold", animal.IsSold);
                            updateCmd.Parameters.AddWithValue("@IsVaccinated", animal.IsVaccinated);
                            updateCmd.Parameters.AddWithValue("@QRCodeData", $"{animal.AnimalId},{animal.AnimalName},{animal.AnimalTagNo},{animal.ProductId},{animal.ShedId},{animal.IsDead},{animal.IsSold},{animal.MilkId},{animal.weight},{animal.DOB},{animal.GenderId},{animal.IsVaccinated},{animal.Status}");
                            updateCmd.Parameters.AddWithValue("@MilkId", animal.MilkId != null ? (object)animal.MilkId : DBNull.Value);
                            updateCmd.Parameters.AddWithValue("@Weight", animal.weight);
                            updateCmd.Parameters.AddWithValue("@DOB", animal.DOB != null ? (object)animal.DOB : DBNull.Value);
                            updateCmd.Parameters.AddWithValue("@GenderId", animal.GenderId);
                            updateCmd.Parameters.AddWithValue("@Status", animal.Status);

                            object logoValue = (object)newImageFileName ?? DBNull.Value;
                            updateCmd.Parameters.AddWithValue("@AnimalImageData", logoValue);
                            updateCmd.Parameters.AddWithValue("@UpdatedBy", animal.UpdatedBy ?? (object)DBNull.Value);
                            updateCmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                            updateCmd.Parameters.AddWithValue("@UpdatedPc", animal.UpDatedPc ?? (object)DBNull.Value);

                            updateCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(existingImageFileName))
                        {
                            return BadRequest();
                        }

                        using (SqlCommand updateCmd = new SqlCommand("UPDATE Animals SET AnimalName = @AnimalName, ProductId = @ProductId, ShedId = @ShedId, IsDead = @IsDead, IsSold = @IsSold, IsVaccinated = @IsVaccinated, QRCodeData = @QRCodeData, MilkId = @MilkId, Weight = @Weight, DOB = @DOB, GenderId = @GenderId, Status = @Status, UpdatedBy = @UpdatedBy, UpdatedDate = @UpdatedDate, UpdatedPc = @UpdatedPc WHERE AnimalId = @AnimalId", con))
                        {
                            updateCmd.Parameters.AddWithValue("@AnimalId", animal.AnimalId);
                            updateCmd.Parameters.AddWithValue("@AnimalName", animal.AnimalName);
                            updateCmd.Parameters.AddWithValue("@ProductId", animal.ProductId);
                            updateCmd.Parameters.AddWithValue("@ShedId", animal.ShedId);
                            updateCmd.Parameters.AddWithValue("@IsDead", animal.IsDead);
                            updateCmd.Parameters.AddWithValue("@IsSold", animal.IsSold);
                            updateCmd.Parameters.AddWithValue("@IsVaccinated", animal.IsVaccinated);
                            updateCmd.Parameters.AddWithValue("@QRCodeData", $"{animal.AnimalId},{animal.AnimalName},{animal.AnimalTagNo},{animal.ProductId},{animal.ShedId},{animal.IsDead},{animal.IsSold},{animal.MilkId},{animal.weight},{animal.DOB},{animal.GenderId},{animal.IsVaccinated},{animal.Status}");
                            updateCmd.Parameters.AddWithValue("@MilkId", animal.MilkId != null ? (object)animal.MilkId : DBNull.Value);
                            updateCmd.Parameters.AddWithValue("@Weight", animal.weight);
                            updateCmd.Parameters.AddWithValue("@DOB", animal.DOB != null ? (object)animal.DOB : DBNull.Value);
                            updateCmd.Parameters.AddWithValue("@GenderId", animal.GenderId);
                            updateCmd.Parameters.AddWithValue("@Status", animal.Status);
                            updateCmd.Parameters.AddWithValue("@UpdatedBy", animal.UpdatedBy ?? (object)DBNull.Value);
                            updateCmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                            updateCmd.Parameters.AddWithValue("@UpdatedPc", animal.UpDatedPc ?? (object)DBNull.Value);

                            updateCmd.ExecuteNonQuery();
                        }
                    }
                    return Ok(new { message = "Animal Updated successfully" });
                }
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }


        [HttpDelete("DeleteAnimal/{id}")]
        public IActionResult DeleteAnimal(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();

                    SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Animals WHERE AnimalId = @AnimalId", con);
                    checkCmd.Parameters.AddWithValue("@AnimalId", id);

                    int animalCount = (int)checkCmd.ExecuteScalar();

                    if (animalCount == 0)
                    {
                        return NotFound(new { message = $"Animal with ID {id} not found" });
                    }

                    string imageFileName;
                    SqlCommand getImageCmd = new SqlCommand("SELECT AnimalImage FROM Animals WHERE AnimalId = @AnimalId", con);
                    getImageCmd.Parameters.AddWithValue("@AnimalId", id);
                    imageFileName = getImageCmd.ExecuteScalar() as string;

                    using (SqlCommand deleteCmd = new SqlCommand("DELETE FROM Animals WHERE AnimalId = @AnimalId", con))
                    {
                        deleteCmd.Parameters.AddWithValue("@AnimalId", id);
                        deleteCmd.ExecuteNonQuery();
                    }
                    if (!string.IsNullOrEmpty(imageFileName))
                    {
                        var uploadsFolderPath = Path.Combine(_environment.ContentRootPath, "Images", "Uploads");
                        var imagePath = Path.Combine(uploadsFolderPath, imageFileName);

                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                            return Ok(new { message = $"Animal with ID {id} and associated image deleted successfully" });
                        }
                    }

                    return Ok(new { message = $"Animal with ID {id} deleted successfully" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the request", error = ex.Message });
            }
        }



        [HttpGet("GetAllAnimalDetails")]
        public IActionResult GetMedicalExhibitionData(int CompanyId)
        {
            List<AnimalDetail> animalDetails = new List<AnimalDetail>();

            SqlConnection con = new SqlConnection(ConnectionString);

            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("GetAnimalDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CompanyId", CompanyId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    AnimalDetail master = new AnimalDetail();
                    master.AnimalId = Convert.ToInt32(reader["AnimalId"].ToString());
                    master.AnimalImage = reader["AnimalImage"].ToString();
                    master.AnimalTagNo = reader["AnimalTagNo"].ToString();
                    master.AnimalName = reader["AnimalName"].ToString();
                    master.ProductId = Convert.ToInt32(reader["ProductId"].ToString());
                    master.ProductName = reader["ProductName"].ToString();
                    master.ShedId = Convert.ToInt32(reader["ShedId"].ToString());
                    master.ShedName = reader["ShedName"].ToString();
                    master.GenderId = Convert.ToInt32(reader["GenderId"].ToString());
                    master.GenderType = reader["GenderType"].ToString();
                    master.Weight = (decimal)reader["Weight"];
                    master.MilkId = reader["MilkId"] != DBNull.Value ? (int?)reader["MilkId"] : null;
                    master.MilkType = reader["MilkType"] != DBNull.Value ? (string)reader["MilkType"] : null;
                    master.DOB = reader["DOB"] != DBNull.Value ? (DateTime?)reader["DOB"] : null;
                    master.IsDead = (bool)reader["IsDead"];
                    master.IsSold = (bool)reader["IsSold"];
                    master.IsVaccinated = (bool)reader["IsVaccinated"];
                    master.Status = (bool)reader["Status"];
                    master.QRCodeData = reader["QRCodeData"].ToString();
                    animalDetails.Add(master);
                }

                if (animalDetails.Any())
                {
                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    foreach (var master in animalDetails)
                    {
                        QRCodeData qrCodeData = qrGenerator.CreateQrCode(master.QRCodeData, QRCodeGenerator.ECCLevel.Q);
                        QRCode qrCode = new QRCode(qrCodeData);
                        Bitmap qrCodeImage = qrCode.GetGraphic(20);

                        using (MemoryStream stream = new MemoryStream())
                        {
                            qrCodeImage.Save(stream, ImageFormat.Png);
                            byte[] qrCodeImageData = stream.ToArray();
                            string base64QRCodeImage = Convert.ToBase64String(qrCodeImageData);
                            master.QRCodeImageBase64 = base64QRCodeImage;
                        }
                    }
                }

                con.Close();
                return Ok(animalDetails);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpGet("GetAnimal")]
        public IActionResult GetAnimal(int AnimalId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();

                    SqlCommand getAnimalCmd = new SqlCommand("GetAnimalById", con);
                    getAnimalCmd.CommandType = CommandType.StoredProcedure;
                    getAnimalCmd.Parameters.AddWithValue("@AnimalId", AnimalId);

                    SqlDataReader reader = getAnimalCmd.ExecuteReader();

                    if (reader.Read())
                    {
                        AnimalQR animal = new AnimalQR
                        {
                            //AnimalId = (int)reader["AnimalId"],
                            AnimalName = reader["AnimalName"].ToString(),
                            AnimalTagNo = reader["AnimalTagNo"].ToString(),
                            ProductName = reader["ProductName"].ToString(),
                            ShedName = reader["ShedName"].ToString(),
                            AnimalImage = reader["AnimalImage"].ToString(),
                            IsDead = (bool)reader["IsDead"],
                            IsSold = (bool)reader["IsSold"],
                            IsVaccinated = (bool)reader["IsVaccinated"],
                            QRCodeData = reader["QRCodeData"].ToString(),
                            MilkId = reader["MilkId"] != DBNull.Value ? (int?)reader["MilkId"] : null,
                            weight = reader["Weight"] != DBNull.Value ? (decimal?)reader["Weight"] : null,
                            DOB = reader["DOB"] != DBNull.Value ? (DateTime?)reader["DOB"] : null,
                            GenderType = reader["GenderType"].ToString(),
                            Status = reader["Status"].ToString()
                        };


                        QRCodeGenerator qrGenerator = new QRCodeGenerator();
                        QRCodeData qrCodeData = qrGenerator.CreateQrCode(animal.QRCodeData, QRCodeGenerator.ECCLevel.Q);
                        QRCode qrCode = new QRCode(qrCodeData);
                        Bitmap qrCodeImage = qrCode.GetGraphic(20);

                        using (MemoryStream stream = new MemoryStream())
                        {
                            qrCodeImage.Save(stream, ImageFormat.Png);
                            byte[] qrCodeImageData = stream.ToArray();
                            string base64QRCodeImage = Convert.ToBase64String(qrCodeImageData);
                            animal.QRCodeImageBase64 = base64QRCodeImage;
                        }

                        return Ok(animal);
                    }
                    else
                    {
                        return NotFound(new { message = $"Animal with ID {AnimalId} not found" });
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the request", error = ex.Message });
            }
        }
        private string GetUniqueFileName(string directory, string animalName, string extension)
        {
            string uniqueFileName = $"{animalName}{extension}";
            string filePath = Path.Combine(directory, uniqueFileName);

            int counter = 1;
            while (System.IO.File.Exists(filePath))
            {
                uniqueFileName = $"{animalName}_{counter}{extension}";
                filePath = Path.Combine(directory, uniqueFileName);
                counter++;
            }

            return uniqueFileName;
        }

    }
}
