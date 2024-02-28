using FarmProject.ConnectionString;
using FarmProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;

namespace FarmProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginController : ControllerBase
    {
        private readonly string ConnectionString;

        public UserLoginController(Connection connection)
        {
            ConnectionString = connection.Cnstr;
        }
        [HttpPost]
        [Route("login")]
        public IActionResult LoginUser([FromForm] UserLogIn user)
        {
            bool a = false;
            string encryptedPassword = EncryptPassword(user.Password);

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT * FROM UserInfo WHERE UserCode = @UserCode AND password = @password AND CompanyId = @CompanyId", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@UserCode", user.UserCode);
                    cmd.Parameters.AddWithValue("@password", encryptedPassword);
                    cmd.Parameters.AddWithValue("@CompanyId", user.CompanyId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string encryptedUserCode = reader["UserCode"].ToString();
                            a = true;
                            return Ok(new { message = "Login successful", encryptedUserCode, a, user.CompanyId });
                        }
                        else
                        {
                            return BadRequest(new { message = "Invalid User Name or password" });
                        }
                    }
                }
            }
        }

        public static string EncryptPassword(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        private string DecryptPassword(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            try
            {
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                return cipherText;
            }
            catch (FormatException ex)
            {
                return "Decryption failed: " + ex.Message;
            }
            catch (Exception ex)
            {
                return "An error occurred during decryption: " + ex.Message;
            }
        }
    }
}
