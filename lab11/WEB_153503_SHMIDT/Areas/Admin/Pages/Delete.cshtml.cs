using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_153503_SHMIDT.Domain.Entities;
using WEB_153503_SHMIDT.Services.CocktailService;

namespace WEB_153503_SHMIDT.Areas.Admin.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly ICocktailService _cocktailService;

        public DeleteModel(ICocktailService cocktailService)
        {
            _cocktailService = cocktailService;
        }

        [BindProperty]
        public Cocktail Cocktail { get; set; } = default!;

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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
                return NotFound();

            await _cocktailService.DeleteCocktailAsync(id.Value);

            return RedirectToPage("./Index");
        }
    }
}
