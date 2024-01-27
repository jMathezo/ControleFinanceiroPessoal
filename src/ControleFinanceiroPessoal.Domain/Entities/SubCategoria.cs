using ControleFinanceiroPessoal.Domain.SeedWork;
using ControleFinanceiroPessoal.Domain.Validation;

namespace ControleFinanceiroPessoal.Domain.Entities;
public class SubCategoria : Entity
{
    public string Nome { get; private set; }
    public string Descricao { get; private set; }
    public bool EstaAtivo { get; private set; }
    public DateTime CriadoEm { get; private set; }

    public virtual Categoria? Categoria { get; private set; }

    public SubCategoria(string nome, string descricao)
        : base()
    {
        Nome = nome;
        Descricao = descricao;
        EstaAtivo = true;
        CriadoEm = DateTime.Now;

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

    private void Validar()
    {
        DomainValidation.NotNullOrEmpty(Nome, nameof(Nome));
        DomainValidation.MinLength(Nome, 3, nameof(Nome));
        DomainValidation.MaxLength(Nome, 100, nameof(Nome));

        DomainValidation.NotNull(Descricao, nameof(Descricao));
        DomainValidation.MaxLength(Descricao, 255, nameof(Descricao));
    }
}
