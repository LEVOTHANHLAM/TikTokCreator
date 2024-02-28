using Newtonsoft.Json;
using Serilog;

namespace TikTokCreator.ViotpApi
{
    public class ViotpHttpHelper
    {
        public static async Task<ViotpApiResponse<ViotpResult>> BuyPhoneNumber(string key, string appId)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ViotpConstant.ViotpApiUrl);
                string query = "request/getv2?token=" + key + "&serviceId=" + appId;
                var response = await httpClient.GetAsync(query);
                var body = await response.Content.ReadAsStringAsync();
                ViotpApiResponse<ViotpResult> data = JsonConvert.DeserializeObject<ViotpApiResponse<ViotpResult>>(body);
                return data;
            }
            catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
            {
                Log.Error($"{nameof(ViotpHttpHelper)}, params; {nameof(BuyPhoneNumber)},key; {key}, Error; {ex.Message}, Exception; {ex}");
                return null;
            }
        }
        public static async Task<ViotpApiResponse<ViotpOtpResult>> GetOtp(string key, string id)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ViotpConstant.ViotpApiUrl);
                string query = "session/getv2?requestId=" + id + "&token=" + key;
                var response = await httpClient.GetAsync(query);
                var body = await response.Content.ReadAsStringAsync();
                ViotpApiResponse<ViotpOtpResult> data = JsonConvert.DeserializeObject<ViotpApiResponse<ViotpOtpResult>>(body);
                return data;
            }
            catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
            {
                Log.Error($"{nameof(ViotpHttpHelper)}, params; {nameof(GetOtp)},key; {key}, Error; {ex.Message}, Exception; {ex}");
                return null;
            }
        }
    }
}
