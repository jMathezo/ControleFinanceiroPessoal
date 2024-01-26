using ControleFinanceiroPessoal.Domain.Exceptions;
using ControleFinanceiroPessoal.UnitTests.Domain.Entity.Categoria;
using FluentAssertions;
using Xunit;

using Dominio = ControleFinanceiroPessoal.Domain.Entities;

namespace ControleFinanceiroPessoal.UnitTests.Domain.Entity.SubCategoria;

[Collection(nameof(SubCategoriaTestFixture))]
public class SubCategoriaTest
{
    private readonly SubCategoriaTestFixture _subCategoriaTestFixture;

    public SubCategoriaTest(SubCategoriaTestFixture subCategoriaTestFixture) =>
        _subCategoriaTestFixture = subCategoriaTestFixture;

    #region .: Instanciar :.

    [Fact(DisplayName = nameof(Instanciar))]
    [Trait("Domain", "SubCategoria")]
    public void Instanciar()
    {
        // Arrange
        var dataValidar = _subCategoriaTestFixture.ObterSubCategoriaValida;

        // Act
        var dateTimeAntes = DateTime.Now;
        var subCategoria  = new Dominio.SubCategoria(
                                            dataValidar.Nome,
                                            dataValidar.Descricao,
                                            dataValidar.CategoriaId);

        var dateTimeDepois = DateTime.Now;

        // Assert
        Assert.NotNull(subCategoria );
        Assert.True(subCategoria.Nome?.Equals(dataValidar.Nome));
        Assert.True(subCategoria.Descricao?.Equals(dataValidar.Descricao));
        Assert.NotEqual(Guid.Empty, subCategoria.Id);
        Assert.NotEqual(default, subCategoria.CriadoEm);
        Assert.True(subCategoria.CriadoEm > dateTimeAntes);
        Assert.True(subCategoria.CriadoEm < dateTimeDepois);
        Assert.True(subCategoria.EstaAtivo);
    }

    [Theory(DisplayName = nameof(Erro_Ao_Instanciar_Quando_Nome_Esta_Vazio))]
    [Trait("Domain", "SubCategoria")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]
    public void Erro_Ao_Instanciar_Quando_Nome_Esta_Vazio(string? nome)
    {
        // Arrange
        Action action =
            () => new Dominio.SubCategoria(
                                  nome!, 
                                  "Descrição de subCategoria",
                                  Guid.NewGuid());

        // Act
        var exception = Assert.Throws<EntityValidationException>(action);

        // Assert
        Assert.Equal("Nome não deve ser vazio ou nulo", exception.Message);
    }

    [Fact(DisplayName = nameof(Erro_Ao_Instanciar_Quando_Descricao_Esta_Nulo))]
    [Trait("Domain", "SubCategoria")]
    public void Erro_Ao_Instanciar_Quando_Descricao_Esta_Nulo()
    {
        // Arrange

        // Act
        Action action =
            () => new Dominio.SubCategoria(
                                  "Nome subCategoria",
                                  null,
                                  Guid.NewGuid());
        
        // Assert
        action.Should().Throw<EntityValidationException>()
            .WithMessage("Descricao não deve ser nulo");
    }

    [Theory(DisplayName = nameof(Erro_Instanciar_Nome_Menos_De_3_Caracteres))]
    [Trait("Domain", "SubCategoria")]
    [MemberData(nameof(ObterNomeInvalido), parameters: 10)]
    public void Erro_Instanciar_Nome_Menos_De_3_Caracteres(string nomeInvalido)
    {
        // Arrange

        // Act
        Action action =
            () => new Dominio.SubCategoria(
                                    nomeInvalido, 
                                    "Categoria com descrição OK"!, 
                                    Guid.NewGuid());

        // Assert
        action.Should().Throw<EntityValidationException>()
            .WithMessage("Nome deve ter no mínimo 3 caracteres");
    }

    [Fact(DisplayName = nameof(Erro_Instanciar_Nome_Mais_De_100_Caracteres))]
    [Trait("Domain", "SubCategoria")]
    public void Erro_Instanciar_Nome_Mais_De_100_Caracteres()
    {
        // Arrange
        var nomeInvalido = _subCategoriaTestFixture.Faker.Lorem.Letter(101);

        // Act
        Action action =
            () => new Dominio.SubCategoria(
                                    nomeInvalido, 
                                    "Categoria com descrição OK", 
                                    Guid.NewGuid());

        // Assert
        action.Should().Throw<EntityValidationException>()
           .WithMessage("Nome deve ter no máximo 100 caracteres");
    }

    [Fact(DisplayName = nameof(Erro_Instanciar_Descricao_Mais_De_255_Caracteres))]
    [Trait("Domain", "SubCategoria")]
    public void Erro_Instanciar_Descricao_Mais_De_255_Caracteres()
    {
        //Arrange
        var descricaoInvalida = _subCategoriaTestFixture.Faker.Lorem.Letter(256);

        // Act
        Action action =
            () => new Dominio.SubCategoria(
                                "Nome Categoria OK", 
                                descricaoInvalida, 
                                Guid.NewGuid());

        // Assert
        action.Should().Throw<EntityValidationException>()
           .WithMessage("Descricao deve ter no máximo 255 caracteres");
    }

    [Fact(DisplayName = nameof(Erro_Instanciar_CategoriaId_Vazio))]
    [Trait("Domain", "SubCategoria")]
    public void Erro_Instanciar_CategoriaId_Vazio()
    {
        //Arrange
        var descricaoInvalida = _subCategoriaTestFixture.Faker.Lorem.Letter(256);

        // Act
        Action action =
            () => new Dominio.SubCategoria(
                                "Nome Categoria OK",
                                descricaoInvalida,
                                Guid.Empty);

        // Assert
        action.Should().Throw<EntityValidationException>()
           .WithMessage("CategoriaId não deve ser vazio");
    }

    #endregion

    #region .: Ativar/Desativar :.

    [Fact(DisplayName = nameof(Ativar))]
    [Trait("Domain", "SubCategoria")]
    public void Ativar()
    {
        // Arrange
        var subCategoria = _subCategoriaTestFixture.ObterSubCategoriaValida;
        subCategoria.Desativar();

        // Act
        subCategoria.Ativar();

        // Assert
        subCategoria.EstaAtivo.Should().BeTrue();
    }

    [Fact(DisplayName = nameof(Desativar))]
    [Trait("Domain", "SubCategoria")]
    public void Desativar()
    {
        // Arrange
        var subCategoria = _subCategoriaTestFixture.ObterSubCategoriaValida;

        // Act
        subCategoria.Desativar();

        // Assert
        subCategoria.EstaAtivo.Should().BeFalse();
    }

    #endregion

    #region .: Private Methods :.

    public static IEnumerable<object[]> ObterNomeInvalido(int numeroDeTestes = 6)
    {
        var fixture = new SubCategoriaTestFixture();

        for (int numeroTeste = 0; numeroTeste < numeroDeTestes; numeroTeste++)
        {
            var eImpar = numeroTeste % 2 == 1;
            yield return new object[] {
                fixture.ObterNomeValidoDeSubCategoria()[..(eImpar ? 1 : 2)]
            };
        }
    }

    #endregion
}
