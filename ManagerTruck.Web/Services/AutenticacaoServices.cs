using Newtonsoft.Json;
using System.Text;
using ManagerTruck.Web.Dtos;

public class AutenticacaoServices
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private string urlBase = string.Empty;

    public AutenticacaoServices(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        urlBase = _configuration["baseApiUrl"];
        
    }

    public async Task<HttpResponseMessage> LoginAsync(LoginViewModel login)
    {
        var json = JsonConvert.SerializeObject(login);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await _httpClient.PostAsync($"{urlBase}/auth/login", content);
    }

    public async Task<HttpResponseMessage> CreateAsync(LoginViewModel login)
    {
        var json = JsonConvert.SerializeObject(login);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await _httpClient.PostAsync($"{urlBase}/auth/create", content);
    }

}