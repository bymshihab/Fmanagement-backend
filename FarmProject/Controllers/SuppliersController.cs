using FarmProject.Manager.Supplier;
using FarmProject.Manager.Uom;
using FarmProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace FarmProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {

        private readonly InterfaceSupplier supplierBusinessLogic;

        public SuppliersController(InterfaceSupplier supplierBusinessLogic)
        {
            this.supplierBusinessLogic = supplierBusinessLogic;
        }

        [HttpPost("CreateUom")]
        public IActionResult CreateSupplier([FromBody] Suppliers suppliers)
        {
            try
            {
                bool createSupplierSuccessful = supplierBusinessLogic.CreateSupplier(suppliers);

                if (createSupplierSuccessful)
                {
                    return Ok(new { message = "supplier Added successfully" });
                }
                else
                {
                    return BadRequest(new { message = "supplier Added failed. Fill the form correctly." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the request", error = ex.Message });
            }
        }


        [HttpPut("UpdateSupplier")]
        public IActionResult UpdateSupplier([FromBody] Suppliers suppliers)
        {
            try
            {
                bool updateUomSuccessful = supplierBusinessLogic.UpdateSupplier(suppliers);

                if (updateUomSuccessful)
                {
                    return Ok(new { message = "supplier info updated successfully" });
                }
                else
                {
                    return BadRequest(new { message = "supplier info  update failed. Fill the form correctly." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the request", error = ex.Message });
            }
        }

        [HttpDelete("DeleteSupplier")]
        public IActionResult DeleteSupplier(int SupplierId)
        {
            try
            {
                supplierBusinessLogic.DeleteSupplier(SupplierId);
                return Ok(new { message = "Supplier Info deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the request", error = ex.Message });
            }
        }


        [HttpGet("GetSupplier")]
        public IActionResult GetSupplier(int CompanyId)
        {
            try
            {
                var res = supplierBusinessLogic.GetSupplier(CompanyId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the request", error = ex.Message });
            }
        }

    }
}
