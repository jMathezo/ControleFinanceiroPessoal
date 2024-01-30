using ControleFinanceiroPessoal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFinanceiroPessoal.Infra.Data.EF.Configurations;
public class SubCategoriaConfiguration : IEntityTypeConfiguration<SubCategoria>
{
    public void Configure(EntityTypeBuilder<SubCategoria> builder)
    {
        // Tabela
        builder.ToTable("SubCategoria");

        // Chave Primária
        builder.HasKey(subCategoria => subCategoria.Id);

        // Propriedades
        builder.Property(subCategoria => subCategoria.Nome)
            .HasMaxLength(100);

        builder.Property(subCategoria => subCategoria.Descricao)
            .HasMaxLength(255);

        builder.Property(x => x.CriadoEm)
            .IsRequired()
            .HasColumnName("CriadoEm")
            .HasColumnType("SMALLDATETIME")
            .HasMaxLength(60)
            .HasDefaultValue(DateTime.Now.ToUniversalTime());

        // Índices
        builder
            .HasIndex(x => x.Nome, "IX_SubCategoria_Nome")
            .IsUnique();

        builder.HasOne(x => x.Categoria)
               .WithMany(x => x.SubCategorias)
               .HasConstraintName("FK_Categoria_SubCategoria")
               .OnDelete(DeleteBehavior.Cascade);
    }
}
