namespace NOVA.Domain.Common.Results
{
    public enum ErrorType
    {
        None,
        Validation,
        BadRequest,
        NotFound,
        Conflict,
        Business,
        RateLimited,
        Unauthorized,
        Unexpected,
        Forbidden,
        Failure
    }
}