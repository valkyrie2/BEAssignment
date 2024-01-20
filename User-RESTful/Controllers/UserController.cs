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
        [HttpGet("/{id}")]
        public async Task<User> UserById(long id)
        {
            try
            {
                var users = await _userService.GetById(id);
                return users;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting users");
                Console.WriteLine(e);
                throw;
            }
        }
        [HttpGet("/Name/{name}")]
        public async Task<User> UserByName(string name)
        {
            try
            {
                var users = await _userService.GetByName(name);
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

        [HttpPut("/{id}")]
        public async Task<IActionResult> UpdateUser(long id, UserUpdateRequest model)
        {
            try
            {
                await _userService.Update(id, model);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error updating users");
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
