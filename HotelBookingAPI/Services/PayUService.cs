using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class PayUService
{
    private readonly HttpClient _httpClient;

    public PayUService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<string> GetAccessTokenAsync(string clientId, string clientSecret)
    {
        var url = "https://secure.snd.payu.com/pl/standard/user/oauth/authorize";

        var byteArray = Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}");
        var authHeader = Convert.ToBase64String(byteArray);

        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" }
            })
        };

        request.Headers.Add("Authorization", "Basic " + authHeader);

        var response = await _httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);
            return jsonResponse.access_token;
        }

        throw new Exception("Failed to get access token.");
    }
    public async Task<string> CreateOrderAsync(string accessToken)
    {
        var url = "https://secure.snd.payu.com/api/v2_1/orders";

        var orderData = new
        {
            notifyUrl = "https://yourdomain.com/notify",
            customerIp = "192.168.0.1",
            merchantPosId = "300746",
            description = "Room booking payment",
            currencyCode = "PLN",
            totalAmount = 100000,
            products = new[]
            {
                new
                {
                    name = "Room booking",
                    unitPrice = 100000,
                    quantity = 1
                }
            }
        };

        var jsonData = JsonConvert.SerializeObject(orderData);

        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(jsonData, Encoding.UTF8, "application/json")
        };

        request.Headers.Add("Authorization", "Bearer " + accessToken);

        var response = await _httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }

        throw new Exception("Error creating order");
    }
}