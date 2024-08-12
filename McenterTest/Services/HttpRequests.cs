using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using McenterTest.Models;

namespace McenterTest.Services;

/// <summary>
/// Provides methods for making HTTP requests.
/// </summary>
/// <remarks>
/// This class uses the <see cref="HttpClientFactory"/> to send HTTP requests to specified endpoints.
/// </remarks>
public class HttpRequests
{
    static ApiAuth apiAuth = new();
    
    /// <summary>
    /// Sends an HTTP request to the specified URL with the given method and content.
    /// </summary>
    /// <param name="url">The URL to send the request to.</param>
    /// <param name="method">The HTTP method to use (e.g., GET, POST).</param>
    /// <param name="content">The content to send with the request.</param>
    /// <returns>The HTTP response message.</returns>
    /// <remarks>
    /// This method creates an HTTP request message with the specified URL, method, and content,
    /// sends the request using the <see cref="HttpClient"/> instance, and returns the response message.
    /// </remarks>
    public static HttpResponseMessage? getBearerToken(string requestUrl, HttpMethod httpMethod, List<KeyValuePair<string, string>> requestBody)
    {
        // create request including header
        // This request is in the x-www-form-urlencoded format rather than JSON
        HttpRequestMessage request = new(httpMethod, requestUrl);
        request.Headers.Add("Accept", "application/json; charset=utf-8");
        request.Content = new FormUrlEncodedContent(requestBody);
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded")
        {
            CharSet = "UTF-8"
        };

        HttpResponseMessage response = new();
        
        // send request and save response
        try
        {
            response = HttpClientFactory.GetHttpClient().SendAsync(request).Result;
            if (response == null)
            {
                throw new HttpRequestException($"Request returned with error: Response from HttpClient is null");
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Request returned with error: {requestUrl} - {response.StatusCode} - {response.Content.ReadAsStringAsync().Result}");
            }
        }
        catch (Exception e)
        {
            return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(e.Message)
            };
        }
        
        // return response
        return response;
    }
    
    /// <summary>
    /// Sends an HTTP request to the specified URL extension with the given HTTP method and request body.
    /// </summary>
    /// <param name="urlExtension">The URL extension to be concatenated with the base URL.</param>
    /// <param name="httpMethod">The HTTP method to use for the request (e.g., GET, POST).</param>
    /// <param name="requestBody">The request body to be sent with the request.</param>
    /// <returns>The HTTP response message.</returns>
    /// <remarks>
    /// This method performs the following steps:
    /// <list type="number">
    /// <item>
    /// <description>Concatenates the base URL and the URL extension to form the complete request URL.</description>
    /// </item>
    /// <item>
    /// <description>Creates an <see cref="HttpRequestMessage"/> for non-GET requests.</description>
    /// </item>
    /// <item>
    /// <description>Sends the request using the <see cref="HttpClient"/> from <see cref="HttpClientFactory"/> and captures the response.</description>
    /// </item>
    /// <item>
    /// <description>Throws a <see cref="SystemException"/> if the response is null or if the response indicates an unsuccessful status.</description>
    /// </item>
    /// <item>
    /// <description>Returns the response message.</description>
    /// </item>
    /// </list>
    /// </remarks>
    /// <exception cref="SystemException">Thrown when the response from the HttpClient is null or if the response indicates an unsuccessful status.</exception>
    public static HttpResponseMessage? httpRequest(string urlExtension, HttpMethod httpMethod, JsonContent requestBody)
    {
        // concatenate the baseUrl and the extension to get the endpoint
        string requestUrl = HttpClientFactory.getBaseUrl() + urlExtension;
        
        // create request for non Get messages
        HttpRequestMessage request = new(httpMethod, requestUrl)
        {
            Content = requestBody
        };
        
        // create response message to capture the response
        HttpResponseMessage? response = null;

        try
        {
            ApiAuth.getBearerToken();
        }
        catch (Exception e)
        {
            return new HttpResponseMessage(System.Net.HttpStatusCode.NetworkAuthenticationRequired)
            {
                Content = new StringContent(e.Message)
            };
        }
        
        try
        {
            // send requests
            if (httpMethod.Equals(HttpMethod.Get))
            {
                // get messages only require the url endpoint
                response = HttpClientFactory.GetHttpClient().GetAsync(requestUrl).Result;
            }
            else
            {
                // non get messages require the endpoint and a body
                response = HttpClientFactory.GetHttpClient().SendAsync(request).Result;
            }
        
            if (response == null)
            {
                throw new SystemException("Error Occurred: Response from HttpClient is null");
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new SystemException($"Request returned with error: {requestUrl} - {response.StatusCode} - {response.Content.ReadAsStringAsync().Result}");
            }
        }
        catch (Exception e)
        {
            return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(e.Message)
            };
        }
        
        // return response
        return response;
    }
}