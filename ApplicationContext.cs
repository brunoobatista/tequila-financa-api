using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tequila.Models;

namespace Tequila
{

    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            
        }

        public static readonly ILoggerFactory MyLoggerFactory
                = LoggerFactory.Create(builder => { builder.AddConsole(); });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(MyLoggerFactory)  //tie-up DbContext with LoggerFactory object
                .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Usuario>()
                    .HasOne(u => u.Endereco)
                    .WithOne()
                    .HasForeignKey<Usuario>(u => u.EnderecoId);

            builder.Entity<Carteira>()
                .HasOne(c => c.Status)
                .WithMany();

            builder.Entity<DespesasFixas>()
                .HasOne(d => d.Usuario)
                .WithMany();

            builder.Entity<RendaAdicional>()
                .HasOne(ra => ra.Carteira)
                .WithMany();
            builder.Entity<RendaAdicional>()
                .HasOne(ra => ra.Usuario)
                .WithMany();
            
            builder.Entity<DespesaVariavel>()
                .HasOne(dv => dv.Carteira)
                .WithMany();
            
            builder.Entity<DespesaFixa>()
                .HasOne(df => df.Carteira)
                .WithMany();
            
            builder.Entity<DespesaFixa>()
                .HasOne(df => df.DespesasFixas)
                .WithMany();
            
            builder.Entity<DespesasFixas>()
                .HasMany(df => df.ListaDespesasFixas)
                .WithOne(d => d.DespesasFixas);
        }
        
        public DbSet<RendaAdicional> RendaAdicional { get; set; }
        public DbSet<DespesaVariavel> DespesaVariavel { get; set; }
        public DbSet<DespesasFixas> DespesasFixas { get; set; }
        public DbSet<DespesaFixa> DespesaFixa { get; set; }
        public DbSet<Endereco> Endereco { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Carteira> Carteira { get; set; }
        public DbSet<Status> Status { get; set; }

    }
 
}