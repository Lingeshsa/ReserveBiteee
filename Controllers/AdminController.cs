using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ReserveBiteee.Models;
using ReserveBiteee.Service;
using System.Collections.Generic;

namespace ReserveBiteee.Controllers
{
    public class AdminController : Controller
    {

        private readonly IMenuService _menuService;
        public AdminController(IMenuService menuService)
        {
            this._menuService = menuService;
        }
        public IActionResult AdminIndex()
        {
            return View();
        }
        public IActionResult ImageUploadPage()
        {
            return View();
        }
        public IActionResult MenuIndex()
        {
            return View();
        }
        public IActionResult AddMenu()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddMenu([FromBody] MenuModel menu)
        {
            try
            {
                if (menu == null || string.IsNullOrEmpty(menu.MenuItem) )
                {
                    return BadRequest(new { message = "Invalid menu name." });
                }

                int menuId = _menuService.AddMenu(menu);

                if (menuId > 0)
                {
                    return Ok(new { message = "Menu added successfully!", menuId = menuId });
                }
                else
                {
                    return BadRequest(new { message = "Failed to add menu." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }
        [HttpGet]
        public IActionResult GetMenus()
        {
            try
            {
                var menus = _menuService.GetMenus();
                return Ok(menus);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }
        [HttpPost]
        public IActionResult UpdateMenu([FromBody] MenuModel menu)
        {
            try
            {
                if (menu == null || string.IsNullOrEmpty(menu.MenuItem))
                    return BadRequest(new { message = "Invalid menu data." });

                int result = _menuService.UpdateMenu(menu);
                if (result > 0 || result == 0)
                    return Ok(new { message = "Menu updated successfully!" });
                else
                    return BadRequest(new { message = "Failed to update menu." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteMenu(int id)
            {
                try
                {
                    int isDeleted = _menuService.DeleteMenu(id);

                    if (isDeleted > 0)
                    {
                        return Ok(new { message = "Menu deleted successfully!" });
                    }
                    else
                    {
                        return BadRequest(new { message = "Failed to delete menu." });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
                }
            }

    }
}
