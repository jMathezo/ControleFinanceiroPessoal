using ControleFinanceiroPessoal.Domain.Exceptions;
using FluentAssertions;
using Xunit;

using Dominio = ControleFinanceiroPessoal.Domain.Entities;

namespace ControleFinanceiroPessoal.UnitTests.Domain.Entity.Lancamentos;

[Collection(nameof(LancamentoTestFixture))]
public class LancamentoTest
{
    private readonly LancamentoTestFixture _fixture;

    public LancamentoTest(LancamentoTestFixture lancamentoTestFixture) =>
        _fixture = lancamentoTestFixture;

    #region .: Instanciar :.

    [Fact(DisplayName = nameof(Instanciar))]
    [Trait("Domain", "Lancamento - Aggregates")]
    public void Instanciar()
    {
        // Arrange
        var lancamentoValidar = _fixture.ObterLancamentoValido;

        // Act
        var dateTimeAntes = DateTime.Now;
        var lancamento = new Dominio.Lancamento(lancamentoValidar.Descricao,
                                               lancamentoValidar.Valor);
        var dateTimeDepois = DateTime.Now;

        // Assert
        lancamento.Should().NotBeNull();
        lancamento.Descricao.Should().Be(lancamentoValidar.Descricao);
        lancamento.Valor.Should().Be(lancamentoValidar.Valor);
        lancamento.Id.Should().NotBeEmpty();
        lancamento.Categoria.Should().BeNull();
        lancamento.Repetir.Should().BeFalse();
        lancamento.CriadoEm.Should().NotBeSameDateAs(default);
        lancamento.UltimaAtualizacao.Should().NotBeSameDateAs(default);
        lancamento.UltimaAtualizacao.Should().Be(lancamento.CriadoEm);
        (lancamento.CriadoEm > dateTimeAntes).Should().BeTrue();
        (lancamento.UltimaAtualizacao > dateTimeAntes).Should().BeTrue();
        (lancamento.CriadoEm < dateTimeDepois).Should().BeTrue();
        (lancamento.UltimaAtualizacao < dateTimeDepois).Should().BeTrue();
    }

