using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Project_Top2000_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        public sealed record UserDto(int Id, string Name);

        [HttpGet]
        public ActionResult<IEnumerable<UserDto>> Get()
        {
            var users = new List<UserDto>
            {
                new(1, "Alice"),
                new(2, "Bob"),
                new(3, "Charlie")
            };

            return Ok(users);
        }
    }
}
