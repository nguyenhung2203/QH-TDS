namespace QHWMTOOL
{
    public sealed class FacebookValidationResult
    {
        public FacebookSession? Session { get; init; }
        public string? ErrorMessage { get; init; }

        public bool Success => Session is not null && string.IsNullOrWhiteSpace(ErrorMessage);
    }

    public sealed class FacebookReactResult
    {
        public bool Success { get; init; }
        public bool IsBlocked { get; init; }
        public string? ErrorMessage { get; init; }
    }
}
