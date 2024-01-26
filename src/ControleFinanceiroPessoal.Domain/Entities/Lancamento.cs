using ControleFinanceiroPessoal.Domain.SeedWork;

namespace ControleFinanceiroPessoal.Domain.Entities;
internal class Lancamento : Entity
{
    public string Titulo { get; private set; }
    public double Valor { get; private set; }
}
