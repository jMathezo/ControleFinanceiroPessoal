using ControleFinanceiroPessoal.Domain.Exceptions;

namespace ControleFinanceiroPessoal.Domain.Validation;

public class DomainValidation
{
    protected DomainValidation() { }

    public static void NotNull(object? target, string fieldName)
    {
        if (target is null)
            throw new EntityValidationException($"{fieldName} não deve ser nulo");
    }

    public static void NotNullOrEmpty(string? target, string fieldName)
    {
        if (String.IsNullOrWhiteSpace(target))
            throw new EntityValidationException($"{fieldName} não deve ser vazio ou nulo");
    }

    public static void MinLength(string target, int minLength, string fieldName)
    {
        if (target.Length < minLength)
            throw new EntityValidationException(
                $"{fieldName} deve ter no mínimo {minLength} caracteres"
                );
    }

    public static void MaxLength(string target, int maxLength, string fieldName)
    {
        if (target.Length > maxLength)
            throw new EntityValidationException(
                $"{fieldName} deve ter no máximo {maxLength} caracteres"
                );
    }
}
