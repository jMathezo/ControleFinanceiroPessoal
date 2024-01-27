using Bogus;
using Xunit;
using ControleFinanceiroPessoal.UnitTests.Common;
using Dominio = ControleFinanceiroPessoal.Domain.Entities;

namespace ControleFinanceiroPessoal.UnitTests.Domain.Entity.SubCategoria;
public class SubCategoriaTestFixture : BaseFixture
{
    public SubCategoriaTestFixture()
        : base() { }

    public string ObterNomeValidoDeSubCategoria()
    {
        var nome = "";
        while (nome.Length < 3)
            nome = Faker.Commerce.Categories(1)[0];
        if (nome.Length > 255)
            nome = nome[..255];

        return nome;
    }

    public string ObterDescricaoValidaDeSubCategoria()
    {
        var categoryDescription = Faker.Commerce.ProductDescription();
        if (categoryDescription.Length > 10_000)
            categoryDescription = categoryDescription[..10_000];
        return categoryDescription;
    }

    public Dominio.SubCategoria ObterSubCategoriaValida =>
        new(
            ObterNomeValidoDeSubCategoria(),
            ObterDescricaoValidaDeSubCategoria()
            );
}

[CollectionDefinition(nameof(SubCategoriaTestFixture))]
public class SubCategoriaTestFixtureCollection
    : ICollectionFixture<SubCategoriaTestFixture>
{ }