using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

    public class GoogleMapsService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public GoogleMapsService(string apiKey)
        {
            _apiKey = apiKey;
            _httpClient = new HttpClient();
        }

        public async Task<string?> GetCoordinatesAsync(string address)
        {
            var url = $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(address)}&key={_apiKey}";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            var results = doc.RootElement.GetProperty("results");
            if (results.GetArrayLength() == 0)
                return null;

            var location = results[0]
                .GetProperty("geometry")
                .GetProperty("location");

            var lat = location.GetProperty("lat").GetDouble();
            var lng = location.GetProperty("lng").GetDouble();

            return $"Lat: {lat}, Lng: {lng}";
        }
    }