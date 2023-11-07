using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;

namespace Test_task
{
    [Route("api/users")]
    [ApiController]
    public class UserController: ControllerBase
    {
        private readonly ApplicationContext _appContext;
        public UserController(ApplicationContext appContext)
        {
            _appContext = appContext;
        }
        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _appContext.Users.ToList();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var user = _appContext.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound(); // 404 Not Found
            }

            return Ok(user);
        }
        [HttpPost]
        public IActionResult CreateUser([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest(); // 400 Bad Request
            }

            _appContext.Users.Add(user);
            _appContext.SaveChanges();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User user)
        {
            var existingUser = _appContext.Users.FirstOrDefault(u => u.Id == id);

            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.Name = user.Name;
            existingUser.Phone = user.Phone;
            existingUser.Email = user.Email;

            _appContext.SaveChanges();

            return NoContent(); // 204 No Content
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _appContext.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            _appContext.Users.Remove(user);
            _appContext.SaveChanges();

            return NoContent();
        }
    }
}
