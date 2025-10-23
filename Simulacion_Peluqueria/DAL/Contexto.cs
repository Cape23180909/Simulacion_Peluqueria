using Microsoft.EntityFrameworkCore;
using Simulacion_Peluqueria.Models;

namespace Simulacion_Peluqueria.DAL;

public class Contexto : DbContext
{
    public Contexto(DbContextOptions<Contexto> options)
          : base(options) { }

    public DbSet<Configuracion> Configuraciones { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<EventoSimulacion> EventosSimulacion { get; set; }
    public DbSet<Indicadores> Indicadores { get; set; }

    public DbSet<HistorialSimulacion> HistorialSimulaciones { get; set; }

    public DbSet<ResultadoSimulacion> ResultadosSimulacion { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurar relaciones
        modelBuilder.Entity<HistorialSimulacion>()
            .HasOne(h => h.Configuracion)
            .WithMany()
            .HasForeignKey(h => h.ConfiguracionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<HistorialSimulacion>()
            .HasOne(h => h.Resultado)
            .WithMany()
            .HasForeignKey(h => h.ResultadoId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ResultadoSimulacion>()
            .HasMany(r => r.Eventos)
            .WithOne(e => e.ResultadoSimulacion)
            .HasForeignKey(e => e.ResultadoSimulacionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ResultadoSimulacion>()
            .HasOne(r => r.Indicadores)
            .WithOne()
            .HasForeignKey<Indicadores>(i => i.Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}