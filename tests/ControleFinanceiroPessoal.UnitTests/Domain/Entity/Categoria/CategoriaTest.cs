using ControleFinanceiroPessoal.Domain.Exceptions;
using FluentAssertions;
using Xunit;

using Dominio = ControleFinanceiroPessoal.Domain.Entities;

namespace ControleFinanceiroPessoal.UnitTests.Domain.Entity.Categoria;

[Collection(nameof(CategoriaTestFixture))]
public class CategoriaTest
{
    private readonly CategoriaTestFixture _fixture;

    public CategoriaTest(CategoriaTestFixture categoriaTestFixture) =>
        _fixture = categoriaTestFixture;

    #region .: Instanciar :.

    [Fact(DisplayName = nameof(Instanciar))]
    [Trait("Domain", "Categoria - Aggregates")]
    public void Instanciar()
    {
        // Arrange
        var dataValidar = new
        {
            Nome = _fixture.ObterNomeValidoDeCategoria(),
            Descricao = _fixture.ObterDescricaoValidaDeCategoria()
        };

        // Act
        var dateTimeAntes = DateTime.Now;
        var categoria = new Dominio.Categoria(dataValidar.Nome,
                                             dataValidar.Descricao);
        var dateTimeDepois = DateTime.Now;

        // Assert
        Assert.NotNull(categoria);
        Assert.True(categoria.Nome?.Equals(dataValidar.Nome));
        Assert.True(categoria.Descricao?.Equals(dataValidar.Descricao));
        Assert.NotEqual(Guid.Empty, categoria.Id);
        Assert.NotEqual(default, categoria.CriadoEm);
        Assert.True(categoria.CriadoEm > dateTimeAntes);
        Assert.True(categoria.CriadoEm < dateTimeDepois);
        Assert.True(categoria.EstaAtivo);
    }

