using ControleFinanceiroPessoal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFinanceiroPessoal.Infra.Data.EF.Configurations;
internal class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        // Tabela
        builder.ToTable("Categoria");

        // Chave Primária
        builder.HasKey(categoria => categoria.Id);

        // Propriedades
        builder.Property(categoria => categoria.Nome)
            .HasMaxLength(255);
        
        builder.Property(categoria => categoria.Descricao)
            .HasMaxLength(10_000);

        builder.Property(x => x.CriadoEm)
            .IsRequired()
            .HasColumnName("CriadoEm")
            .HasColumnType("SMALLDATETIME")
            .HasMaxLength(60)
            .HasDefaultValue(DateTime.Now.ToUniversalTime());

        // Índices
        builder
            .HasIndex(x => x.Nome, "IX_Categoria_Nome")
            .IsUnique();
    }
}