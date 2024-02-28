using FarmProject.Manager.Menu;
using FarmProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FarmProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {

        private readonly InterfaceMenu interfaceMenu;

        public MenuController(InterfaceMenu interfaceMenu)
        {
            this.interfaceMenu = interfaceMenu;
        }


        [HttpPost]
        public IActionResult InsertMenu([FromForm] Menus menu)
        {
            try
            {
                var res = interfaceMenu.InsertMenu(menu);
                if (res > 0)
                {
                    return Ok(new { message = "Menu inserted successfully" });
                }
                else
                {
                    return Ok(new { message = "Menu isn't inserted successfully" });
                }


            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the request", error = ex.Message });
            }
        }


        [HttpPut("UpdateMenu")]
        public IActionResult UpdateMenu([FromForm] Menus updatedMenu)
        {
            try
            {
                int rowsAffected = interfaceMenu.UpdateMenu(updatedMenu);

                if (rowsAffected > 0)
                {
                    return Ok(new { message = "Menu updated successfully" });
                }
                else
                {
                    return NotFound(new { message = "Menu not found" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the request", error = ex.Message });
            }
        }


        [HttpDelete("{menuId}")]
        public IActionResult DeleteMenu(int menuId)
        {
            try
            {

                var rowsAffected = interfaceMenu.DeleteMenu(menuId);
                if (rowsAffected > 0)
                {
                    return Ok(new { message = "Menu deleted successfully" });
                }
                else
                {
                    return NotFound(new { message = "Menu not found" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the request", error = ex.Message });
            }
        }

        [HttpGet("GetSpAllMenu")]
        public IActionResult GetSpAllMenu()
        {
            try
            {
                var res = interfaceMenu.GetSpAllMenu();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        [HttpGet("GetAllMenu")]
        public IActionResult GetAllMenu()
        {
            try
            {
                var res = interfaceMenu.GetAllMenu();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