    [Theory(DisplayName = nameof(Dado_Descricao_Vazia_Ou_Nula_Deve_Retornar_Erro))]
    [Trait("Domain", "Lancamento - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]
    public void Dado_Descricao_Vazia_Ou_Nula_Deve_Retornar_Erro(string? descricao)
    {
        // Arrange
        Action action =
            () => new Dominio.Lancamento(
                                descricao!,
                                _fixture.ObterValorValido());

        // Act
        var exception = Assert.Throws<EntityValidationException>(action);

        // Assert
        Assert.Equal("Descricao não deve ser vazio ou nulo", exception.Message);
    }

    [Theory(DisplayName = nameof(Dado_Descricao_Menor_Que_3_Caracteres_Deve_Retornar_Erro))]
    [Trait("Domain", "Lancamento - Aggregates")]
    [MemberData(nameof(ObterDescricaoInvalida), parameters: 10)]
    public void Dado_Descricao_Menor_Que_3_Caracteres_Deve_Retornar_Erro(string descricaoInvalida)
    {
        // Arrange

        // Act
        Action action =
            () => new Dominio.Lancamento(
                                descricaoInvalida,
                                _fixture.ObterValorValido());

        // Assert
        action.Should().Throw<EntityValidationException>()
           .WithMessage("Descricao deve ter no mínimo 3 caracteres");
    }

    [Fact(DisplayName = nameof(Dado_Descricao_Maior_Que_100_Caracteres_Deve_Retornar_Erro))]
    [Trait("Domain", "Lancamento - Aggregates")]
    public void Dado_Descricao_Maior_Que_100_Caracteres_Deve_Retornar_Erro()
    {
        // Arrange
        var descricaoInvalida = _fixture.Faker.Lorem.Letter(101);

        // Act
        Action action =
            () => new Dominio.Lancamento(
                               descricao: descricaoInvalida,
                                _fixture.ObterValorValido());

        // Assert
        action.Should().Throw<EntityValidationException>()
           .WithMessage("Descricao deve ter no máximo 100 caracteres");
    }

    [Fact(DisplayName = nameof(Dado_Observacao_Maior_Que_1000_Caracteres_Deve_Retornar_Erro))]
    [Trait("Domain", "Lancamento - Aggregates")]
    public void Dado_Observacao_Maior_Que_1000_Caracteres_Deve_Retornar_Erro()
    {
        //Arrange
        var obsInvalida = _fixture.Faker.Lorem.Letter(1001);

        // Act
        Action action =
            () => new Dominio.Lancamento(
                                descricao: _fixture.ObterDescricaoValida(),
                                valor: _fixture.ObterValorValido(),
                                observacao: obsInvalida);

        // Assert
        action.Should().Throw<EntityValidationException>()
           .WithMessage("Observacao deve ter no máximo 1000 caracteres");
    }

    #endregion

    #region .: Atualizar :.

    [Fact(DisplayName = nameof(Atualizar))]
    [Trait("Domain", "Lancamento - Aggregates")]
    public void Atualizar()
    {
        // Arrange
        var lancamento = _fixture.ObterLancamentoValido;
        var lancamentoAtualizado = _fixture.ObterLancamentoValidoComCategoriaEObservacao;

        // Act
        var dateTimeAntesUpdate = DateTime.Now;

        lancamento.Update(
            lancamentoAtualizado.Descricao,
            lancamentoAtualizado.Valor,
            lancamentoAtualizado.Categoria,
            true,
            lancamentoAtualizado.Observacao
            );
        
        var dateTimeDepoisUpdate = DateTime.Now;

        // Assert
        lancamento.Descricao.Should().Be(lancamentoAtualizado.Descricao);
        lancamento.Valor.Should().Be(lancamentoAtualizado.Valor);
        lancamento.Categoria.Should().Be(lancamentoAtualizado.Categoria);
        lancamento.Repetir.Should().BeTrue();
        lancamento.Observacao.Should().Be(lancamentoAtualizado.Observacao);
        lancamento.CriadoEm.Should().BeSameDateAs(lancamento.CriadoEm);
        lancamento.UltimaAtualizacao.Should().NotBe(lancamentoAtualizado.CriadoEm);
        (lancamento.UltimaAtualizacao > dateTimeAntesUpdate).Should().BeTrue();
        (lancamento.UltimaAtualizacao < dateTimeDepoisUpdate).Should().BeTrue();
    }

    [Fact(DisplayName = nameof(AtualizarNome))]
    [Trait("Domain", "Lancamento - Aggregates")]
    public void AtualizarNome()
    {
        // Arrange
        var lancamento = _fixture.ObterLancamentoValido;
        var novaDescricao = _fixture.ObterDescricaoValida();
        var valorAtual = lancamento.Valor;

        // Act
        lancamento.Update(novaDescricao, valorAtual);

        // Assert
        lancamento.Descricao.Should().Be(novaDescricao);
        lancamento.Valor.Should().Be(valorAtual);
    }

    [Fact(DisplayName = nameof(AtualizarValor))]
    [Trait("Domain", "Lancamento - Aggregates")]
    public void AtualizarValor()
    {
        // Arrange
        var lancamento = _fixture.ObterLancamentoValido;
        var novoValor = _fixture.ObterValorValido();
        var descricaoAtual = lancamento.Descricao;

        // Act
        lancamento.Update(descricaoAtual, valor: novoValor);

        // Assert
        lancamento.Valor.Should().Be(novoValor);
        lancamento.Descricao.Should().Be(descricaoAtual);
    }

    [Theory(DisplayName = nameof(Dado_Descricao_Nula_Ou_Vazia_Deve_Retornar_Erro))]
    [Trait("Domain", "Lancamento - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]
    public void Dado_Descricao_Nula_Ou_Vazia_Deve_Retornar_Erro(string? descricaoInvalida)
    {
        // Arrange
        var lancamento = _fixture.ObterLancamentoValido;

        // Act
        Action action =
           () => lancamento.Update(descricaoInvalida!, lancamento.Valor);

        // Assert
        action.Should().Throw<EntityValidationException>()
           .WithMessage("Descricao não deve ser vazio ou nulo");
    }

    [Theory(DisplayName = nameof(Dado_Descricao_Menor_Que_3_Caracteres_Ao_Atualizar_Deve_Retornar_Erro))]
    [Trait("Domain", "Lancamento - Aggregates")]
    [MemberData(nameof(ObterDescricaoInvalida), parameters: 10)]
    public void Dado_Descricao_Menor_Que_3_Caracteres_Ao_Atualizar_Deve_Retornar_Erro(string descricaoInvalida)
    {
        // Arrange
        var lancamento = _fixture.ObterLancamentoValido;

        // Act
        Action action =
           () => lancamento.Update(descricaoInvalida!, lancamento.Valor);

        // Assert
        action.Should().Throw<EntityValidationException>()
          .WithMessage("Descricao deve ter no mínimo 3 caracteres");
    }

    [Fact(DisplayName = nameof(Dado_Descricao_Com_Mais_De_100_Caracteres_Ao_Atualizar_Deve_Retornar_Erro))]
    [Trait("Domain", "Lancamento - Aggregates")]
    public void Dado_Descricao_Com_Mais_De_100_Caracteres_Ao_Atualizar_Deve_Retornar_Erro()
    {
        // Arrange
        var descricaoInvalida = _fixture.Faker.Lorem.Letter(101);
        var lancamento = _fixture.ObterLancamentoValido;

        // Act
        Action action =
            () => lancamento.Update(descricaoInvalida!, lancamento.Valor);

        // Assert
        action.Should().Throw<EntityValidationException>()
          .WithMessage("Descricao deve ter no máximo 100 caracteres");
    }

    #endregion

    #region .: Categoria :.

    [Fact(DisplayName = nameof(Adicionar_Categoria))]
    [Trait("Domain", "Lancamento - Aggregates")]
    public void Adicionar_Categoria()
    {
        // Arrange
        var lancamento = _fixture.ObterLancamentoValido;
        var categoria = _fixture.ObterCategoriaValida;

        // Act
        var dateTimeAntesUpdate = DateTime.Now;
        lancamento.AdicionarCategoria(categoria);
        var dateTimeDepoisUpdate = DateTime.Now;

        // Assert
        lancamento.Categoria.Should().NotBeNull();
        lancamento.Categoria.Should().Be(categoria);
        (lancamento.UltimaAtualizacao > dateTimeAntesUpdate).Should().BeTrue();
        (lancamento.UltimaAtualizacao < dateTimeDepoisUpdate).Should().BeTrue();
    }

    [Fact(DisplayName = nameof(Remover_Categoria))]
    [Trait("Domain", "Lancamento - Aggregates")]
    public void Remover_Categoria()
    {
        // Arrange
        var lancamento = _fixture.ObterLancamentoValido;

        // Act
        var dateTimeAntesUpdate = DateTime.Now;
        lancamento.RemoverCategoria();
        var dateTimeDepoisUpdate = DateTime.Now;

        // Assert
        lancamento.Categoria.Should().BeNull();
        (lancamento.UltimaAtualizacao > dateTimeAntesUpdate).Should().BeTrue();
        (lancamento.UltimaAtualizacao < dateTimeDepoisUpdate).Should().BeTrue();
    }

    #endregion

    #region .: Private Methods :.

    public static IEnumerable<object[]> ObterDescricaoInvalida(int numeroDeTestes = 6)
    {
        var fixture = new LancamentoTestFixture();

        for (int numeroTeste = 0; numeroTeste < numeroDeTestes; numeroTeste++)
        {
            var eImpar = numeroTeste % 2 == 1;
            yield return new object[] {
                fixture.ObterDescricaoValida()[..(eImpar ? 1 : 2)]
            };
        }
    }

    #endregion
}
