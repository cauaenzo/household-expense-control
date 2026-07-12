using Microsoft.EntityFrameworkCore;
using GastosResidenciais.Api.Models;

namespace GastosResidenciais.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Pessoa> Pessoas => Set<Pessoa>();
    public DbSet<Transacao> Transacoes => Set<Transacao>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pessoa>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Nome).IsRequired().HasMaxLength(200);
            entity.Property(p => p.Idade).IsRequired();

            // Cascata: ao excluir pessoa, suas transacoes sao removidas automaticamente
            entity.HasMany(p => p.Transacoes)
                  .WithOne(t => t.Pessoa)
                  .HasForeignKey(t => t.PessoaId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Transacao>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Descricao).IsRequired().HasMaxLength(500);
            entity.Property(t => t.Valor).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(t => t.Tipo).HasConversion<string>().IsRequired();
        });
    }
}
