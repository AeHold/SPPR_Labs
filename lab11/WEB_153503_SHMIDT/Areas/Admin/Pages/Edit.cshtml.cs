using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_153503_SHMIDT.Domain.Entities;
using WEB_153503_SHMIDT.Services.CocktailService;

namespace WEB_153503_SHMIDT.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly ICocktailService _cocktailService;

        public EditModel(ICocktailService cocktailService)
        {
            _cocktailService = cocktailService;
        }

        [BindProperty]
        public Cocktail Cocktail { get; set; } = default!;

        [BindProperty]
        public IFormFile? Image { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var response = await _cocktailService.GetCocktailByIdAsync(id.Value);

            if (!response.Success)
                return NotFound();

            Cocktail = response.Data!;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            await _cocktailService.UpdateCocktailAsync(Cocktail.Id, Cocktail, Image);

            return RedirectToPage("./Index");
        }

        private async Task<bool> CocktailExists(int id)
        {
            var response = await _cocktailService.GetCocktailByIdAsync(id);
            return response.Success;
        }
    }
}
