namespace QHWMTOOL
{
    public sealed class TDSProfileResponse
    {
        public int success { get; set; }
        public TDSProfileData? data { get; set; }
    }

    public sealed class TDSProfileData
    {
        public string user { get; set; } = string.Empty;
        public string xu { get; set; } = string.Empty;
        public string xudie { get; set; } = string.Empty;
    }

    public sealed class NhiemVuTDS
    {
        public string id { get; set; } = string.Empty;
        public string code { get; set; } = string.Empty;
        public string type { get; set; } = string.Empty;
    }

    public sealed class TDSResponse
    {
        public int cache { get; set; }
        public string error { get; set; } = string.Empty;
        public int countdown { get; set; }
        public List<NhiemVuTDS> data { get; set; } = new();
    }

    public sealed class TDSResSuccess
    {
        public int success { get; set; }
        public string error { get; set; } = string.Empty;
        public TDSDataSuccess? data { get; set; }
    }

    public sealed class TDSDataSuccess
    {
        public long xu { get; set; }
        public int job_success { get; set; }
        public string msg { get; set; } = string.Empty;
    }

    public sealed class TdsTaskFetchResult
    {
        public List<NhiemVuTDS> Tasks { get; init; } = new();
        public string? ErrorMessage { get; init; }
        public int CountdownSeconds { get; init; }
    }

    public sealed class CoinClaimResult
    {
        public TDSDataSuccess? Data { get; init; }
        public string? ErrorMessage { get; init; }
    }
}
