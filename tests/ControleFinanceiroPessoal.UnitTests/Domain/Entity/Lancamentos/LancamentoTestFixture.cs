using ControleFinanceiroPessoal.UnitTests.Common;
using Xunit;

using Dominio = ControleFinanceiroPessoal.Domain.Entities;

namespace ControleFinanceiroPessoal.UnitTests.Domain.Entity.Lancamentos;
public class LancamentoTestFixture : BaseFixture
{
    public string ObterDescricaoValida()
    {
        var dedscricao = Faker.Commerce.ProductDescription();
        if (dedscricao.Length > 100)
            dedscricao = dedscricao[..100];
        return dedscricao;
    }

    public double ObterValorValido() =>
        new Random().NextDouble() * (1 - 10) + 1;

    public string ObterObservacaoValida()
    {
        var observacao = Faker.Commerce.ProductDescription();
        if (observacao.Length > 1000)
            observacao = observacao[..1000];
        return observacao;
    }

    public Dominio.Lancamento ObterLancamentoValido =>
    new(
        ObterDescricaoValida(),
        ObterValorValido()
        );

    public Dominio.Lancamento ObterLancamentoValidoComCategoriaEObservacao =>
        new(
            ObterDescricaoValida(),
            ObterValorValido(),
            ObterCategoriaValida,
            false,
            ObterObservacaoValida()
           );

    public string ObterNomeValidoDeCategoria()
    {
        var nome = "";
        while (nome.Length < 3)
            nome = Faker.Commerce.Categories(1)[0];
        if (nome.Length > 255)
            nome = nome[..255];

        return nome;
    }

    public string ObterDescricaoValidaDeCategoria()
    {
        var categoryDescription = Faker.Commerce.ProductDescription();
        if (categoryDescription.Length > 255)
            categoryDescription = categoryDescription[..255];
        return categoryDescription;
    }

    public Dominio.Categoria ObterCategoriaValida =>
        new(
            ObterNomeValidoDeCategoria(),
            ObterDescricaoValidaDeCategoria()
            );
}

[CollectionDefinition(nameof(LancamentoTestFixture))]
public class LancamentoTestFixtureCollection
    : ICollectionFixture<LancamentoTestFixture>
{ }