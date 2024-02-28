using Newtonsoft.Json;
using Serilog;
using System.Net;

namespace TikTokCreator.Helper
{
    public class InstagramHelper
    {
        public static async Task<string> GetFacebookLoginCode2FA(string twoFactorCode, string? proxyUrl)
        {
            try
            {
                var httpClientHandler = new HttpClientHandler();
                if (!string.IsNullOrEmpty(proxyUrl))
                {
                    var proxyParts = proxyUrl.Split(':');
                    var Address = proxyParts[0];
                    var Port = int.Parse(proxyParts[1]);
                    var Username = proxyParts.Length > 2 ? proxyParts[2] : null;
                    var Password = proxyParts.Length > 3 ? proxyParts[3] : null;
                    if (!string.IsNullOrEmpty(Address) && Port > 0)
                    {
                        var proxy = new WebProxy($"http://{Address}:{Port}");
                        if (!string.IsNullOrEmpty(Username))
                        {
                            proxy.Credentials = new NetworkCredential(Username, Password);
                        }
                        httpClientHandler.Proxy = proxy;
                    }
                }
                using (var httpClient = new HttpClient(httpClientHandler))
                {
                    httpClient.BaseAddress = new Uri($"https://2fa.live");
                    string query = $"tok/{twoFactorCode}";
                    var response = await httpClient.GetAsync(query);
                    var body = await response.Content.ReadAsStringAsync();
                    Code2Fa data = JsonConvert.DeserializeObject<Code2Fa>(body);
                    if (data != null)
                    {
                        Log.Information("GetFacebookLoginCode|" + twoFactorCode + "|" + data.token);
                        return data.token;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);

            }
            return string.Empty;
        }
        public class Code2Fa
        {
            public string token { get; set; }
        }
    }
}
