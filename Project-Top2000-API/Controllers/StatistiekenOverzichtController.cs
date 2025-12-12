using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TemplateJwtProject.Data;
using TemplateJwtProject.Models.DTOs;

namespace TemplateJwtProject.Controllers
{
    [Route("api/[controller]")] // De URL wordt: /api/StatistiekenOverzicht
    [ApiController]
    public class StatistiekenOverzichtController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StatistiekenOverzichtController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("dalers/{jaar}")]
        public async Task<ActionResult<IEnumerable<StatistiekItemDto>>> GetDalers(int jaar)
        {
            var result = await _context.Database
                .SqlQuery<StatistiekItemDto>($"EXEC GetDalers @Jaar = {jaar}")
                .ToListAsync();

            return Ok(result);
        }

        [HttpGet("stijgers/{jaar}")]
        public async Task<ActionResult<IEnumerable<StatistiekItemDto>>> GetStijgers(int jaar)
        {
            var result = await _context.Database
                .SqlQuery<StatistiekItemDto>($"EXEC GetStijgers @Jaar = {jaar}")
                .ToListAsync();

            return Ok(result);
        }
    }
}