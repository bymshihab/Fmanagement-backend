using FarmProject.Manager.Module;
using FarmProject.Manager.ShedType;
using FarmProject.Manager.Uom;
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
    public class ShedTypeController : ControllerBase
    {
        private readonly InterfaceShedType shedTypeBusinessLogic;

        public ShedTypeController(InterfaceShedType shedTypeBusinessLogic)
       {
            this.shedTypeBusinessLogic = shedTypeBusinessLogic;

        }
        [HttpPost("CreateShedType")]
        public IActionResult CreateShedType( ShedTypes shedTypes)
        {
            try
            {
                shedTypes.shedTypeId = 0;

                bool createSuccessful = shedTypeBusinessLogic.CreateShedType(shedTypes);

                if (createSuccessful)
                {
                    return Ok(new { message = "ShedType created successfully" });
                }
                else
                {
                    return BadRequest(new { message = "ShedType creation failed. Fill the form correctly." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the request", error = ex.Message });
            }
        }
        [HttpPut("UpdateShedType")]
        public IActionResult UpdateShedType(ShedTypes shedType)
        {
            try
            {
                bool updateUomSuccessful = shedTypeBusinessLogic.UpdateShedType(shedType);

                if (updateUomSuccessful)
                {
                    return Ok(new { message = "ShedType updated successfully" });
                }
                else
                {
                    return BadRequest(new { message = "ShedType update failed. Fill the form correctly." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the request", error = ex.Message });
            }
        }
        [HttpGet("GetShedType")]
        public IActionResult GetShedType()
        {
            try
            {
                var res = shedTypeBusinessLogic.GetShedType();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the request", error = ex.Message });
            }
        }


        [HttpDelete("DeleteShedType")]
        public IActionResult DeleteShedType(int ShedTypeId)
        {
            try
            {
                shedTypeBusinessLogic.DeleteShedType(ShedTypeId);
                return Ok(new { message = "ShedType deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the request", error = ex.Message });
            }
        }

    }
}
