using System.Text.Json;
using System.Text;
using WEB_153503_SHMIDT.Domain.Models;
using WEB_153503_SHMIDT.Domain.Entities;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Headers;

namespace WEB_153503_SHMIDT.BlazorWasm.Services
{
    public class DataService : IDataService
    {
        public event Action DataChanged;
        private readonly HttpClient _httpClient;
        private readonly IAccessTokenProvider _accessTokenProvider;
		private readonly ILogger<DataService> _logger;
		private readonly int _pageSize = 3;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public DataService(HttpClient httpClient, IConfiguration configuration, IAccessTokenProvider accessTokenProvider, ILogger<DataService> logger)
        {
            _httpClient = httpClient;
			_logger = logger;
			_accessTokenProvider = accessTokenProvider;
            _pageSize = configuration.GetSection("PageSize").Get<int>();
            _jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public List<CocktailType> Types { get; set; }
        public List<Cocktail> CocktailList { get; set; }
        public bool Success { get; set; } = true;
        public string ErrorMessage { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }

        public async Task GetCocktailListAsync(string? typeNormalizedName, int pageNo = 1)
        {
            var tokenRequest = await _accessTokenProvider.RequestAccessToken();
            if (tokenRequest.TryGetToken(out var token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
                var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}api/Cocktails/");
                if (typeNormalizedName != null)
                {
                    urlString.Append($"{typeNormalizedName}/");
                };
                if (pageNo > 1)
                {
                    urlString.Append($"{pageNo}");
                };
                if (!_pageSize.Equals("3"))
                {
                    urlString.Append(QueryString.Create("pageSize", _pageSize.ToString()));
                }

                var ff = new Uri(urlString.ToString());

				var response = await _httpClient.GetAsync(ff);
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var responseData = await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Cocktail>>>(_jsonSerializerOptions);
                        CocktailList = responseData?.Data?.Items;
                        TotalPages = responseData?.Data?.TotalPages ?? 0;
                        CurrentPage = responseData?.Data?.CurrentPage ?? 0;
                        DataChanged?.Invoke();
						_logger.LogInformation("<------ Clothes list received successfully ------>");
					}
                    catch (JsonException ex)
                    {
                        Success = false;
                        ErrorMessage = $"Ошибка: {ex.Message}";
                    }
                }
                else
                {
                    Success = false;
                    ErrorMessage = $"Данные не получены от сервера. Error:{response.StatusCode}";
                }
            }
     
        }

        public async Task<Cocktail?> GetCocktailByIdAsync(int id)
        {
            var tokenRequest = await _accessTokenProvider.RequestAccessToken();
            if (tokenRequest.TryGetToken(out var token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
                var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}api/cocktails/{id}");
                var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        return (await response.Content.ReadFromJsonAsync<ResponseData<Cocktail>>(_jsonSerializerOptions))?.Data;
                    }
                    catch (JsonException ex)
                    {
                        Success = false;
                        ErrorMessage = $"Ошибка: {ex.Message}";
                        return null;
                    }
                }
                Success = false;
                ErrorMessage = $"Данные не получены от сервера. Error:{response.StatusCode}";
            }
            return null;          
        }

        public async Task GetTypeListAsync()
        {
            var tokenRequest = await _accessTokenProvider.RequestAccessToken();
            if (tokenRequest.TryGetToken(out var token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
                var urlString = new StringBuilder($"{_httpClient.BaseAddress?.AbsoluteUri}api/CocktailTypes/");
                var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var responseData = await response.Content.ReadFromJsonAsync<ResponseData<List<CocktailType>>>(_jsonSerializerOptions);
                        Types = responseData?.Data;
                    }
                    catch (JsonException ex)
                    {
                        Success = false;
                        ErrorMessage = $"Ошибка: {ex.Message}";
                    }
                }
                else
                {
                    Success = false;
                    ErrorMessage = $"Данные не получены от сервера. Error:{response.StatusCode}";
                }
            }           
        }
    }
}
