using FluentValidation.Results;

namespace ChatOps.Infra.Core;

internal static class ValidationResultExtensions
{
    public static object AsLoggable(this ValidationResult result)
    {
        return result.Errors.Select(e => new
        {
            Field = e.PropertyName,
            Message = e.ErrorMessage
        });
    }
}