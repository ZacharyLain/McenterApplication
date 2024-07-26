using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using McenterTest.Services;
using McenterTest.Services.Requests.Models;

namespace McenterTest.Models;

public class AuthToken
{
    private string? baseUrl;
    private string? opennessTokenFilePath = Environment.GetEnvironmentVariable("CREDENTIAL_FILE_PATH");
    private TokenFile? tokenFile;
    private BearerToken? bearerToken;
    private JsonSerializerOptions serializerOptions;
    private HttpClient httpClient;

    // extract text from OpenAPIClientCredentials.json
    private void extractFileContents()
    {
        // check if token path is set
        if (string.IsNullOrWhiteSpace(opennessTokenFilePath))
        {
            throw new Exception("Please set the token file path in your environment");
        }

        serializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        // read content from token file to extract the necessary stuff
        string credentialFileText = File.ReadAllText(opennessTokenFilePath);
        
        // deserialize token file content
        tokenFile = JsonSerializer.Deserialize<TokenFile>(credentialFileText, serializerOptions);
            
        //extract baseURL from openness token
        var baseURI = (tokenFile != null) ? new Uri(this.tokenFile.IdentityServiceUrl): throw new Exception($"Openness token not available");
        baseUrl =  baseURI.Scheme + "://" + baseURI.Authority;
    }

    private bool checkBearerToken()
    {
        // check if token exists and if it is valid for less than 5 minutes
        if (bearerToken == null ||
            bearerToken.Expires_at < DateTime.Now.AddMinutes(5))
        {
            return true;
        }

        // throw exception if opennessToken is not existing
        if (tokenFile == null)
        {
            throw new Exception("tokenFile Null: Tried to read the token file when it was null.");
        }

        return false;
    }

    private HttpResponseMessage? sendRequest(string requestUrl, HttpMethod httpMethod, List<KeyValuePair<string, string>> requestBody)
    {
        // create request including header
        HttpRequestMessage request = new(httpMethod, requestUrl);
        request.Headers.Add("Accept", "application/json; charset=utf-8");
        request.Content = new FormUrlEncodedContent(requestBody);
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded")
        {
            CharSet = "UTF-8"
        };

        // send request and save response
        var response = HttpClientFactory.GetHttpClient().SendAsync(request).Result;
        if (response == null)
        {
            throw new SystemException("Error Occurred: Response from HttpClient is null");
        }

        if (!response.IsSuccessStatusCode)
        {
            throw new SystemException($"Request returned with error: {requestUrl} - {response.StatusCode} - {response.Content.ReadAsStringAsync().Result}");
        }
        
        // return response
        return response;
    }
    
    private void generateBearerToken()
    {
        // check if a token needs to be generated
        if (checkBearerToken())
        {
            try
            {
                // endpoint to get the bearer token
                string requestUrl = tokenFile.IdentityServiceUrl + "/connect/token";

                // build content body with client id and other content from ClientCredentials tokenFile
                var requestBody = new List<KeyValuePair<string, string>>
                {
                    new("grant_type", "ClientAuthGrantType"),
                    new("client_id", tokenFile.IdentityServer4Client),
                    new("ClientAuthPayload", JsonSerializer.Serialize(tokenFile, serializerOptions)),
                };

                HttpMethod post = HttpMethod.Post;
                
                // Send request to endpoint with body as message
                HttpResponseMessage? response = sendRequest(requestUrl, post, requestBody);
                string responseText = response.Content.ReadAsStringAsync().Result;

                // deserialize bearer token
                bearerToken = JsonSerializer.Deserialize<BearerToken>(responseText, serializerOptions);
                if (bearerToken == null)
                {
                    throw new Exception($"Error Occurred: Bearer token could not be deserialized from '{responseText}'");
                }

                // set expiration date based on the timestamp of the response and the "expires_in" value
                bearerToken.Expires_at = (bearerToken != null && response.Headers.Date != null) ? response.Headers.Date.Value.ToLocalTime().AddSeconds(bearerToken.Expires_in).DateTime : DateTime.Now;

                // set bearer token as default header for all requests
                HttpClientFactory.GetHttpClient().DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken?.Access_token);

                Console.WriteLine($"Bearer Token Generated: Bearer token is valid until, {bearerToken?.Expires_at}");
            }
            catch (Exception e)
            {
                Console.Out.Write("Error Generating Token: " + e);
            }
            
        }
        else
        {
            Console.Out.Write("Token does not need to be generated, already exists with enough time.");
        }
    }

    public string? getBearerToken()
    {
        try
        {
            extractFileContents();
            generateBearerToken();

            return this.bearerToken?.Access_token;
        }
        catch (Exception e)
        {
            throw new Exception("Error Occurred: An error occurred when getting the bearer token. " + e);
        }
    }
}