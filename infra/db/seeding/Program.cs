using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Server.Domain.Entities;
using Server.Infrastructure.Persistence;

class Program
{
    static async Task Main(string[] args)
    {
        // moves up from bin/Debug/net8.0 to infra\db\seeding
        var seederProjectDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../"));

        // infra\db\seeding -> up 3  -> root, then apps/server/Server.API
        var apiPath = Path.Combine(seederProjectDir, "../../../apps/server/Server.API");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(apiPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        // Get connection string
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            Console.WriteLine("No connection string found. Check appsettings or environment variables.");
            return;
        }

        // Build DI
        var services = new ServiceCollection();
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        using var provider = services.BuildServiceProvider();
        var db = provider.GetRequiredService<ApplicationDbContext>();

        // Apply migrations
        await db.Database.MigrateAsync();

        // Seed roles
        if (!await db.Roles.AnyAsync())
        {
            db.Roles.Add(Role.Create("Candidate", "Views job openings, uploads CVs, and submits documents."));
            db.Roles.Add(Role.Create("Recruiter", "Manages job openings, candidate profiles, interviews"));
            db.Roles.Add(Role.Create("HR", "Culture fit, final negotiation, documentation and background verification"));
            db.Roles.Add(Role.Create("Interviewer", "Provides interview feedback"));
            db.Roles.Add(Role.Create("Reviewer", "Screens CVs and shortlists candidates"));
            db.Roles.Add(Role.Create("Admin", "Manages users, roles, and system-wide configurations"));
            db.Roles.Add(Role.Create("Viewer", "Read-only access to all data."));

            await db.SaveChangesAsync();
            Console.WriteLine("Roles seeded successfully.");
        }
        else
        {
            Console.WriteLine("Roles already exist. Skipping.");
        }
    }
}
