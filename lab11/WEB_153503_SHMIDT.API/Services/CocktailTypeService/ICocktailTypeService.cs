using WEB_153503_SHMIDT.Domain.Entities;
using WEB_153503_SHMIDT.Domain.Models;

namespace WEB_153503_SHMIDT.API.Services.CocktailTypeService
{
    public interface ICocktailTypeService
    {
        /// <summary>
        /// Получение списка всех категорий
        /// </summary>
        /// <returns></returns>
        public Task<ResponseData<List<CocktailType>>> GetCocktailTypeListAsync();
    }
}
