using LDPlayerAndADBController.ADBClient;
using Newtonsoft.Json;
using Serilog;

namespace AppDestop.TelegramCreator.yuenankaApi
{
    public class YuenankaHttpHelper
    {
        public static async Task<YuenankaApiResponse<YuenankaResult>> BuyPhoneNumber(string key, string appId)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(YuenankaConstant.YuenankaApiUrl);
                string query = "api?act=number&apik=" + key + "&appId=" + appId;
                var response = await httpClient.GetAsync(query);
                var body = await response.Content.ReadAsStringAsync();
                YuenankaApiResponse<YuenankaResult> data = JsonConvert.DeserializeObject<YuenankaApiResponse<YuenankaResult>>(body);
                return data;
            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(YuenankaHttpHelper)}, params; {nameof(BuyPhoneNumber)}, cmd; {key}, Error; {ex.Message}, Exception; {ex}");
                return null;
            }
        }
        public static async Task<YuenankaApiResponse<YuenankaOtpResult>> GetOtp(string key, string id)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(YuenankaConstant.YuenankaApiUrl);
                string query = "api?act=code&apik=" + key + "&id=" + id;
                var response = await httpClient.GetAsync(query);
                var body = await response.Content.ReadAsStringAsync();
                YuenankaApiResponse<YuenankaOtpResult> data = JsonConvert.DeserializeObject<YuenankaApiResponse<YuenankaOtpResult>>(body);
                return data;
            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(YuenankaHttpHelper)}, params; {nameof(BuyPhoneNumber)}, cmd; {key}, Error; {ex.Message}, Exception; {ex}");
                return null;
            }
        }
    }
}
