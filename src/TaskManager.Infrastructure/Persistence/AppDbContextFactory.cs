using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TaskManager.Infrastructure.Persistence;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var connectionString = "server=localhost;port=3306;database=taskmanagerdb;user=root;password=yourpassword";

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        optionsBuilder.UseMySql(
            connectionString,
            ServerVersion.AutoDetect(connectionString)
        );

        return new AppDbContext(optionsBuilder.Options);
    }
}