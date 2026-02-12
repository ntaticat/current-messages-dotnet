using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<Chat> Chats { get; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<QuickMessage> QuickMessages { get; set; }
    public DbSet<ChatParticipant> ChatParticipants { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}