    [Theory(DisplayName = nameof(Instanciar_Com_EstaAtivo))]
    [Trait("Domain", "Categoria - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void Instanciar_Com_EstaAtivo(bool estaAtivo)
    {
        // Arrange
        var dataValidar = _fixture.ObterCategoriaValida;

        // Act
        var dateTimeAntes = DateTime.Now;
        var categoria = new Dominio.Categoria(dataValidar.Nome,
                                             dataValidar.Descricao,
                                             estaAtivo);
        var dateTimeDepois = DateTime.Now;

        // Assert
        categoria.Should().NotBeNull();
        categoria.Nome.Should().Be(dataValidar.Nome);
        categoria.Descricao.Should().Be(dataValidar.Descricao);
        categoria.Id.Should().NotBeEmpty();
        categoria.CriadoEm.Should().NotBeSameDateAs(default);
        (categoria.CriadoEm > dateTimeAntes).Should().BeTrue();
        (categoria.CriadoEm < dateTimeDepois).Should().BeTrue();
        categoria.EstaAtivo.Should().Be(estaAtivo);
    }

    [Theory(DisplayName = nameof(Erro_Ao_Instanciar_Quando_Nome_Esta_Vazio))]
    [Trait("Domain", "Categoria - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]
    public void Erro_Ao_Instanciar_Quando_Nome_Esta_Vazio(string? nome)
    {
        // Arrange
        Action action =
            () => new Dominio.Categoria(
                                nome!, 
                                _fixture.ObterDescricaoValidaDeCategoria());

        // Act
        var exception = Assert.Throws<EntityValidationException>(action);

        // Assert
        Assert.Equal("Nome não deve ser vazio ou nulo", exception.Message);
    }

    [Fact(DisplayName = nameof(Erro_Ao_Instanciar_Quando_Descricao_Esta_Nulo))]
    [Trait("Domain", "Categoria - Aggregates")]
    public void Erro_Ao_Instanciar_Quando_Descricao_Esta_Nulo()
    {

        void action() => new Dominio.Categoria(
                                        _fixture.ObterNomeValidoDeCategoria(),
                                        null!);

        // Act
        var exception = Assert.Throws<EntityValidationException>(action);

        // Assert
        Assert.Equal("Descricao não deve ser nulo", exception.Message);
    }

    [Theory(DisplayName = nameof(Erro_Instanciar_Nome_Menos_De_3_Caracteres))]
    [Trait("Domain", "Categoria - Aggregates")]
    [MemberData(nameof(ObterNomeInvalido), parameters: 10)]
    public void Erro_Instanciar_Nome_Menos_De_3_Caracteres(string nomeInvalido)
    {
        // Arrange

        // Act
        Action action =
            () => new Dominio.Categoria(
                                nomeInvalido, 
                                _fixture.ObterDescricaoValidaDeCategoria());

        // Assert
        action.Should().Throw<EntityValidationException>()
            .WithMessage("Nome deve ter no mínimo 3 caracteres");
    }

    [Fact(DisplayName = nameof(Erro_Instanciar_Nome_Mais_De_100_Caracteres))]
    [Trait("Domain", "Categoria - Aggregates")]
    public void Erro_Instanciar_Nome_Mais_De_100_Caracteres()
    {
        // Arrange
        var nomeInvalido = _fixture.Faker.Lorem.Letter(101);

        // Act
        Action action =
            () => new Dominio.Categoria(
                                nomeInvalido, 
                                _fixture.ObterDescricaoValidaDeCategoria());

        // Assert
        action.Should().Throw<EntityValidationException>()
           .WithMessage("Nome deve ter no máximo 100 caracteres");
    }

    [Fact(DisplayName = nameof(Erro_Instanciar_Descricao_Mais_De_255_Caracteres))]
    [Trait("Domain", "Categoria - Aggregates")]
    public void Erro_Instanciar_Descricao_Mais_De_255_Caracteres()
    {
        //Arrange
        var descricaoInvalida = _fixture.Faker.Lorem.Letter(256);

        // Act
        Action action =
            () => new Dominio.Categoria(
                                _fixture.ObterNomeValidoDeCategoria(),
                                descricaoInvalida);
        
        // Assert
        action.Should().Throw<EntityValidationException>()
           .WithMessage("Descricao deve ter no máximo 255 caracteres");
    }

    #endregion

    #region .: Ativar/Desativar :.

    [Fact(DisplayName = nameof(Ativar))]
    [Trait("Domain", "Categoria - Aggregates")]
    public void Ativar()
    {
        // Arrange

        // Act
        var Categoria = new Dominio.Categoria(
                                        _fixture.ObterNomeValidoDeCategoria(), 
                                        _fixture.ObterDescricaoValidaDeCategoria(), 
                                        false);
        Categoria.Ativar();

        // Assert
        Categoria.EstaAtivo.Should().BeTrue();
    }

    [Fact(DisplayName = nameof(Desativar))]
    [Trait("Domain", "Categoria - Aggregates")]
    public void Desativar()
    {
        // Arrange

        // Act
        var Categoria = _fixture.ObterCategoriaValida;
        Categoria.Desativar();

        // Assert
        Categoria.EstaAtivo.Should().BeFalse();
    }

    #endregion

    #region .: Atualizar :.

    [Fact(DisplayName = nameof(Atualizar))]
    [Trait("Domain", "Categoria - Aggregates")]
    public void Atualizar()
    {
        // Arrange
        var categoria = _fixture.ObterCategoriaValida;
        var categoriaAtualizada = _fixture.ObterCategoriaValida;

        // Act
        categoria.Update(categoriaAtualizada.Nome, categoriaAtualizada.Descricao);

        // Assert
        categoria.Nome.Should().Be(categoriaAtualizada.Nome);
        categoria.Descricao.Should().Be(categoriaAtualizada.Descricao);
    }

    [Fact(DisplayName = nameof(AtualizarNome))]
    [Trait("Domain", "Categoria - Aggregates")]
    public void AtualizarNome()
    {
        // Arrange
        var categoria = _fixture.ObterCategoriaValida;
        var novoNome = _fixture.ObterNomeValidoDeCategoria();
        var descricaoAtual = categoria.Descricao;

        // Act
        categoria.Update(novoNome);

        // Assert
        categoria.Nome.Should().Be(novoNome);
        categoria.Descricao.Should().Be(descricaoAtual);
    }

    [Theory(DisplayName = nameof(Erro_Ao_Atualizar_Nome_Vazio_Ou_Nulo))]
    [Trait("Domain", "Categoria - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]
    public void Erro_Ao_Atualizar_Nome_Vazio_Ou_Nulo(string? nome)
    {
        // Arrange
        var categoria = _fixture.ObterCategoriaValida;

        // Act
        Action action =
           () => categoria.Update(nome!);

        // Assert
        action.Should().Throw<EntityValidationException>()
           .WithMessage("Nome não deve ser vazio ou nulo");
    }

    [Theory(DisplayName = nameof(Erro_Ao_Atualizar_Nome_Menos_De_3_Caracteres))]
    [Trait("Domain", "Categoria - Aggregates")]
    [MemberData(nameof(ObterNomeInvalido), parameters: 10)]
    public void Erro_Ao_Atualizar_Nome_Menos_De_3_Caracteres(string nomeInvalido)
    {
        // Arrange
        var categoria = _fixture.ObterCategoriaValida;

        // Act
        Action action =
           () => categoria.Update(nomeInvalido!);

        // Assert
        action.Should().Throw<EntityValidationException>()
          .WithMessage("Nome deve ter no mínimo 3 caracteres");
    }

    [Fact(DisplayName = nameof(Erro_Ao_Atualizar_Nome_Com_Mais_De_100_Caracteres))]
    [Trait("Domain", "Categoria - Aggregates")]
    public void Erro_Ao_Atualizar_Nome_Com_Mais_De_100_Caracteres()
    {
        // Arrange
        var nomeInvalido = _fixture.Faker.Lorem.Letter(101);
        var categoria = _fixture.ObterCategoriaValida;

        // Act
        Action action =
            () => categoria.Update(nomeInvalido!);

        // Assert
        action.Should().Throw<EntityValidationException>()
          .WithMessage("Nome deve ter no máximo 100 caracteres");
    }

    [Fact(DisplayName = nameof(Erro_Ao_Atualizar_Descricao_Com_Mais_De_255_Caracteres))]
    [Trait("Domain", "Categoria - Aggregates")]
    public void Erro_Ao_Atualizar_Descricao_Com_Mais_De_255_Caracteres()
    {
        // Arrange
        var invalidDescription = _fixture.Faker.Lorem.Letter(256); 
        var categoria = _fixture.ObterCategoriaValida;

        // Act
        Action action =
            () => categoria.Update(
                        _fixture.ObterNomeValidoDeCategoria(), 
                        invalidDescription!);

        // Assert
        action.Should().Throw<EntityValidationException>()
         .WithMessage("Descricao deve ter no máximo 255 caracteres");
    }
    #endregion

    #region .: SubCategoria :.

    [Fact(DisplayName = nameof(Adicionar_SubCategoria))]
    [Trait("Domain", "Categoria - Aggregates")]
    public void Adicionar_SubCategoria()
    {
        // Arrange
        var categoria = _fixture.ObterCategoriaValida;
        var subCategoria = new Dominio.SubCategoria(
            "Nome subcategoria", 
            "Descrição subcategoria");

        // Act
        categoria.AdicionarSubcategoria(subCategoria);

        // Assert
        categoria.SubCategorias.Count.Should().Be(1);
        categoria.SubCategorias.Should().Contain(subCategoria);
    }

    #endregion

    #region .: Private Methods :.

    public static IEnumerable<object[]> ObterNomeInvalido(int numeroDeTestes = 6)
    {
        var fixture = new CategoriaTestFixture();

        for (int numeroTeste = 0; numeroTeste < numeroDeTestes; numeroTeste++)
        {
            var eImpar = numeroTeste % 2 == 1;
            yield return new object[] {
                fixture.ObterNomeValidoDeCategoria()[..(eImpar ? 1 : 2)]
            };
        }
    }

    #endregion
}
