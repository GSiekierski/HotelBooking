using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

public class PayUService
{
    private readonly HttpClient _httpClient;

    public PayUService(HttpClient httpClient)
    {
        var handler = new HttpClientHandler
        {
            AllowAutoRedirect = false
        };
        _httpClient = new HttpClient(handler);
    }

    public async Task<string> GetAccessTokenAsync(string clientId, string clientSecret)
    {
        var requestContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "client_credentials"),
            new KeyValuePair<string, string>("client_id", clientId),
            new KeyValuePair<string, string>("client_secret", clientSecret)
        });

        var response = await _httpClient.PostAsync("https://secure.snd.payu.com/pl/standard/user/oauth/authorize", requestContent);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            return $"Błąd: {response.StatusCode} - {responseContent}";
        }

        dynamic tokenData = JsonConvert.DeserializeObject(responseContent)!;
        return tokenData.access_token;
    }

    public async Task<string> CreateOrderAsync(string accessToken)
    {
        var orderData = new
        {
            customerIp = "127.0.0.1",
            merchantPosId = "300746",
            description = "Test order",
            currencyCode = "PLN",
            totalAmount = "10000",
            extOrderId = Guid.NewGuid().ToString(),
            products = new[]
            {
            new
            {
                name = "Test Product",
                unitPrice = "10000",
                quantity = "1"
            }
        }
        };

        var jsonContent = new StringContent(JsonConvert.SerializeObject(orderData), Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(HttpMethod.Post, "https://secure.snd.payu.com/api/v2_1/orders")
        {
            Headers =
        {
            { "Authorization", $"Bearer {accessToken}" },
            { "Accept", "application/json" }
            //{ "Content-Type", "application/json" }

        },
            Content = jsonContent
        };

        try
        {
            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            var contentType = response.Content.Headers.ContentType?.MediaType ?? "";

            if (contentType.Contains("text/html") || responseContent.TrimStart().StartsWith("<!DOCTYPE html>", StringComparison.OrdinalIgnoreCase))
            {
                return responseContent;
            }

            using var doc = JsonDocument.Parse(responseContent);
            var status = doc.RootElement.GetProperty("status").GetProperty("statusCode").GetString();

            if (status == "SUCCESS")
            {
                var redirectUri = doc.RootElement.GetProperty("redirectUri").GetString();
                return redirectUri!;
            }

            return $"Błąd PayU: {doc.RootElement.GetProperty("status").GetProperty("statusDesc").GetString()}";
        }
        catch (Exception ex)
        {
            return $"Błąd podczas tworzenia zamówienia: {ex.Message}";
        }
    }




}
