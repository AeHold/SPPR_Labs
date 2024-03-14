using WEB_153503_SHMIDT.Domain.Entities;
using WEB_153503_SHMIDT.Domain.Models;

namespace WEB_153503_SHMIDT.Services.CocktailTypeService
{
    public class MemoryCocktailTypeService : ICocktailTypeService
    {
        public Task<ResponseData<List<CocktailType>>> GetCocktailTypeListAsync()
        {
            var cocktailTypes = new List<CocktailType>
            {
                new CocktailType { Id = 1, Name = "Легкие коктейли", NormalizedName = "lightCocktails"},
                new CocktailType { Id = 2, Name = "Крепкие коктейли", NormalizedName = "strongCocktails"},
            };
            var result = new ResponseData<List<CocktailType>>();
            result.Data = cocktailTypes;
            return Task.FromResult(result);
        }
    }
}
