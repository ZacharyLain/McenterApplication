using System.Net.Http;
using System.Net.Http.Headers;

namespace McenterTest.Infrastructure;

//<summary>
// Singleton class to handle http client
// These use sockets so we really dont want a ton of them open
// Unless we need them to target different points
//</summary>
public static class HttpClientFactory
{
    private static HttpClient _httpClient;
    private static DateTime _lastInitializationTime;
    private static readonly TimeSpan TimeoutPeriod = TimeSpan.FromMinutes(10);

    static HttpClientFactory()
    {
        InitializeHttpClient();
    }

    private static void InitializeHttpClient()
    {
        _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromMinutes(10)
        };
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _lastInitializationTime = DateTime.UtcNow;
    }

    public static HttpClient GetHttpClient()
    {
        if (DateTime.UtcNow - _lastInitializationTime > TimeoutPeriod)
        {
            InitializeHttpClient();
        }

        return _httpClient;
    }
}