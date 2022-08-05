using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class CurrentMessagesNetContext : DbContext
{
    public DbSet<Chat> Chats { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<CurrentMessage> CurrentMessages { get; set; }

    public CurrentMessagesNetContext(DbContextOptions<CurrentMessagesNetContext> options): base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}