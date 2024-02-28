using FarmProject.Manager.Stock;
using FarmProject.Manager.User;
using FarmProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace FarmProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        private readonly InterfaceUser userBusinessLogic;

        public UserInfoController(InterfaceUser userBusinessLogic)
        {
            this.userBusinessLogic = userBusinessLogic;
        }

        [HttpPost("CreateUser")]
        public IActionResult CreateUser([FromForm] UserInfo user)
        {
            try
            {
                bool createUserSuccessful = userBusinessLogic.CreateUser(user);

                if (createUserSuccessful)
                {
                    return Ok(new { message = "User created successfully" });
                }
                else
                {
                    return BadRequest(new { message = "User creation failed. Fill the form correctly." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the request", error = ex.Message });
            }
        }

        [HttpPut("UpdateUser")]
        public IActionResult UpdateUser([FromForm] UserInfo user)
        {
            try
            {
                bool updateUserSuccessful = userBusinessLogic.UpdateUser(user);

                if (updateUserSuccessful)
                {
                    return Ok(new { message = "User updated successfully" });
                }
                else
                {
                    return BadRequest(new { message = "User update failed. Fill the form correctly." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the request", error = ex.Message });
            }
        }
        [HttpDelete("DeleteUser/{userCode}")]
        public IActionResult DeleteUser(string userCode)
        {
            try
            {
                userBusinessLogic.DeleteUser(userCode);
                return Ok(new { message = "User deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the request", error = ex.Message });
            }
        }

        [HttpGet("GetAllUserInfo")]
        public IActionResult GetAllUserInfo(int CompanyId)
        {
            try
            {
                var res = userBusinessLogic.GetAllUserInfo(CompanyId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        [HttpGet("GetUserInfoByUserCode")]
        public IActionResult  GetUserInfoByUserCode(string userCode)
        {
            try
            {
                var res = userBusinessLogic.GetUserInfoByUserCode(userCode);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        [HttpPut("UpdatePassword")]
        public IActionResult UpdatePassword(UpdatePassword user)
        {
            try
            {
                var res = userBusinessLogic.UpdatePassword(user);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
