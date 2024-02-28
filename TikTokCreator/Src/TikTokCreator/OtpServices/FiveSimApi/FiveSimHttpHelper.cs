using TikTokCreator.ViotpApi;
using Newtonsoft.Json;
using Serilog;
using System.Net;
using System.Net.Http.Headers;
using System.Windows.Markup;

namespace TikTokCreator.FiveSimApi
{
    public class FiveSimHttpHelper
    {
        public static async Task<HttpResponseModel> BuyPhoneNumber(string country, string key)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(FiveSimContant.FiveSimApiUrl);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", key);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string query = "v1/user/buy/activation/" + country + "/any/instagram";
                var response = await httpClient.GetAsync(query);
                HttpResponseModel result = new HttpResponseModel();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    if (body == "no free phones" || body == "bad country" || body == "bad operator" || body == "no product")
                    {
                        result.Data = body;
                        result.StatusCode = 400;
                    }
                    else
                    {
                        try
                        {
                            FiveResult data = JsonConvert.DeserializeObject<FiveResult>(body);
                            result.Data = data;
                            result.StatusCode = 200;

                        }
                        catch (Exception ex)
                        {
                            Serilog.Log.Error(ex, ex.Message);
                        }
                    }
                    return result;
                }
                else
                {
                    var body = await response.Content.ReadAsStringAsync();
                    result.Data = body;
                    result.StatusCode = 400;
                    return result;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(FiveSimHttpHelper)}, params; {nameof(BuyPhoneNumber)},key; {key}, Error; {ex.Message}, Exception; {ex}");
                return null;
            }
        }
        public static async Task<HttpResponseModel> GetOtp(string key, string id)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(FiveSimContant.FiveSimApiUrl);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", key);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string query = "v1/user/check/" + id;
                var response = await httpClient.GetAsync(query);
                HttpResponseModel result = new HttpResponseModel();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    bool hasId = body.Contains("code");
                    if (!hasId)
                    {
                        result.Data = body;
                        result.StatusCode = 400;
                    }
                    else
                    {
                        try
                        {
                            FiveResult data = JsonConvert.DeserializeObject<FiveResult>(body);
                            result.Data = data;
                            result.StatusCode = 200;

                        }
                        catch (Exception ex)
                        {
                            Serilog.Log.Error(ex, ex.Message);
                        }
                    }
                    return result;
                }
                else
                {
                    var body = await response.Content.ReadAsStringAsync();
                    result.Data = body;
                    result.StatusCode = 400;
                    return result;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(FiveSimHttpHelper)}, params; {nameof(GetOtp)},key; {key}, Error; {ex.Message}, Exception; {ex}");
                return null;
            }
        }
    }
}
