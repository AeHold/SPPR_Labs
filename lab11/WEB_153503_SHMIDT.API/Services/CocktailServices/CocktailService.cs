using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using WEB_153503_SHMIDT.API.Controllers;
using WEB_153503_SHMIDT.API.Data;
using WEB_153503_SHMIDT.Domain.Entities;
using WEB_153503_SHMIDT.Domain.Models;

namespace WEB_153503_SHMIDT.API.Services.CocktailServices
{
    public class CocktailService : ICocktailService
    {
        public int MaxPageSize = 20;

        private readonly AppDbContext _dbContext;
        private readonly ILogger<CocktailsController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CocktailService(
            AppDbContext dbContext,
            IWebHostEnvironment env,
            IConfiguration configuration,
            ILogger<CocktailsController> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _logger = logger;
            _webHostEnvironment = env;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;            
        }


        public async Task<ResponseData<Cocktail>> CreateCocktailAsync(Cocktail cocktail)
        {

            try
            {
                _dbContext.Cocktails.Add(cocktail);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new ResponseData<Cocktail>
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                };
            }

            return new ResponseData<Cocktail>
            {
                Data = cocktail,
                Success = true,
            };
        }


        public async Task DeleteCocktailAsync(int id)
        {
            var cocktail = await _dbContext.Cocktails.FindAsync(id);

            if (cocktail is null)
                return;


            _dbContext.Cocktails.Remove(cocktail);
            await _dbContext.SaveChangesAsync();
        }


        public async Task<ResponseData<Cocktail>> GetCocktailByIdAsync(int id)
        {
            var cocktail = await _dbContext.Cocktails.FindAsync(id); await _dbContext.Cocktails.Include(g => g.Type).SingleOrDefaultAsync(g => g.Id == id);

            if (cocktail is null)
            {
                return new ResponseData<Cocktail>
                {
                    Success = false,
                    ErrorMessage = "Cocktail not founded",
                };
            }

            return new ResponseData<Cocktail>
            {
                Data = cocktail,
                Success = true,
            };
        }


        public async Task<ResponseData<ListModel<Cocktail>>> GetCocktailListAsync(string? typeNormalizedName, int pageNo = 1, int pageSize = 3)
        {
            if (pageSize > MaxPageSize)
                pageSize = MaxPageSize;

            var query = _dbContext.Cocktails.AsQueryable();
            var dataList = new ListModel<Cocktail>();
            query = query.Where(d => typeNormalizedName == null || d.Type!.NormalizedName.Equals(typeNormalizedName));

            var count = await query.CountAsync();

            if (count == 0)
            {
                return new ResponseData<ListModel<Cocktail>>
                {
                    Data = dataList,
                    Success = true,
                };
            }

            int totalPages =
                    count % pageSize == 0 ?
                    count / pageSize :
                    count / pageSize + 1;

            if (pageNo > totalPages)
            {
                return new ResponseData<ListModel<Cocktail>>
                {
                    Success = false,
                    ErrorMessage = "No such page"
                };
            }

            dataList.Items = await query.Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync();
            dataList.CurrentPage = pageNo;
            dataList.TotalPages = totalPages;

            return new ResponseData<ListModel<Cocktail>>
            {
                Data = dataList,
                Success = true,
            };
        }


        public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
        {
            var responseData = new ResponseData<string>();
            var cocktail = await _dbContext.Cocktails.FindAsync(id);
            if (cocktail == null)
            {
                responseData.Success = false;
                responseData.ErrorMessage = "No item found";
                return responseData;
            }

            var host = "https://" + _httpContextAccessor.HttpContext.Request.Host;

            var imageFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");

            if (formFile != null)
            {
                if (!string.IsNullOrEmpty(cocktail.Path))
                {
                    var prevImage = Path.GetFileName(cocktail.Path);
                    var prevImagePath = Path.Combine(imageFolder, prevImage);
                    if (File.Exists(prevImagePath))
                    {
                        File.Delete(prevImagePath);
                    }
                }

                var ext = Path.GetExtension(formFile.FileName);
                var fName = Path.ChangeExtension(Path.GetRandomFileName(), ext);
                var filePath = Path.Combine(imageFolder, fName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }

                cocktail.Path = $"{host}/images/{fName}";
                await _dbContext.SaveChangesAsync();
            }
            responseData.Data = cocktail.Path;
            return responseData;
        }


        public async Task UpdateCocktailAsync(int id, Cocktail cocktail)
        {
            var updatingCocktail = await _dbContext.Cocktails.FindAsync(id);

            if (updatingCocktail is null)
                return;

            updatingCocktail.Name = cocktail.Name;
            updatingCocktail.Description = cocktail.Description;
            updatingCocktail.Price = cocktail.Price;
            updatingCocktail.Path = cocktail.Path;
            updatingCocktail.TypeId = cocktail.TypeId;

            await _dbContext.SaveChangesAsync();
        }

    }
}
