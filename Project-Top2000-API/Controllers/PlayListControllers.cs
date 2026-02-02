using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TemplateJwtProject.Data;
using TemplateJwtProject.Models;
using TemplateJwtProject.Models.DTOs;

namespace TemplateJwtProject.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PlaylistController : ControllerBase
    {
        private readonly AppDbContext _context;


        public PlaylistController(AppDbContext context)
        {
            _context = context;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        }


        // =========================
        // CREATE PLAYLIST
        // =========================
        [HttpPost]
        public async Task<IActionResult> CreatePlaylist(CreatePlaylistDto dto)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized(); 

            var playlist = new Playlist
            {
                Name = dto.Name,
                UserId = userId
            };
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();

            return Ok(new { playlist.Id, playlist.Name });
        }



        // =========================
        // GET MY PLAYLISTS
        // =========================
        [HttpGet]
        public async Task<IActionResult> GetPlaylists()
        {
            var userId = GetUserId();

            var playlists = await _context.Playlists
                .Where(p => p.UserId == userId)
                .Select(p => new PlaylistResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Songs = p.PlayListSongs.Select(ps => new PlaylistSongDto
                    {
                        SongId = ps.SongId,
                        Titel = ps.Song.Titel,
                        Artist = ps.Song.Artist.Name
                    }).ToList()
                })
                .ToListAsync();

            return Ok(playlists);
        }



        // =========================
        // ADD SONG TO PLAYLIST
        // =========================
        [HttpPost("{playlistId}/songs/{songId}")]
        public async Task<IActionResult> AddSongToPlaylist(int playlistId, int songId)
        {
            var userId = GetUserId();

            var playlist = await _context.Playlists
                .FirstOrDefaultAsync(p => p.Id == playlistId && p.UserId == userId);

            if (playlist == null)
                return NotFound("Playlist not found");

            var songExists = await _context.Songs.AnyAsync(s => s.SongId == songId);
            if (!songExists)
                return NotFound("Song not found");

            var alreadyExists = await _context.PlaylistSongs.AnyAsync(ps =>
                ps.PlayListId == playlistId && ps.SongId == songId);

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


        // =========================
        // REMOVE SONG FROM PLAYLIST
        // =========================
        [HttpDelete("{playlistId}/songs/{songId}")]
        public async Task<IActionResult> RemoveSongFromPlaylist(int playlistId, int songId)
        {
            var userId = GetUserId();

            var playlist = await _context.Playlists
                .FirstOrDefaultAsync(p => p.Id == playlistId && p.UserId == userId);

            if (playlist == null)
                return NotFound();

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

        // =========================
        // DELETE PLAYLIST
        // =========================
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlaylist(int id)
        {
            var userId = GetUserId();

            var playlist = await _context.Playlists
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (playlist == null)
                return NotFound();

            _context.Playlists.Remove(playlist);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
