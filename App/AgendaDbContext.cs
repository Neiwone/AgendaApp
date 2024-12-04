using App.models;
using Microsoft.EntityFrameworkCore;

public class AgendaDbContext : DbContext
{
    public DbSet<Pacient> Pacients { get; set; }
    public DbSet<Appointment> Appointments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        string host = "localhost";
        string port = "";
        string username = "admin";
        string password = "1234";
        string database = "agendaapp";
        string connectionString = $"Host={host};Username={username};Password={password};Database={database}";

        optionsBuilder.UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
