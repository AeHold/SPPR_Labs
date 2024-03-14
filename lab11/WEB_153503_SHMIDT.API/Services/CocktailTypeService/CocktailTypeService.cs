using Microsoft.EntityFrameworkCore;
using WEB_153503_SHMIDT.API.Data;
using WEB_153503_SHMIDT.Domain.Entities;
using WEB_153503_SHMIDT.Domain.Models;

namespace WEB_153503_SHMIDT.API.Services.CocktailTypeService
{
    public class CocktailTypeService: ICocktailTypeService
    {
        private readonly AppDbContext _dbContext;

        public CocktailTypeService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResponseData<List<CocktailType>>> GetCocktailTypeListAsync()
        {
            return new ResponseData<List<CocktailType>>
            {
                Data = await _dbContext.CocktailTypes.ToListAsync(),
            };
        }

    }
}
