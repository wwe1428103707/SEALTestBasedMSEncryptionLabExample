using FitnessTracker.Common.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTrackerClient
{
    public static class FitnessTrackerClient
    {
        private static HttpClient _client = new HttpClient();

        private static readonly string BaseUri = "http://localhost:58849/api";

        public static async Task<KeysModel> GetKeys()
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUri}/prime/keys"))
            {
                var response = await _client.SendAsync(request);

                response.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<KeysModel>(await response.Content.ReadAsStringAsync());
            }
        }

        internal static async Task AddNewPrime(PrimeItem metricsRequest)
        {
            var metricsRequestAsJsonStr = JsonConvert.SerializeObject(metricsRequest);

            using (var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUri}/prime"))
            using (var content = new StringContent(metricsRequestAsJsonStr, Encoding.UTF8, "application/json"))
            {
                request.Content = content;
                var response = await _client.SendAsync(request);
                response.EnsureSuccessStatusCode();
            }
        }

        internal static async Task<EncAnswerItem> GetAnswer()
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUri}/prime"))
            {
                var response = await _client.SendAsync(request);
                return JsonConvert.DeserializeObject<EncAnswerItem>(await response.Content.ReadAsStringAsync());
            }
        }
    }
}