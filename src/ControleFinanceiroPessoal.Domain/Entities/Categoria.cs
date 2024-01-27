using ControleFinanceiroPessoal.Domain.SeedWork;
using ControleFinanceiroPessoal.Domain.Validation;

namespace ControleFinanceiroPessoal.Domain.Entities;
public class Categoria : AggregateRoot
{
    public string Nome { get; private set; }
    public string Descricao { get; private set; }
    public bool EstaAtivo { get; private set; }
    public DateTime CriadoEm { get; private set; }
    
    private readonly IList<SubCategoria> _subcategorias;

    public IReadOnlyCollection<SubCategoria> SubCategorias => _subcategorias.ToList().AsReadOnly();

    public Categoria(string nome, string descricao, bool estaAtivo = true)
        : base()
    {
        Nome = nome;
        Descricao = descricao;
        EstaAtivo = estaAtivo;
        CriadoEm = DateTime.Now;

        _subcategorias = new List<SubCategoria>();

        Validar();
    }

    public void Ativar()
    {
        EstaAtivo = true;
        Validar();
    }

    public void Desativar()
    {
        EstaAtivo = false;
        Validar();
    }

    public void Update(string nome, string? descricao = null)
    {
        Nome = nome;
        Descricao = descricao ?? Descricao;

        Validar();
    }

    public void AdicionarSubcategoria(SubCategoria subCategoria)
    {
        _subcategorias.Add(subCategoria);
    }

    private void Validar()
    {
        DomainValidation.NotNullOrEmpty(Nome, nameof(Nome));
        DomainValidation.MinLength(Nome, 3, nameof(Nome));
        DomainValidation.MaxLength(Nome, 100, nameof(Nome));

        DomainValidation.NotNull(Descricao, nameof(Descricao));
        DomainValidation.MaxLength(Descricao, 255, nameof(Descricao));
    }
}
