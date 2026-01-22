using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TemplateJwtProject.Models;

namespace TemplateJwtProject.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Songs> Songs { get; set; } = null!;
    public DbSet<Artist> Artists { get; set; } = null!;
    public DbSet<Top2000Entry> Top2000Entries { get; set; } = null!;
    public DbSet<Playlist> Playlists { get; set; } = null!;

    public DbSet<PlaylistSong> PlaylistSongs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>()
            .HasMany(u => u.PlayLists)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);


        builder.Entity<RefreshToken>()
            .HasOne(rt => rt.User)
            .WithMany()
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<RefreshToken>()
            .HasIndex(rt => rt.Token)
            .IsUnique();


        builder.Entity<PlaylistSong>()
            .HasKey(ps => new { ps.PlayListId, ps.SongId });

        builder.Entity<PlaylistSong>()
            .HasOne(ps => ps.PlayList)
            .WithMany(p => p.PlayListSongs)
            .HasForeignKey(ps => ps.PlayListId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<PlaylistSong>()
            .HasOne(ps => ps.Song)
            .WithMany(s => s.PlayListSongs)
            .HasForeignKey(ps => ps.SongId)
            .OnDelete(DeleteBehavior.Cascade);


        builder.Entity<Songs>()
            .HasOne(s => s.Artist)
            .WithMany(a => a.Songs)
            .HasForeignKey(s => s.ArtistId)
            .HasConstraintName("FK_Songs_Artist_ArtistId")
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Entity<Top2000Entry>()
            .HasKey(e => new { e.SongId, e.Year });

        builder.Entity<Artist>().ToTable("Artist");
        builder.Entity<Songs>().ToTable("Songs");
        builder.Entity<Top2000Entry>().ToTable("Top2000Entry");

    }
}