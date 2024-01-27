using System.Runtime.Serialization;

namespace ControleFinanceiroPessoal.Domain.Exceptions;

[Serializable]
public class EntityValidationException : Exception
{
    public EntityValidationException(string? message) : base(message)
    {
    }

    public EntityValidationException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected EntityValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
