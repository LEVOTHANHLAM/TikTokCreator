using Newtonsoft.Json;
using Serilog;
using System.Net.Http.Headers;

namespace TikTokCreator.GetSmsCode
{
    public class GetSmsCodeHttpHelper
    {
        public static async Task<GetSmsCodeResult> BuyPhoneNumber(string key, string countryId)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(GetSmsCodeConstant.GetSmsCodeURL);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", key);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string query = $"api/order-number?service=ig&country={countryId}";
                var response = await httpClient.GetAsync(query);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    GetSmsCodeResult data = JsonConvert.DeserializeObject<GetSmsCodeResult>(body);
                    return data;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(GetSmsCodeHttpHelper)}, params; {nameof(BuyPhoneNumber)},key; {key}, Error; {ex.Message}, Exception; {ex}");
                return null;
            }
            return null;
        }
        public static async Task<GetSmsCodeResult> GetOtp(string key, string id)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(GetSmsCodeConstant.GetSmsCodeURL);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", key);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string query = $"api/get-sms-code?activationId={id}";
                var response = await httpClient.GetAsync(query);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    GetSmsCodeResult data = JsonConvert.DeserializeObject<GetSmsCodeResult>(body);
                    return data;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(GetSmsCodeHttpHelper)}, params; {nameof(GetOtp)},key; {key}, Error; {ex.Message}, Exception; {ex}");
                return null;
            }
            return null;
        }
    }
}
