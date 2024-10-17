using Microsoft.EntityFrameworkCore;

/// <summary>
/// Represents the application's database context.
/// </summary>
public class AppDbContext : DbContext
{
    // Define DbSets
    /// <summary>
    /// Gets or sets the Contacts DbSet.
    /// </summary>
    public DbSet<Contact> Contacts { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AppDbContext"/> class using the specified options.
    /// </summary>
    /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}