using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MedicinaESE.Models;
using System.IO;

namespace MedicinaESE.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Usuario>  Usuarios  { get; set; }
        public DbSet<Medico>   Medicos   { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Noticia>  Noticias  { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Usuarios
            builder.Entity<Usuario>()
                .HasKey(u => u.IdUsuario);
            builder.Entity<Usuario>()
                .HasIndex(u => u.DocumentoId).IsUnique();
            builder.Entity<Usuario>()
                .HasIndex(u => u.Correo).IsUnique();

            // Médicos
            builder.Entity<Medico>()
                .HasKey(m => m.IdMedico);

            // Pacientes
            builder.Entity<Paciente>()
                .HasKey(p => p.IdPaciente);

            // Noticias
            builder.Entity<Noticia>()
                .HasKey(n => n.Id);
        }


        // Este método permite a 'dotnet ef' conocer la cadena de conexión
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                var cs = config.GetConnectionString("DoctorNowDB");
                optionsBuilder.UseSqlServer(cs);
            }
        }
    }
}
