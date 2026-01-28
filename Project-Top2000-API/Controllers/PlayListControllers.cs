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

        // ============================
        // CREATE PLAYLIST
        // ============================
        [HttpPost]
        public async Task<IActionResult> CreatePlaylist(CreatePlaylistDto dto)
        {
            var playlist = new Playlist
            {
                Name = dto.Name,
                UserId = null // later: User.FindFirstValue(ClaimTypes.NameIdentifier)
            };

            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                id = playlist.Id,
                name = playlist.Name
            });
        }

        // ============================
        // GET ALL PLAYLISTS (DTO)
        // ============================
        [HttpGet]
        public async Task<IActionResult> GetPlaylists()
        {
            var playlists = await _context.Playlists
                .Include(p => p.PlayListSongs)
                    .ThenInclude(ps => ps.Song)
                .Select(p => new
                {
                    id = p.Id,
                    name = p.Name,
                    songs = p.PlayListSongs.Select(ps => new
                    {
                        songId = ps.SongId,
                        title = ps.Song.Titel,
                        artist = ps.Song.Artist != null
                            ? ps.Song.Artist.Name
                            : "Onbekend"
                    }).ToList()
                })
                .ToListAsync();

            return Ok(playlists);
        }

        // ============================
        // ADD SONG TO PLAYLIST
        // ============================
        [HttpPost("{playlistId}/songs/{songId}")]
        public async Task<IActionResult> AddSongToPlaylist(
            int playlistId,
            int songId)
        {
            var playlistExists = await _context.Playlists
                .AnyAsync(p => p.Id == playlistId);

            if (!playlistExists)
                return NotFound("Playlist not found");

            var songExists = await _context.Songs
                .AnyAsync(s => s.SongId == songId);

            if (!songExists)
                return NotFound("Song not found");

            var alreadyExists = await _context.PlaylistSongs
                .AnyAsync(ps =>
                    ps.PlayListId == playlistId &&
                    ps.SongId == songId);

            if (alreadyExists)
                return Conflict("Song already in playlist");

            _context.PlaylistSongs.Add(new PlaylistSong
            {
                PlayListId = playlistId,
                SongId = songId
            });

            await _context.SaveChangesAsync();
            return Ok();
        }

        // ============================
        // REMOVE SONG FROM PLAYLIST
        // ============================
        [HttpDelete("{playlistId}/songs/{songId}")]
        public async Task<IActionResult> RemoveSongFromPlaylist(
            int playlistId,
            int songId)
        {
            var playlistSong = await _context.PlaylistSongs
                .FirstOrDefaultAsync(ps =>
                    ps.PlayListId == playlistId &&
                    ps.SongId == songId);

            if (playlistSong == null)
                return NotFound();

            _context.PlaylistSongs.Remove(playlistSong);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ============================
        // DELETE PLAYLIST
        // ============================
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlaylist(int id)
        {
            var playlist = await _context.Playlists
                .Include(p => p.PlayListSongs)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (playlist == null)
                return NotFound();

            _context.Playlists.Remove(playlist);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}