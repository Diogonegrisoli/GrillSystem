namespace GrillSystem.Infrastructure;

public class RegraNegocioException : Exception
{
    public RegraNegocioException(string message) : base(message) { }
}

public sealed class ConflitoNegocioException : RegraNegocioException
{
    public ConflitoNegocioException(string message) : base(message) { }
}
