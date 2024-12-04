using App.models;
using Microsoft.EntityFrameworkCore;

public class ConsultorioContext : DbContext
{
    public DbSet<Pacient> Pacients { get; set; }
    public DbSet<Appointment> Appointments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Database=AgendaDB;Username=vinicius;Password=123456");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       
    }
}
