using System.Text.Json;
using System.Text;
using WEB_153503_SHMIDT.Domain.Models;
using WEB_153503_SHMIDT.Domain.Entities;
using Azure.Core;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace WEB_153503_SHMIDT.Services.CocktailService
{
    public class ApiCocktailService : ICocktailService
    {
        private readonly HttpClient _httpClient;
        private string _pageSize;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly ILogger<ApiCocktailService> _logger;
        private readonly HttpContext _httpContext;

        public ApiCocktailService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiCocktailService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _pageSize = configuration.GetSection("ItemsPerPage").Value!;

            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
            _httpContext = httpContextAccessor.HttpContext!;
            var token = _httpContext.GetTokenAsync("access_token").Result;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        }

        public async Task<ResponseData<ListModel<Cocktail>>> GetCocktailListAsync(string? typeNormalizedName, int pageNo = 1)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}Cocktails/");

            if (typeNormalizedName != null)
            {
                urlString.Append($"{typeNormalizedName}/");
            }

            if (pageNo > 1)
            {
                urlString.Append($"{pageNo}");
            }

            if (!_pageSize.Equals("3"))
            {
                urlString.Append(QueryString.Create("pageSize", _pageSize.ToString()));
            }

            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Cocktail>>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"Error: {ex.Message}");
                    return new ResponseData<ListModel<Cocktail>>
                    {
                        Success = false,
                        ErrorMessage = $"Error: {ex.Message}"
                    };
                }
            }

            _logger.LogError($"No data received from the server. Error: {response.StatusCode}");
            return new ResponseData<ListModel<Cocktail>>()
            {
                Success = false,
                ErrorMessage = $"No data received from the server. Error: {response.StatusCode}"
            };
        }


        public async Task<ResponseData<Cocktail>> CreateCocktailAsync(Cocktail cocktail, IFormFile? formFile)
        {
            var uri = new Uri(_httpClient.BaseAddress!.AbsoluteUri + "Cocktails");
            var response = await _httpClient.PostAsJsonAsync(uri, cocktail, _serializerOptions);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<ResponseData<Cocktail>>(_serializerOptions);
                if (formFile != null)
                {
                    await SaveImageAsync(data!.Data!.Id, formFile);
                }
                return data!;
            }
            _logger.LogError($"Cocktail not created. Error: {response.StatusCode}");

            return new ResponseData<Cocktail>
            {
                Success = false,
                ErrorMessage = $"Cocktail not add. Error: {response.StatusCode}"
            };
        }

        public async Task DeleteCocktailAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_httpClient.BaseAddress!.AbsoluteUri}Cocktails/cocktail-{id}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"No data received from the server. Error: {response.StatusCode}");
            }
        }

        public async Task<ResponseData<Cocktail>> GetCocktailByIdAsync(int id)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}Cocktails/cocktail-{id}");
            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<Cocktail>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"Error: {ex.Message}");
                    return new ResponseData<Cocktail>
                    {
                        Success = false,
                        ErrorMessage = $"Error: {ex.Message}"
                    };
                }
            }
            _logger.LogError($"No data received from the server. Error: {response.StatusCode}");

            return new ResponseData<Cocktail>()
            {
                Success = false,
                ErrorMessage = $"No data received from the server. Error: {response.StatusCode}"
            };
        }


        public async Task UpdateCocktailAsync(int id, Cocktail cocktail, IFormFile? formFile)
        {
            var uri = new Uri(_httpClient.BaseAddress!.AbsoluteUri + "Cocktails/cocktail-" + id);
            var response = await _httpClient.PutAsJsonAsync(uri, cocktail, _serializerOptions);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"No data received from the server. Error: {response.StatusCode}");
            }
            else if (formFile != null)
            {
                int cocktailId = (await response.Content.ReadFromJsonAsync<ResponseData<Cocktail>>(_serializerOptions))!.Data!.Id;
                await SaveImageAsync(cocktailId, formFile);
            }
        }

        private async Task SaveImageAsync(int id, IFormFile image)
        {

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_httpClient.BaseAddress.AbsoluteUri}Cocktails/{id}")
            };
            var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(image.OpenReadStream());
            content.Add(streamContent, "formFile", image.FileName);
            request.Content = content;

            await _httpClient.SendAsync(request);

        }

    }
}
