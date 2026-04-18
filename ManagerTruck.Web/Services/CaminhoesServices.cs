using ManagerTruck.Web.Dtos;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;


public class CaminhoesServices
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private string urlBase = string.Empty;

    public CaminhoesServices(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        urlBase = _configuration["baseApiUrl"];
    }

    public void SetAuthorizationHeader(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<HttpResponseMessage> GetCaminhoesAsync()
    {
        return await _httpClient.GetAsync($"{urlBase}/caminhoes");
    }

    public async Task<HttpResponseMessage> GetCaminhoesByIdAsync(int id)
    {
        return await _httpClient.GetAsync($"{urlBase}/caminhoes/{id}");
    }

    public async Task<HttpResponseMessage> CreateCaminhoesAsync(CaminhaoViewModel caminhaoDto)
    {
        var json = JsonConvert.SerializeObject(caminhaoDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await _httpClient.PostAsync($"{urlBase}/caminhoes", content);
    }

    public async Task<HttpResponseMessage> UpdateCaminhoesAsync(CaminhaoViewModel caminhaoDto)
    {
        var json = JsonConvert.SerializeObject(caminhaoDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await _httpClient.PutAsync($"{urlBase}/caminhoes/{caminhaoDto.Id}", content);
    }

    public async Task<HttpResponseMessage> DeleteCaminhoesAsync(int id)
    {
        return await _httpClient.DeleteAsync($"{urlBase}/caminhoes/{id}");
    }

    public async Task<HttpResponseMessage> CreateManutencaoCaminhoesAsync(ManutencaoInput input)
    {
        var json = JsonConvert.SerializeObject(input);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await _httpClient.PostAsync($"{urlBase}/caminhoes/manutencao/",content);
    }
}