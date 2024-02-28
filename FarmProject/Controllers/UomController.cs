using FarmProject.Manager.Uom;
using FarmProject.Manager.User;
using FarmProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;

namespace FarmProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UomController : ControllerBase
    {
        private readonly InterfaceUom uomBusinessLogic;

        public UomController(InterfaceUom uomBusinessLogic)
        {
            this.uomBusinessLogic = uomBusinessLogic;
        }


        [HttpPost("CreateUom")]
        public IActionResult CreateUom([FromBody] Uom uom)
        {
            try
            {
                bool createUomSuccessful = uomBusinessLogic.CreateUom(uom);

                if (createUomSuccessful)
                {
                    return Ok(new { message = "Unit created successfully" });
                }
                else
                {
                    return BadRequest(new { message = "Unit creation failed. Fill the form correctly." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the request", error = ex.Message });
            }
        }


        [HttpPut("UpdateUom")]
        public IActionResult UpdateUom([FromBody] Uom uom)
        {
            try
            {
                bool updateUomSuccessful = uomBusinessLogic.UpdateUom(uom);

                if (updateUomSuccessful)
                {
                    return Ok(new { message = "Unit updated successfully" });
                }
                else
                {
                    return BadRequest(new { message = "Unit update failed. Fill the form correctly." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the request", error = ex.Message });
            }
        }


        [HttpGet("GetUoms")]
        public IActionResult GetUoms(int CompanyId)
        {
            try
            {
                var res = uomBusinessLogic.GetUoms(CompanyId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the request", error = ex.Message });
            }
        }


        [HttpDelete("DeleteUom")]
        public IActionResult DeleteUom(int UomId)
        {
            try
            {
                uomBusinessLogic.DeleteUom(UomId);
                return Ok(new { message = "Unit deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the request", error = ex.Message });
            }
        }
    }
}
