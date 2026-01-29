using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TemplateJwtProject.Data;
using TemplateJwtProject.Models.DTOs;
using TemplateJwtProject.Models;

namespace TemplateJwtProject.Controllers
{
    [ApiController]
    [Route("admin/artists")]
    public class AdminArtistsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminArtistsController(AppDbContext context)
        {
            _context = context;
        }

        //[Authorize(Roles = "admin")]
        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateArtist(int id, [FromBody] UpdateArtistDto dto)
        {
            var artist = await _context.Artists.FindAsync(id);
            if (artist == null) return NotFound("Artist niet gevonden");

            if (dto.Name != null) artist.Name = dto.Name;
            if (dto.Wiki != null) artist.Wiki = dto.Wiki;
            if (dto.Biography != null) artist.Biography = dto.Biography;
            if (dto.Photo != null) artist.Photo = dto.Photo;

            await _context.SaveChangesAsync();
            return Ok("Artist bijgewerkt");
        }

        //[Authorize(Roles = "admin")]
        [AllowAnonymous]
        [HttpGet("search")]
        public async Task<IActionResult> SearchArtists([FromQuery] int? id, [FromQuery] string? name)
        {
            var query = _context.Artists.AsQueryable();

            if (id.HasValue)
                query = query.Where(a => a.ArtistId == id.Value);

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(a => a.Name.Contains(name));

            var artists = await query
                .Select(a => new
                {
                    artistId = a.ArtistId,
                    name = a.Name ?? null,
                    wiki = a.Wiki ?? null,
                    biography = a.Biography ?? null,
                    photo = a.Photo ?? null
                })
                .ToListAsync();

            if (!artists.Any()) return NotFound("Geen artists gevonden");

            return Ok(artists);
        }
    }
}
