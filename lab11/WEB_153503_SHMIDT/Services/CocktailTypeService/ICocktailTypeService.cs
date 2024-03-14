using WEB_153503_SHMIDT.Domain.Models;
using WEB_153503_SHMIDT.Domain.Entities;

namespace WEB_153503_SHMIDT.Services.CocktailTypeService
{
    public interface ICocktailTypeService
    {
        public Task<ResponseData<List<CocktailType>>> GetCocktailTypeListAsync();
    }
}
