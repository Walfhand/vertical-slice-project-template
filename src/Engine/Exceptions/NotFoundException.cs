using System.Net;

namespace Engine.Exceptions;

public class NotFoundException : CustomException
{
    public NotFoundException(string message) : base(message, HttpStatusCode.NotFound)
    {
    }

    public NotFoundException() : base("", HttpStatusCode.NotFound)
    {
    }
}

public static class ObjectExtensions
{
    public static NotFoundException NotFound(this object? obj, string message)
    {
        return new NotFoundException(message);
    }
}