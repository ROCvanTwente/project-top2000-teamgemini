using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TemplateJwtProject.Data;
using TemplateJwtProject.Models;
using TemplateJwtProject.Models.DTOs;

namespace TemplateJwtProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaylistController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PlaylistController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlaylist(CreatePlaylistDto dto)
        {
            var playlist = new Playlist
            {
                Name = dto.Name,
                UserId = null // later moet het worden: User.FindFirstValue(ClaimTypes.NameIdentifier) (eerste inloggen klaar maken)
            };

            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();

            return Ok(playlist);
        }

        [HttpGet]
        public async Task<IActionResult> GetPlaylists()
        {
            var playlists = await _context.Playlists
                .Include(p => p.PlayListSongs)
                    .ThenInclude(ps => ps.Song)
                .ToListAsync();

            return Ok(playlists);
        }

        [HttpPost("{playlistId}/songs")]
        public async Task<IActionResult> AddSongToPlaylist(
            int playlistId,
            AddSongToPlaylistDto dto)
        {
            var playlistExists = await _context.Playlists
                .AnyAsync(p => p.Id == playlistId);

            if (!playlistExists)
                return NotFound("Playlist not found");

            var songExists = await _context.Songs
                .AnyAsync(s => s.SongId == dto.SongId);

            if (!songExists)
                return NotFound("Song not found");

            var alreadyExists = await _context.PlaylistSongs
                .AnyAsync(ps =>
                    ps.PlayListId == playlistId &&
                    ps.SongId == dto.SongId);

            if (alreadyExists)
                return BadRequest("Song already in playlist");

            var playlistSong = new PlaylistSong
            {
                PlayListId = playlistId,
                SongId = dto.SongId
            };

            _context.PlaylistSongs.Add(playlistSong);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlaylist(int id)
        {
            var playlist = await _context.Playlists.FindAsync(id);

            if (playlist == null)
                return NotFound();

            _context.Playlists.Remove(playlist);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
