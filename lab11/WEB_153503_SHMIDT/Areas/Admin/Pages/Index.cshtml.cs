using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WEB_153503_SHMIDT.Domain.Entities;
using WEB_153503_SHMIDT.Services.CocktailService;

namespace WEB_153503_SHMIDT.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ICocktailService _cocktailService;

        public IndexModel(ICocktailService cocktailService)
        {
            _cocktailService = cocktailService;
        }

        public IList<Cocktail> Cocktail { get;set; } = default!;
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; } 

        public async Task<IActionResult> OnGetAsync(int pageNo = 1)
        {
            var response = await _cocktailService.GetCocktailListAsync(null, pageNo);

            if (!response.Success)
                return NotFound();

            Cocktail = response.Data?.Items!;
            CurrentPage = response.Data?.CurrentPage ?? 0;
            TotalPages = response.Data?.TotalPages ?? 0;

            return Page();
        }
    }
}
