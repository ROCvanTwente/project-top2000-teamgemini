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

        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchSongs([FromQuery] int? id, [FromQuery] string? titel)
        {
            var query = _context.Songs.AsQueryable();

            if (id.HasValue)
                query = query.Where(s => s.SongId == id.Value);

            if (!string.IsNullOrWhiteSpace(titel))
                query = query.Where(s => s.Titel!.Contains(titel));

            var songs = await query
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
                .ToListAsync();

            if (!songs.Any()) return NotFound("Geen liedjes gevonden");

            return Ok(songs);
        }

    }
}