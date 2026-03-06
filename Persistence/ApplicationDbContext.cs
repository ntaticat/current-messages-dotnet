using Application.Common.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.Identity;

namespace Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
    {
    }
    
    public DbSet<Chat> Chats { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<QuickMessage> QuickMessages { get; set; }
    public DbSet<ChatParticipant> ChatParticipants { get; set; }
    public DbSet<ChatKeyDistribution> ChatKeyDistributions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<ChatParticipant>()
            .HasKey(p => new { p.ChatId, p.UserId });

        modelBuilder.Entity<ChatKeyDistribution>(entity =>
        {
            entity.HasKey(ck => new { ck.ChatId, ck.UserId });
            
            entity.HasOne(ck => ck.Chat)
                .WithMany(c => c.KeyDistributions)
                .HasForeignKey(ck => ck.ChatId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ck => ck.User)
                .WithMany(u => u.KeyDistributions)
                .HasForeignKey(ck => ck.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

    }
}