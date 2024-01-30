namespace ControleFinanceiroPessoal.Domain.Entities;
public class LancamentoCredito : Lancamento
{
    public LancamentoCredito(
        string? descricao, 
        double valor, 
        Categoria? categoria, 
        bool repetir,
        DateTime dataCredito, 
        bool recebido)
        : base(descricao, valor, categoria, repetir)
    {
        DataCredito = dataCredito;
        Recebido = recebido;
    }

    public DateTime DataCredito { get; private set; }
    public bool Recebido { get; private set; }
}
