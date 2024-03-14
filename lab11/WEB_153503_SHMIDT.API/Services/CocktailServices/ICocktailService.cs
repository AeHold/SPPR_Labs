using WEB_153503_SHMIDT.Domain.Entities;
using WEB_153503_SHMIDT.Domain.Models;

namespace WEB_153503_SHMIDT.API.Services.CocktailServices
{
    public interface ICocktailService
    {
        /// <summary>
        /// Получение списка всех объектов
        /// </summary>
        /// <param name="typeNormalizedName">нормализованное имя категории для фильтрации</param>
        /// <param name="pageNo">номер страницы списка</param>
        /// <param name="pageSize">количество объектов на странице</param>
        /// <returns></returns>
        public Task<ResponseData<ListModel<Cocktail>>> GetCocktailListAsync(string? typeNormalizedName, int pageNo = 1, int pageSize = 3);


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
        /// <returns></returns>
        public Task UpdateCocktailAsync(int id, Cocktail cocktail);


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
        /// <returns>Созданный объект</returns>
        public Task<ResponseData<Cocktail>> CreateCocktailAsync(Cocktail cocktail);


        /// <summary>
        /// Сохранить файл изображения для объекта
        /// </summary>
        /// <param name="id">Id объекта</param>
        /// <param name="formFile">файл изображения</param>
        /// <returns>Url к файлу изображения</returns
        public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile);
    }
}
