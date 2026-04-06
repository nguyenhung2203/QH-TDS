using Leaf.xNet;
using Newtonsoft.Json;

namespace QHWMTOOL
{
    public sealed class TdsService
    {
        public async Task<TDSProfileData?> GetProfileAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }

            try
            {
                using var request = CreateRequest();
                var url = $"https://traodoisub.com/api/?fields=profile&access_token={token}";
                var json = await ExecuteAsync(() => request.Get(url).ToString());

                if (string.IsNullOrWhiteSpace(json) || json.Contains("\"error\""))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<TDSProfileResponse>(json)?.data;
            }
            catch
            {
                return null;
            }
        }

        public async Task<TdsTaskFetchResult> GetTasksAsync(string token, string field, string type)
        {
            try
            {
                using var request = CreateRequest();
                var url = $"https://traodoisub.com/api/?fields={field}&access_token={token}&type={type}";
                var json = await ExecuteAsync(() => request.Get(url).ToString());
                var result = JsonConvert.DeserializeObject<TDSResponse>(json) ?? new TDSResponse();

                if (!string.IsNullOrWhiteSpace(result.error))
                {
                    return new TdsTaskFetchResult
                    {
                        ErrorMessage = result.error,
                        CountdownSeconds = result.countdown > 0 ? result.countdown : 60
                    };
                }

                return new TdsTaskFetchResult
                {
                    Tasks = result.data ?? new List<NhiemVuTDS>()
                };
            }
            catch (Exception ex)
            {
                return new TdsTaskFetchResult
                {
                    ErrorMessage = ex.Message,
                    CountdownSeconds = 30
                };
            }
        }

        public async Task<CoinClaimResult> ClaimCoinAsync(string token, string field, string jobId)
        {
            try
            {
                using var request = CreateRequest();
                var url = $"https://traodoisub.com/api/coin/?type={field}&id={jobId}&access_token={token}";
                var json = await ExecuteAsync(() => request.Get(url).ToString());
                var result = JsonConvert.DeserializeObject<TDSResSuccess>(json);

                if (result is null)
                {
                    return new CoinClaimResult { ErrorMessage = "TDS trả về dữ liệu không hợp lệ." };
                }

                if (!string.IsNullOrWhiteSpace(result.error))
                {
                    return new CoinClaimResult { ErrorMessage = result.error };
                }

                return new CoinClaimResult { Data = result.data };
            }
            catch (Exception ex)
            {
                return new CoinClaimResult { ErrorMessage = ex.Message };
            }
        }

        private static HttpRequest CreateRequest()
        {
            var request = new HttpRequest();
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/146.0.0.0 Safari/537.36";
            request.KeepAlive = true;
            request.AllowAutoRedirect = true;
            return request;
        }

        private static Task<string> ExecuteAsync(Func<string> action)
        {
            return Task.Run(action).WaitAsync(TimeSpan.FromSeconds(20));
        }
    }
}
