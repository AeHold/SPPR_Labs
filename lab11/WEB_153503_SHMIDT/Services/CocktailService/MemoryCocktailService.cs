using Microsoft.AspNetCore.Mvc;
using WEB_153503_SHMIDT.Domain.Entities;
using WEB_153503_SHMIDT.Domain.Models;
using WEB_153503_SHMIDT.Services.CocktailTypeService;

namespace WEB_153503_SHMIDT.Services.CocktailService
{
    public class MemoryCocktailService : ICocktailService
    {
        private List<Cocktail> _cocktails = new();
        private List<CocktailType> _types = new();
        private IConfiguration _config;

        public MemoryCocktailService([FromServices] IConfiguration config, ICocktailTypeService cocktailTypeService)
        {
            _config  = config;
            SetupData();
        }


        public Task<ResponseData<Cocktail>> CreateCocktailAsync(Cocktail cocktail, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCocktailAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Cocktail>> GetCocktailByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<ListModel<Cocktail>>> GetCocktailListAsync(string? typeNormalizedName, int pageNo = 1)
        {
            var filteredCocktails =
                typeNormalizedName != null ?
                _cocktails.Where(cocktail => cocktail.Type?.NormalizedName == typeNormalizedName).ToList() :
                _cocktails;

            int itemsPerPage = _config.GetValue<int>("ItemsPerPage");

            int totalPages =
                filteredCocktails.Count() % itemsPerPage == 0 ?
                filteredCocktails.Count() / itemsPerPage :
                filteredCocktails.Count() / itemsPerPage + 1;         


            var responseData = new ResponseData<ListModel<Cocktail>>
            {
                Data = new ListModel<Cocktail>
                {
                    Items = filteredCocktails.Skip((pageNo - 1) * itemsPerPage).Take(itemsPerPage).ToList(),
                    CurrentPage = pageNo,
                    TotalPages = totalPages,
                }
            };

            return Task.FromResult(responseData);
        }

        public Task UpdateCocktailAsync(int id, Cocktail cocktail, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        private void SetupData()
        {
            _cocktails = new List<Cocktail>
            {
                new Cocktail
                {
                    Id = 1,
                    Name = "Дайкири",
                    Description = "ром + сок лайма + сахар/сахарный сироп",
                    Type = new CocktailType { Id = 1, Name = "Легкие коктейли", NormalizedName = "lightCocktails" },
                    Price = 49.99m,
                    Path = "/images/daikiri.png"
                },
                new Cocktail
                {
                    Id = 2,
                    Name = "Космополитен",
                    Description = "водка + ликер куантро + лимонный сок + клюквенный сок",
                    Type = new CocktailType { Id = 2, Name = "Легкие коктейли", NormalizedName = "lightCocktails" },
                    Price = 39.99m,
                    Path = "/images/cosmopolitan.png"
                },
                new Cocktail
                {
                    Id = 3,
                    Name = "Кайпиринья",
                    Description = "кашаса + сахарный сироп + лайм",
                    Type = new CocktailType { Id = 3, Name = "Легкие коктейли", NormalizedName = "lightCocktails" },
                    Price = 29.99m,
                    Path = "/images/caipirinia.png"
                },
                new Cocktail
                {
                    Id = 4,
                    Name = "Кровавая Мэри",
                    Description = "водка + томатный сок + сок лимона + молотый перец + вустерский соус + табаско + смесь соли с порошком сельдерея",
                    Type = new CocktailType { Id = 4, Name = "Легкие коктейли", NormalizedName = "lightCocktails" },
                    Price = 34.99m,
                    Path = "/images/bloody.png"
                },
                new Cocktail
                {
                    Id = 5,
                    Name = "Лонг-Айленд",
                    Description = "водка + текила + трипл сек + джин + лимонный сок + кока-кола",
                    Type = new CocktailType { Id = 1, Name = "Легкие коктейли", NormalizedName = "lightCocktails" },
                    Price = 59.99m,
                    Path = "/images/long.png"
                },
                new Cocktail
                {
                    Id = 6,
                    Name = "margarita",
                    Description = "белый ром + сахарный сироп + содовая + мята + лайм",
                    Type = new CocktailType { Id = 4, Name = "Легкие коктейли", NormalizedName = "lightCocktails" },
                    Price = 49.99m,
                    Path = "/images/mohito.png"
                },
                new Cocktail
                {
                    Id = 7,
                    Name = "Апероль-шприц",
                    Description = "апероль + просекко + содовая + апельсин + лед в кубиках",
                    Type = new CocktailType { Id = 3, Name = "Крепкие коктейли", NormalizedName = "strongCocktails" },
                    Price = 39.99m,
                    Path = "/images/aperol.png"
                },
                new Cocktail
                {
                    Id = 8,
                    Name = "Беллини",
                    Description = "просекко + сахарный сироп + лимонный сок + персиковое пюре + персик + лед в кубиках",
                    Type = new CocktailType { Id = 2, Name = "Крепкие коктейли", NormalizedName = "strongCocktails" },
                    Price = 29.99m,
                    Path = "/images/belini.png"
                },
                new Cocktail
                {
                    Id = 9,
                    Name = "Френч 75",
                    Description = "лондонский сухой джин + просекко + сахарный сироп + лимонный сок + лед в кубиках",
                    Type = new CocktailType { Id = 2, Name = "Крепкие коктейли", NormalizedName = "strongCocktails" },
                    Price = 29.99m,
                    Path = "/images/french.png"
                },
                new Cocktail
                {
                    Id = 10,
                    Name = "Американо",
                    Description = "красный вермут + красный биттер + содовая + апельсиновая цедра + лед в кубиках",
                    Type = new CocktailType { Id = 2, Name = "Крепкие коктейли", NormalizedName = "strongCocktails" },
                    Price = 29.99m,
                    Path = "/images/americano.png"
                },
                new Cocktail
                {
                    Id = 11,
                    Name = "Беллини",
                    Description = "водка + апельсиновый сок + апельсин + лед в кубиках",
                    Type = new CocktailType { Id = 2, Name = "Крепкие коктейли", NormalizedName = "strongCocktails" },
                    Price = 29.99m,
                    Path = "/images/otvertka.png"
                }


            };
        }
    }
}
