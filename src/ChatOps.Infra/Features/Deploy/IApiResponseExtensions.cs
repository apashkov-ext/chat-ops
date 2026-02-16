using System.Net;
using ChatOps.App.Features.Deploy;
using Refit;

namespace ChatOps.Infra.Features.Deploy;

internal static class IApiResponseExtensions
{
    public static FindRefFailureReason MapToFindRefFailureReason(this IApiResponse response)
    {
        switch (response.StatusCode)
        {
            case HttpStatusCode.TooManyRequests:
            case >= HttpStatusCode.InternalServerError:
            case HttpStatusCode.RequestTimeout:
                return FindRefFailureReason.Transient;
            case HttpStatusCode.BadRequest 
                or HttpStatusCode.Unauthorized 
                or HttpStatusCode.Forbidden
                or HttpStatusCode.NotFound
                or HttpStatusCode.Conflict 
                or HttpStatusCode.UnprocessableEntity:
                return FindRefFailureReason.Permanent;
            default:
                return FindRefFailureReason.Unknown;
        }
    }
}