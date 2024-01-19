using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using BE.Model;
using BE.Services;
using BE.DataAccess;

namespace User_RESTful.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IEnumerable<User>> UserList()
        {
            try
            {
                var users = await _userService.GetList();
                return users;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting users");
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserCreateRequest model)
        {
            try
            {
                await _userService.Create(model);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating users");
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
