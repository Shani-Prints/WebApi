
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IUserServise userService;
        private IJewelryService jewelryService;

        public UserController(IUserServise userService, IJewelryService jewelryService)
        {
            this.userService = userService;
            this.jewelryService = jewelryService;
        }

        private int? UserId
        {
            get
            {
                var idClaim = User.FindFirst("id");
                return idClaim != null && int.TryParse(idClaim.Value, out int userId) ? userId : (int?)null;
            }
        }
        private string? userRole
        {
            get
            {
                var roleClaim = User.FindFirst("type");
                return roleClaim?.Value;
            }
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize(Policy = "Admin")]
        public ActionResult<List<User>> GetAll()
        {
            var users = userService.GetAll();

            if (users == null)
            {
                return NotFound(); // מחזיר סטטוס 404 אם אין משתמשים
            }

            return Ok(users); // מחזיר את רשימת המשתמשים עם סטטוס 200
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize(Policy = "User")]
        public ActionResult<User> Get()
        {
            // var user = userService.Get((int)UserId);
            var user = userService.Get(UserId.GetValueOrDefault());
            if (user == null)
                return NotFound();
            return user;
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize(Policy = "Admin")]
        public IActionResult Create(User user)
        {
            userService.Add(user);
            return CreatedAtAction(nameof(Create), new { id = user.Id }, user);
        }

        [HttpPut]
        [Authorize(Policy = "User")]
        public ActionResult Update(User user)
        {
            if (userRole != "Admin" && UserId != user.Id)
                return Unauthorized();
            var exitingUser = userService.Get(UserId.GetValueOrDefault());
            if (exitingUser is null)
                return NotFound();
            userService.Update(user, user?.Role ?? "User");
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        public ActionResult Delete(int id)
        {
            var user = userService.Get(id);
            if (user is null)
                return NotFound();
            jewelryService.DeleteJewelryByUserId(id);
            userService.Delete(id);
            return Content(userService.Count.ToString());
        }

        [HttpPost]
        [Route("[action]")]
        public ActionResult<String> Login([FromBody] User user)
        {
            var result = userService.Login(user);
            if (result == null)
                return Unauthorized();
            return result;
        }
    }
}