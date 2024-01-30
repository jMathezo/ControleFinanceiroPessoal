using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleFinanceiroPessoal.Domain.Entities;
public class LancamentoDebito : Lancamento
{
    public LancamentoDebito(
        string? descricao, 
        double valor, 
        Categoria? categoria, 
        bool repetir) 
        : base(descricao, valor, categoria, repetir)
    {
    }
}
