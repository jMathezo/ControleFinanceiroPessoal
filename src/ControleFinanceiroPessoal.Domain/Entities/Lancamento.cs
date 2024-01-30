using ControleFinanceiroPessoal.Domain.SeedWork;
using ControleFinanceiroPessoal.Domain.Validation;

namespace ControleFinanceiroPessoal.Domain.Entities;
public class Lancamento : AggregateRoot
{
    public string Descricao { get; private set; }
    public double Valor { get; private set; }
    public Categoria? Categoria { get; private set; }
    public bool Repetir { get; private set; }
    public string Observacao { get; private set; }
    public DateTime CriadoEm { get; private set; }
    public DateTime UltimaAtualizacao { get; private set; }

    public Lancamento(
        string descricao,
        double valor,
        Categoria? categoria = null,
        bool repetir = false,
        string observacao = "")
        : base()
    {
        Descricao = descricao;
        Valor = valor;
        Categoria = categoria;
        Repetir = repetir;
        Observacao = observacao;
        CriadoEm = DateTime.Now;
        UltimaAtualizacao = CriadoEm;

        Validar();
        Observacao = observacao;
    }

    public void Update(string descricao,
        double valor,
        Categoria? categoria = null,
        bool repetir = false,
        string observacao = "")
    {
        Descricao = descricao;
        Valor = valor;
        Categoria = categoria;
        Repetir = repetir;
        Observacao = observacao;
        UltimaAtualizacao = DateTime.Now;

        Validar();
    }

    public void AdicionarCategoria(Categoria categoria)
    {
        Categoria = categoria;
        UltimaAtualizacao = DateTime.Now;
    }

    public void RemoverCategoria()
    {
        Categoria = null;
        UltimaAtualizacao = DateTime.Now;
    }

    public void Validar()
    {
        DomainValidation.NotNullOrEmpty(Descricao, nameof(Descricao));
        DomainValidation.MinLength(Descricao, 3, nameof(Descricao));
        DomainValidation.MaxLength(Descricao, 100, nameof(Descricao));

        DomainValidation.MaxLength(Observacao, 1000, nameof(Observacao));
    }
}
