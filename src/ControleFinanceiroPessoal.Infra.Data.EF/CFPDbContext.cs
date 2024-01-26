using ControleFinanceiroPessoal.Domain.Entities;
using ControleFinanceiroPessoal.Infra.Data.EF.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiroPessoal.Infra.Data.EF;
public class CFPDbContext : DbContext
{
    public DbSet<Categoria> Categorias => Set<Categoria>();

    public CFPDbContext(
    DbContextOptions<CFPDbContext> options
) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new CategoriaConfiguration());
    }
}
