using FarmProject.Manager.UserParmissions;
using FarmProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;

namespace FarmProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPermissionController : ControllerBase
    {
        private readonly InterfaceUserPermission userPermissionBusinessLogic;

        public UserPermissionController(InterfaceUserPermission userPermissionBusinessLogic)
        {
            this.userPermissionBusinessLogic = userPermissionBusinessLogic;
        }

        [HttpPost("InsertUserPermission")]
        public IActionResult InsertUserPermission([FromForm]IEnumerable<userpermissionPOST> userpost)
        {
            try
            {
                userPermissionBusinessLogic.InsertUserPermission(userpost);
                return Ok("User permissions inserted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpGet("GetAllUserInfo")]
        public IActionResult GetAllUserInfo(int CompanyId)
        {
            try
            {
                var userInfoList = userPermissionBusinessLogic.GetAllUserInfo(CompanyId);
                return Ok(userInfoList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        [HttpGet("allMenuList")]
        public IActionResult GetallMenuList()
        {
            try
            {
                var userInfoList = userPermissionBusinessLogic.GetallMenuList();
                return Ok(userInfoList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        [HttpGet("GetMenuByUserCodeAndCompany")]
        public IActionResult GetMenuByUserCode(string userCode, int CompanyId)
        {
            try
            {
                var reportMenuList = userPermissionBusinessLogic.GetMenuByUserCode(userCode, CompanyId);
                return Ok(reportMenuList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}

