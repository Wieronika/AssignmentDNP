using Entities;
using Microsoft.EntityFrameworkCore;

namespace EfcRepositories;

public class AppContext : DbContext
{
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Comment> Comments => Set<Comment>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string projectDirectory = Directory.GetCurrentDirectory();

        if (Path.GetFileName(projectDirectory) == "WebAPI")
        {
            projectDirectory = Path.Combine(
                projectDirectory, "..", "EfcRepositories");
        }

        string databasePath = Path.GetFullPath(
            Path.Combine(projectDirectory, "app.db"));

        optionsBuilder.UseSqlite($"Data Source={databasePath}");
    }
}