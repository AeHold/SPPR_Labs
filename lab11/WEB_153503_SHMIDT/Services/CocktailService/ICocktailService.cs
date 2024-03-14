using System.Threading.Tasks;
using WEB_153503_SHMIDT.Domain.Models;
using WEB_153503_SHMIDT.Domain.Entities;

namespace WEB_153503_SHMIDT.Services.CocktailService
{
    public interface ICocktailService
    {
        /// <summary>
        /// Получение списка всех объектов
        /// </summary>
        /// <param name="typeNormalizedName">нормализованное имя категории для фильтрации</param>
        /// <param name="pageNo">номер страницы списка</param>
        /// <returns></returns>
        public Task<ResponseData<ListModel<Cocktail>>> GetCocktailListAsync(string? typeNormalizedName, int pageNo = 1);

        /// <summary>
        /// Поиск объекта по Id
        /// </summary>
        /// <param name="id">Идентификатор объекта</param>
        /// <returns>Найденный объект или null, если объект не найден</returns>
        public Task<ResponseData<Cocktail>> GetCocktailByIdAsync(int id);

        /// <summary>
        /// Обновление объекта
        /// </summary>
        /// <param name="id">Id изменяемомго объекта</param>
        /// <param name="cocktail">объект с новыми параметрами</param>
        /// <param name="formFile">Файл изображения</param>
        /// <returns></returns>
        public Task UpdateCocktailAsync(int id, Cocktail cocktail, IFormFile? formFile);

        /// <summary>
        /// Удаление объекта
        /// </summary>
        /// <param name="id">Id удаляемомго объекта</param>
        /// <returns></returns>
        public Task DeleteCocktailAsync(int id);

        /// <summary>
        /// Создание объекта
        /// </summary>
        /// <param name="cocktail">Новый объект</param>
        /// <param name="formFile">Файл изображения</param>
        /// <returns>Созданный объект</returns>
        public Task<ResponseData<Cocktail>> CreateCocktailAsync(Cocktail cocktail, IFormFile?formFile);

    }
}
