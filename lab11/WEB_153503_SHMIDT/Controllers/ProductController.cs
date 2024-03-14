using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_153503_SHMIDT.Domain.Entities;
using WEB_153503_SHMIDT.Domain.Models;
using WEB_153503_SHMIDT.Extensions;
using WEB_153503_SHMIDT.Services.CocktailTypeService;
using WEB_153503_SHMIDT.Services.CocktailService;

namespace WEB_153503_SHMIDT.Controllers
{

    public class ProductController : Controller
    {
        private ICocktailTypeService _cocktailTypeService;
        private ICocktailService _cocktailService;

        public ProductController(ICocktailService cocktailService, ICocktailTypeService cocktailTypeService)
        {
            _cocktailTypeService = cocktailTypeService;
            _cocktailService = cocktailService;
        }

        [Route("Catalog")]
        [Route("Catalog/{cocktailTypeNormalized?}")]
        public async Task<IActionResult> Index(string? cocktailTypeNormalized, int pageNo = 1)
        {
            var typesResponse = await _cocktailTypeService.GetCocktailTypeListAsync();
            if (!typesResponse.Success)
                return NotFound(typesResponse.ErrorMessage);

            var types = typesResponse.Data;

            ViewData["types"] = types;
            ViewData["currentType"] =
                cocktailTypeNormalized == null ?
                new CocktailType { Name = "Все", NormalizedName = null } :
                types?.SingleOrDefault(g=> g.NormalizedName == cocktailTypeNormalized);


            if (cocktailTypeNormalized == "Все")
                cocktailTypeNormalized = null;

            var cocktailResponse = await _cocktailService.GetCocktailListAsync(cocktailTypeNormalized, pageNo);
            if (!cocktailResponse.Success)
                return NotFound(cocktailResponse.ErrorMessage);

            if (Request.IsAjaxRequest())
            {
                ListModel<Cocktail> data = cocktailResponse.Data!;
                return PartialView("_ProductIndexPartial", new
                {
                    Items = data.Items,
                    CurrentPage = pageNo,
                    TotalPages =  data.TotalPages,
                    CocktailTypeNormalized = cocktailTypeNormalized
                });
            }
            else
            {
                //return View(new ListModel<Cocktail>
                //{
                //    Items = cocktailResponse.Data.Items,
                //    CurrentPage = pageNo,
                //    TotalPages = cocktailResponse.Data.TotalPages,
                //});
                return View(cocktailResponse.Data);
            }
            
        }
    }
}
