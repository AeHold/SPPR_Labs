using System.Text.Json;
using System.Text;
using WEB_153503_SHMIDT.Domain.Models;
using WEB_153503_SHMIDT.Domain.Entities;

namespace WEB_153503_SHMIDT.Services.CocktailTypeService
{
    public class ApiCocktailTypeService : ICocktailTypeService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiCocktailTypeService> _logger;
        private readonly JsonSerializerOptions _serializerOptions;


        public ApiCocktailTypeService(HttpClient httpClient, ILogger<ApiCocktailTypeService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }


        public async Task<ResponseData<List<CocktailType>>> GetCocktailTypeListAsync()
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress?.AbsoluteUri}CocktailTypes/");
            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<List<CocktailType>>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"Error: {ex.Message}");
                    return new ResponseData<List<CocktailType>>
                    {
                        Success = false,
                        ErrorMessage = $"Error: { ex.Message}",
                    };

                }
            }
            _logger.LogError($"No data received from the server. Error: {response.StatusCode}");
            return new ResponseData<List<CocktailType>>()
            {
                Success = false,
                ErrorMessage = $"No data received from the server. Error: {response.StatusCode}"
            };
        }
    }
}
