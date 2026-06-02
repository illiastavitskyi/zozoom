using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using ZoZoom.Models;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Meeting> Meetings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Meeting>()
            .Property(m => m.ParticipantIds)
            .HasConversion(
                v => JsonSerializer.Serialize(v ?? new List<string>(), (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<List<string>>(v ?? "[]", (JsonSerializerOptions)null)
            );
    }
}