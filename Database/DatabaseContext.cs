using Microsoft.EntityFrameworkCore;
using FormGeneratorAPI.Authentication;
using FormGeneratorAPI.Authentication.Entities;
using FormGeneratorAPI.Entities;

namespace FormGeneratorAPI.Database;

public class DatabaseContext:DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Form> Forms { get; set; }
    public DbSet<FormElement> FormElements { get; set; }
    public DbSet<FormElementValue> Answers { get; set; }
    public DatabaseContext(DbContextOptions<DatabaseContext> options):base(options)
    {
        
    }
}