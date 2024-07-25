namespace McenterTest.Services.Requests.Models;

public class TokenFile
{
    public string IdentityServiceUrl { get; set; } = string.Empty;
    public string IdentityServer4Client { get; set; } = string.Empty;
    public string Id { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public List<string> OpenApiRights { get; set; } = new List<string>();
}