using McenterTest.Utilities;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace McenterTest.Services;

/// <summary>
/// Singleton class to handle HttpClient instances.
/// </summary>
/// <remarks>
/// This class ensures that a single instance of <see cref="HttpClient"/> is used throughout the application 
/// to avoid opening multiple sockets, which can be resource-intensive. It also manages SSL certificates 
/// and sets the base URL for HTTP requests.
/// </remarks>
public static class HttpClientFactory
{
    private static HttpClient httpClient;
    private static HttpClientHandler httpClientHandler;
    private static DateTime lastInitializationTime;
    private static readonly TimeSpan TimeoutPeriod = TimeSpan.FromMinutes(10);
    private static string baseUrl;
    private static LogWriter logWriter = LogWriter.Instance;

    static HttpClientFactory()
    {
        logWriter.LogWithTimestamp("Creating HttpClientFactory Instance", LogWriter.LogLevel.Info);
        InitializeHttpClient();
    }

    /// <summary>
    /// Configures the HttpClientHandler with the required SSL certificates.
    /// </summary>
    /// <returns>An instance of <see cref="HttpClientHandler"/> configured with SSL certificates.</returns>
    /// <exception cref="ArgumentException">Thrown when the SSL certificate is not found in the environment variables.</exception>
    /// <remarks>
    /// This method performs the following steps:
    /// <list type="number">
    /// <item>
    /// <description>Creates a new instance of <see cref="HttpClientHandler"/>.</description>
    /// </item>
    /// <item>
    /// <description>Sets the <c>ClientCertificateOptions</c> to <c>Manual</c> and the SSL protocol to TLS 1.2.</description>
    /// </item>
    /// <item>
    /// <description>Adds the SSL certificate from the environment variable to the handler's client certificates.</description>
    /// </item>
    /// </list>
    /// </remarks>
    private static HttpClientHandler setCertificates()
    {
        // create handler to deal with SSL certification
        var handler = new HttpClientHandler();
        handler.ClientCertificateOptions = ClientCertificateOption.Manual;
        handler.SslProtocols = SslProtocols.Tls12;

        // add the client certificate, may be an error if pathing is wrong
        try
        {
            handler.ClientCertificates.Add(new X509Certificate2("../../../McenterCA.crt"));
        }
        catch (Exception ex)
        {
            // log the error and throw an exception
            logWriter.LogWithTimestamp("An error occurred while getting the SSL Certificate. Check your environment", LogWriter.LogLevel.Error);
            throw new ArgumentException("Error Occurred: An error occurred while setting the client SSL Certificate.");
        }

        // success
        logWriter.LogWithTimestamp("Handler successfully created", LogWriter.LogLevel.Info);

        return handler;
    }

    /// <summary>
    /// Initializes the HttpClient instance with the configured HttpClientHandler and default settings.
    /// </summary>
    /// <remarks>
    /// This method performs the following steps:
    /// <list type="number">
    /// <item>
    /// <description>Sets the certificates in the <see cref="HttpClientHandler"/> by calling <see cref="setCertificates"/>.</description>
    /// </item>
    /// <item>
    /// <description>Creates a new instance of <see cref="HttpClient"/> with the handler and sets the timeout period.</description>
    /// </item>
    /// <item>
    /// <description>Clears any existing default request headers and sets the <c>Accept</c> header to <c>application/json</c>.</description>
    /// </item>
    /// <item>
    /// <description>Updates the <c>lastInitializationTime</c> to the current UTC time.</description>
    /// </item>
    /// </list>
    /// </remarks>
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

    /// <summary>
    /// Sets the base URL for the HttpClient.
    /// </summary>
    /// <param name="mcenterBaseUrl">The base URL to be used by the HttpClient.</param>
    /// <remarks>
    /// This method updates the <c>baseUrl</c> field with the provided URL.
    /// </remarks>
    public static void setBaseUrl(string mcenterBaseUrl)
    {
        logWriter.LogWithTimestamp($"Base URL: {mcenterBaseUrl}", LogWriter.LogLevel.Info);
        baseUrl = mcenterBaseUrl;
    }
    
    /// <summary>
    /// Gets the singleton instance of the HttpClient.
    /// </summary>
    /// <returns>The singleton instance of <see cref="HttpClient"/>.</returns>
    /// <remarks>
    /// This method performs the following steps:
    /// <list type="number">
    /// <item>
    /// <description>Checks if the current time is beyond the <c>TimeoutPeriod</c> since the <c>lastInitializationTime</c>.</description>
    /// </item>
    /// <item>
    /// <description>If the timeout period has elapsed, it reinitializes the HttpClient by calling <see cref="InitializeHttpClient"/>.</description>
    /// </item>
    /// <item>
    /// <description>Returns the singleton instance of the HttpClient.</description>
    /// </item>
    /// </list>
    /// </remarks>
    public static HttpClient GetHttpClient()
    {
        if (DateTime.UtcNow - lastInitializationTime > TimeoutPeriod)
        {
            logWriter.LogWithTimestamp("Time limit has been exceeded. Generating a new token.", LogWriter.LogLevel.Info);

            InitializeHttpClient();
        }

        logWriter.LogWithTimestamp($"Time remaining: {TimeoutPeriod - (DateTime.UtcNow - lastInitializationTime)}", LogWriter.LogLevel.Info);

        return httpClient;
    }

    /// <summary>
    /// Gets the base URL set for the HttpClient.
    /// </summary>
    /// <returns>The base URL as a string.</returns>
    /// <remarks>
    /// This method returns the current value of the <c>baseUrl</c> field.
    /// </remarks>
    public static string getBaseUrl()
    {
        return baseUrl;
    }
}