using Engine.Exceptions;

namespace Api.Domain.Exceptions;

public class DomainException(string message) : CustomException(message)
{
}