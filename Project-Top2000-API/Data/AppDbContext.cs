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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<RefreshToken>()
            .HasOne(rt => rt.User)
            .WithMany()
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<RefreshToken>()
            .HasIndex(rt => rt.Token)
            .IsUnique();

        builder.Entity<Top2000Entry>()
            .HasKey(e => new { e.SongId, e.Year });
    }
}