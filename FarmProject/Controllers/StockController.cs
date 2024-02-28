using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using FarmProject.ConnectionString;
using FarmProject.Manager.Menu;
using FarmProject.Manager.Stock;

namespace FarmProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly InterfaceStock interfaceStock;

        public StockController(InterfaceStock interfaceStock)
        {
            this.interfaceStock = interfaceStock;
        }
        [HttpGet("GetFeedStock")]
        public IActionResult GetFeedStock(int CompanyId)
        {
            try
            {
                var res = interfaceStock.GetFeedStock(CompanyId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        [HttpGet("GetAnimalSummary")]
        public IActionResult GetAnimalSummary(int CompanyId)
        {
            try
            {
                var res = interfaceStock.GetAnimalSummary(CompanyId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        [HttpGet("GetMedicineStock")]
        public IActionResult GetMedicineStock(int CompanyId)
        {
            try
            {
                var res = interfaceStock.GetMedicineStock(CompanyId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        [HttpGet("GetVaccineStock")]
        public IActionResult GetVaccineStock(int CompanyId)
        {
            try
            {
                var res = interfaceStock.GetVaccineStock(CompanyId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("GetSemenStock")]
        public IActionResult GetSemenStock(int CompanyId)
        {
            try
            {
                var res = interfaceStock.GetSemenStock(CompanyId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
