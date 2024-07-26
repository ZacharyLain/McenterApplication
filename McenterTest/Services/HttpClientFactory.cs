using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace McenterTest.Services;

//<summary>
// Singleton class to handle http client
// These use sockets so we really dont want a ton of them open
// Unless we need them to target different points
//</summary>
public static class HttpClientFactory
{
    private static HttpClient httpClient;
    private static HttpClientHandler httpClientHandler;
    private static DateTime lastInitializationTime;
    private static readonly TimeSpan TimeoutPeriod = TimeSpan.FromMinutes(10);
    private static string baseUrl;

    static HttpClientFactory()
    {
        InitializeHttpClient();
    }

    private static HttpClientHandler setCertificates()
    {
        var handler = new HttpClientHandler();
        handler.ClientCertificateOptions = ClientCertificateOption.Manual;
        handler.SslProtocols = SslProtocols.Tls12;
        handler.ClientCertificates.Add(new X509Certificate2(Environment.GetEnvironmentVariable("SSL_CERTIFICATE")
                                                            ?? throw new InvalidOperationException
                                                                ("Error Occurred: An error occurred while getting the SSL Certificate.")));
        return handler;
    }

    private static void InitializeHttpClient()
    {
        // set the certificates in the client handler
        httpClientHandler = setCertificates();
        
        httpClient = new HttpClient(httpClientHandler)
        {
            Timeout = TimeSpan.FromMinutes(10)
        };
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        lastInitializationTime = DateTime.UtcNow;
    }

    public static void setBaseUrl(string mcenterBaseUrl)
    {
        baseUrl = mcenterBaseUrl;
    }
    
    public static HttpClient GetHttpClient()
    {
        if (DateTime.UtcNow - lastInitializationTime > TimeoutPeriod)
        {
            InitializeHttpClient();
        }

        return httpClient;
    }

    public static string getBaseUrl()
    {
        return baseUrl;
    }
}