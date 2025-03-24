using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ReserveBiteee.Models;
using ReserveBiteee.Service;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ReserveBiteee.Controllers
{

    public class AccountController : Controller
    {
        private readonly IRegisterService _registerService;
        private readonly ILoginService _loginService;

        public AccountController(IRegisterService registerService,ILoginService loginService)
        {
            _registerService = registerService;
            _loginService = loginService;
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RegisterUser([FromBody] Register registerModel)
        {
            try
            {
                if (registerModel == null)
                {
                    return BadRequest("Invalid data received.");
                }

                if (registerModel.Password != registerModel.ConfirmPassword)
                {
                    return BadRequest("Password and Confirm Password do not match.");
                }

                int isRegistered = _registerService.Register(registerModel);

                if (isRegistered > 0 || isRegistered == 0)
                {
                    return Ok("Registration successful!");
                }
                else
                {
                    return BadRequest("Registration failed.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        [HttpPost]
        public IActionResult FindUser([FromBody] Login login)
        {
            try
            {
                if (login == null || string.IsNullOrEmpty(login.UserName) || string.IsNullOrEmpty(login.Password))
                {
                    return BadRequest(new { message = "Invalid login details." });
                }

                if (login.UserName.Trim() == "Admin" && login.Password.Trim() == "Admin")
                {
                    return Ok(new
                    {
                        message = "Login successful",
                        redirectUrl = Url.Action("AdminIndex", "Admin")
                    });
                }

                bool isUserValid = _loginService.FindUser(login.UserName, login.Password);

                if (isUserValid)
                {
                    Console.WriteLine("User found! Redirecting...");
                    return Ok(new
                    {
                        message = "Login successful",
                        redirectUrl = Url.Action("CustomerHome", "Customer")
                    });
                }
                else
                {
                    Console.WriteLine("User not found.");
                    return NotFound(new { message = "User not found." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }



    }
}
