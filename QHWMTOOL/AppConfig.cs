namespace QHWMTOOL
{
    public sealed class AppConfig
    {
        public string TdsToken { get; set; } = string.Empty;
        public int DelayMin { get; set; } = 10;
        public int DelayMax { get; set; } = 20;
        public int TotalTasks { get; set; } = 999;
        public int TasksPerBatch { get; set; } = 30;
        public int RestSeconds { get; set; } = 60;
        public FacebookSession FacebookSession { get; set; } = new();
    }

    public sealed class FacebookSession
    {
        public string Cookie { get; set; } = string.Empty;
        public string FbDtsg { get; set; } = string.Empty;
        public string Lsd { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }
}
