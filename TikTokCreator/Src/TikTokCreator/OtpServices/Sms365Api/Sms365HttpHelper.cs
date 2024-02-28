﻿using TikTokCreator.FiveSimApi;
using Serilog;

namespace TikTokCreator.Sms365Api
{
    public class Sms365HttpHelper
    {
        public static async Task<string> BuyPhoneNumber(string key, string countryId)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(Sms365Constant.Sms365ApiUrl);
                string appId = "ig";
                string operaTor = "any";
                string query = "stubs/handler_api.php?api_key=" + key + "&action=getNumber&service=" + appId + "&operator=" + operaTor + "&country=" + countryId + "&ref=ep1QnyEM";
                var response = await httpClient.GetAsync(query);
                string body = await response.Content.ReadAsStringAsync();
                string result = body;
                return result;
            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(Sms365HttpHelper)}, params; {nameof(BuyPhoneNumber)},key; {key}, Error; {ex.Message}, Exception; {ex}");
                return null;
            }
        }
        public static async Task<string> GetOtp(string key, string id)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(Sms365Constant.Sms365ApiUrl);
                string query = "stubs/handler_api.php?api_key=" + key + "&action=getStatus&id=" + id;
                var response = await httpClient.GetAsync(query);
                var body = await response.Content.ReadAsStringAsync();
                string result = body;
                return result;
            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(Sms365HttpHelper)}, params; {nameof(GetOtp)},key; {key}, Error; {ex.Message}, Exception; {ex}");
                return null;
            }
        }
    }
}
