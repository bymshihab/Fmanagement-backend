using FarmProject.Manager.Module;
using FarmProject.Manager.Uom;
using FarmProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace FarmProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleController : ControllerBase
    {
        private readonly InterfaceModule moduleBusinessLogic;

        public ModuleController(InterfaceModule moduleBusinessLogic)
        {
            this.moduleBusinessLogic = moduleBusinessLogic;
        }

        [HttpPost("CreateModule")]
        public IActionResult CreateModule([FromForm] Moduless module)
        {
            try
            {
                bool createUomSuccessful = moduleBusinessLogic.CreateModule(module);

                if (createUomSuccessful)
                {
                    return Ok(new { message = "Module created successfully" });
                }
                else
                {
                    return BadRequest(new { message = "Module creation failed. Fill the form correctly." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the request", error = ex.Message });
            }
        }
        [HttpPut("UpdateModule")]
        public IActionResult UpdateModuless([FromForm] Moduless moduless)
        {
            try
            {
                bool updateModuleSuccessful = moduleBusinessLogic.UpdateModuless(moduless);

                if (updateModuleSuccessful)
                {
                    return Ok(new { message = "Module updated successfully" });
                }
                else
                {
                    return BadRequest(new { message = "Module update failed. Fill the form correctly." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the request", error = ex.Message });
            }
        }

        [HttpDelete("DeleteModule")]
        public IActionResult DeleteModule([FromForm]int ModuleId)
        {
            try
            {
                moduleBusinessLogic.DeleteModule(ModuleId);
                return Ok(new { message = "module deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the request", error = ex.Message });
            }
        }
        [HttpGet]
        public IActionResult GetModule()
        {
            try
            {
                List<Moduless> modules = new List<Moduless>();
                modules = moduleBusinessLogic.getData();
                if (modules.Count > 0)
                {
                    return Ok(modules);
                }
                else
                {
                    return NotFound(new { message = "No modules found" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the request", error = ex.Message });
            }
        }
    }
}
