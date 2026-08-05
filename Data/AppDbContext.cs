using Microsoft.EntityFrameworkCore;
using NoteApi.Entity;

namespace NoteApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Note> Notes { get; set; }
    }
}
