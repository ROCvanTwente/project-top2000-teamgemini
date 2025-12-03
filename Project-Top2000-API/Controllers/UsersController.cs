using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

public class UsersController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        // Dummy data als placeholder
        var users = new List<object>
        {
            new { id = 1, name = "Alice" },
            new { id = 2, name = "Bob" },
            new { id = 3, name = "Charlie" }
        };

        return Json(users); // JSON teruggeven naar React
    }
}
