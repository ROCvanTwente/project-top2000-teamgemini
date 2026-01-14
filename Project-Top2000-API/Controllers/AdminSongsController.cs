using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateJwtProject.Data;
using TemplateJwtProject.Models.DTOs;

namespace TemplateJwtProject.Controllers
{
    [ApiController]
    [Route("admin/songs")]
    public class AdminSongsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AdminSongsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSong(int id, [FromBody] UpdateSongDto dto)
        {
            if (dto == null)
                return BadRequest("Ongeldige data");

            var song = await _context.Songs.FindAsync(id);
            if (song == null)
                return NotFound("Liedje niet gevonden");

            if (dto.ArtistId.HasValue) song.ArtistId = dto.ArtistId.Value;
            if (!string.IsNullOrEmpty(dto.Titel)) song.Titel = dto.Titel;
            if (dto.ReleaseYear.HasValue) song.ReleaseYear = dto.ReleaseYear;
            if (!string.IsNullOrEmpty(dto.ImgUrl)) song.ImgUrl = dto.ImgUrl;
            if (!string.IsNullOrEmpty(dto.Lyrics)) song.Lyrics = dto.Lyrics;
            if (!string.IsNullOrEmpty(dto.Youtube)) song.Youtube = dto.Youtube;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Liedje succesvol bijgewerkt" });
        }
    }
}
