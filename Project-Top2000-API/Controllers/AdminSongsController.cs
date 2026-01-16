using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [AllowAnonymous]
        public async Task<IActionResult> UpdateSong(int id, [FromBody] UpdateSongDto dto)
        {
            var song = await _context.Songs.FindAsync(id);
            if (song == null) return NotFound("Liedje niet gevonden");

            if (dto.Titel != null) song.Titel = dto.Titel;
            if (dto.ReleaseYear.HasValue) song.ReleaseYear = dto.ReleaseYear.Value;
            if (dto.ArtistId.HasValue) song.ArtistId = dto.ArtistId.Value;
            if (dto.ImgUrl != null) song.ImgUrl = dto.ImgUrl;
            if (dto.Lyrics != null) song.Lyrics = dto.Lyrics;
            if (dto.Youtube != null) song.Youtube = dto.Youtube;

            await _context.SaveChangesAsync();
            return Ok("Liedje bijgewerkt");
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSong(int id)
        {
            var song = await _context.Songs
                .Where(s => s.SongId == id)
                .Select(s => new {
                    songId = s.SongId,
                    titel = s.Titel ?? null,
                    releaseYear = s.ReleaseYear,
                    artistId = s.Artist != null ? (int?)s.Artist.ArtistId : null,
                    artistName = s.Artist != null ? s.Artist.Name : null,
                    imgUrl = s.ImgUrl ?? null,
                    lyrics = s.Lyrics ?? null,
                    youtube = s.Youtube ?? null
                })
                .FirstOrDefaultAsync();
            if (song == null) return NotFound("Liedje niet gevonden");
            return Ok(song);
        }
    }
}