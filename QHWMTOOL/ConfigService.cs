using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace QHWMTOOL
{
    public sealed class ConfigService
    {
        private const string ConfigFileName = "cauhinh.json";
        private const string LegacyConfigFileName = "cauhinh.txt";
        private static readonly byte[] Entropy = Encoding.UTF8.GetBytes("QHWMTOOL_CONFIG_V2");

        private string ConfigPath => Path.Combine(AppContext.BaseDirectory, ConfigFileName);
        private string LegacyConfigPath => Path.Combine(AppContext.BaseDirectory, LegacyConfigFileName);

        public AppConfig Load()
        {
            if (File.Exists(ConfigPath))
            {
                return LoadJsonConfig();
            }

            if (File.Exists(LegacyConfigPath))
            {
                var legacyConfig = LoadLegacyConfig();
                Save(legacyConfig);
                return legacyConfig;
            }

            return new AppConfig();
        }

        public void Save(AppConfig config)
        {
            var storage = new AppConfigStorage
            {
                Version = 2,
                DelayMin = config.DelayMin,
                DelayMax = config.DelayMax,
                TotalTasks = config.TotalTasks,
                TasksPerBatch = config.TasksPerBatch,
                RestSeconds = config.RestSeconds,
                EncryptedTdsToken = Protect(config.TdsToken),
                EncryptedCookie = Protect(config.FacebookSession.Cookie),
                FbDtsg = config.FacebookSession.FbDtsg,
                Lsd = config.FacebookSession.Lsd,
                UserId = config.FacebookSession.UserId,
                UserName = config.FacebookSession.UserName
            };

            var json = JsonConvert.SerializeObject(storage, Formatting.Indented);
            File.WriteAllText(ConfigPath, json, Encoding.UTF8);
        }

        private AppConfig LoadJsonConfig()
        {
            try
            {
                var json = File.ReadAllText(ConfigPath, Encoding.UTF8);
                var storage = JsonConvert.DeserializeObject<AppConfigStorage>(json) ?? new AppConfigStorage();

                return new AppConfig
                {
                    DelayMin = storage.DelayMin > 0 ? storage.DelayMin : 10,
                    DelayMax = storage.DelayMax > 0 ? storage.DelayMax : 20,
                    TotalTasks = storage.TotalTasks > 0 ? storage.TotalTasks : 999,
                    TasksPerBatch = storage.TasksPerBatch > 0 ? storage.TasksPerBatch : 30,
                    RestSeconds = storage.RestSeconds > 0 ? storage.RestSeconds : 60,
                    TdsToken = Unprotect(storage.EncryptedTdsToken),
                    FacebookSession = new FacebookSession
                    {
                        Cookie = Unprotect(storage.EncryptedCookie),
                        FbDtsg = storage.FbDtsg ?? string.Empty,
                        Lsd = storage.Lsd ?? string.Empty,
                        UserId = storage.UserId ?? string.Empty,
                        UserName = storage.UserName ?? string.Empty
                    }
                };
            }
            catch
            {
                return new AppConfig();
            }
        }

        private AppConfig LoadLegacyConfig()
        {
            try
            {
                var data = File.ReadAllLines(LegacyConfigPath, Encoding.UTF8);
                return new AppConfig
                {
                    TdsToken = GetValue(data, 0),
                    DelayMin = ParseOrDefault(GetValue(data, 1), 10),
                    DelayMax = ParseOrDefault(GetValue(data, 2), 20),
                    TotalTasks = ParseOrDefault(GetValue(data, 3), 999),
                    TasksPerBatch = ParseOrDefault(GetValue(data, 4), 30),
                    RestSeconds = ParseOrDefault(GetValue(data, 5), 60),
                    FacebookSession = new FacebookSession
                    {
                        Cookie = GetValue(data, 6),
                        FbDtsg = GetValue(data, 7),
                        UserName = GetValue(data, 8),
                        Lsd = GetValue(data, 9),
                        UserId = GetValue(data, 10)
                    }
                };
            }
            catch
            {
                return new AppConfig();
            }
        }

        private static int ParseOrDefault(string value, int fallback)
        {
            return int.TryParse(value, out var result) && result > 0 ? result : fallback;
        }

        private static string GetValue(string[] values, int index)
        {
            return values.Length > index ? values[index].Trim() : string.Empty;
        }

        private static string Protect(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            var plainBytes = Encoding.UTF8.GetBytes(value);
            var protectedBytes = ProtectedData.Protect(plainBytes, Entropy, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(protectedBytes);
        }

        private static string Unprotect(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            try
            {
                var protectedBytes = Convert.FromBase64String(value);
                var plainBytes = ProtectedData.Unprotect(protectedBytes, Entropy, DataProtectionScope.CurrentUser);
                return Encoding.UTF8.GetString(plainBytes);
            }
            catch
            {
                return string.Empty;
            }
        }

        private sealed class AppConfigStorage
        {
            public int Version { get; set; }
            public int DelayMin { get; set; }
            public int DelayMax { get; set; }
            public int TotalTasks { get; set; }
            public int TasksPerBatch { get; set; }
            public int RestSeconds { get; set; }
            public string EncryptedTdsToken { get; set; } = string.Empty;
            public string EncryptedCookie { get; set; } = string.Empty;
            public string FbDtsg { get; set; } = string.Empty;
            public string Lsd { get; set; } = string.Empty;
            public string UserId { get; set; } = string.Empty;
            public string UserName { get; set; } = string.Empty;
        }
    }
}
