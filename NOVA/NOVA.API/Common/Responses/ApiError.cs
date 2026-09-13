namespace NOVA.API.Common.Responses
{
    public sealed class ApiError
    {
        public string Code { get; init; } = string.Empty;
        public string Message { get; init; } = string.Empty;
        public Dictionary<string, string[]>? FieldErrors { get; init; }
        public string? TraceId { get; init; }
    }
}
