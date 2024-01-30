using Bogus;
using Xunit;
using ControleFinanceiroPessoal.UnitTests.Common;
using Dominio = ControleFinanceiroPessoal.Domain.Entities;

namespace ControleFinanceiroPessoal.UnitTests.Domain.Entity.Categoria;
public class CategoriaTestFixture : BaseFixture
{
    public CategoriaTestFixture() 
        : base() { }

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

[CollectionDefinition(nameof(CategoriaTestFixture))]
public class CategoriaTestFixtureCollection
    : ICollectionFixture<CategoriaTestFixture>
{ }