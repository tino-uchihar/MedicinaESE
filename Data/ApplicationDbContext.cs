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

        public DbSet<Usuario>  Usuarios  { get; set; } = default!;
        public DbSet<Medico>   Medicos   { get; set; } = default!;
        public DbSet<Paciente> Pacientes { get; set; } = default!;
        public DbSet<Noticia>  Noticias  { get; set; }
        public DbSet<HorarioGeneral>    HorariosGenerales  { get; set; }
        public DbSet<AsignacionHorario> AsignacionHorarios { get; set; }
        public DbSet<ExcepcionHorario>  ExcepcionesHorario { get; set; }
        public DbSet<Cita>              Citas              { get; set; }
        public DbSet<HistorialClinico>  HistorialClinico   { get; set; }


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

            // EstadoCita
            builder.Entity<Cita>()
            .Property(c => c.EstadoCita)
            .HasDefaultValue("Pendiente");

            builder.Entity<Cita>()
                        .HasMany(c => c.Historiales)
                        .WithOne(h => h.Cita)
                        .HasForeignKey(h => h.IdCita)
                        .OnDelete(DeleteBehavior.Cascade);

            //AsignacionHorario
            builder.Entity<AsignacionHorario>()
                .HasKey(a => a.IdAsignacion);

            builder.Entity<HorarioGeneral>()
                .HasKey(h => h.IdHorarioGeneral);

            builder.Entity<ExcepcionHorario>()
                .HasKey(e => e.IdExcepcion);

            builder.Entity<Cita>()
                .HasKey(c => c.IdCita);

            builder.Entity<HistorialClinico>()
                .HasKey(hc => hc.IdHistorial);


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
