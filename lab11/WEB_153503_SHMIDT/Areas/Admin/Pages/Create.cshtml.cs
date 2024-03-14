using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_153503_SHMIDT.Domain.Entities;
using WEB_153503_SHMIDT.Services.CocktailService;

namespace WEB_153503_SHMIDT.Areas.Admin.Pages
{
    public class CreateModel : PageModel
    {
        private readonly ICocktailService _cocktailService;

        public CreateModel(ICocktailService cocktailService)
        {
            _cocktailService = cocktailService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Cocktail Cocktail { get; set; } = default!;

        [BindProperty]
        public IFormFile? Image { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var responce = await _cocktailService.CreateCocktailAsync(Cocktail, Image);

            if (!responce.Success)
                return Page();

            return RedirectToPage("./Index");
        }
    }
}
