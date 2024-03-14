using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEB_153503_SHMIDT.Domain.Models;
using WEB_153503_SHMIDT.Extensions;
using WEB_153503_SHMIDT.Services.CocktailService;

namespace WEB_153503_SHMIDT.Controllers
{
    public class CartController : Controller
    {
        private readonly ICocktailService _cocktailService;
        private readonly Cart _cart;
        public CartController(ICocktailService cocktailService, Cart cart)
        {
            _cocktailService = cocktailService;
            _cart = cart;
        }


        [Authorize]
        [Route("[controller]")]
        public IActionResult Index()
        {
            return View(_cart.CartItems);
        }


        [Authorize]
        [Route("[controller]/add/{id:int}")]
        public async Task<IActionResult> Add(int id, string returnUrl)
        {
            var response = await _cocktailService.GetCocktailByIdAsync(id);
            if (response.Success)
            {
                _cart.AddToCart(response.Data!);
            }

            return Redirect(returnUrl);
        }


        [Authorize]
        [Route("[controller]/remove/{id:int}")]
        public async Task<ActionResult> Remove(int id, string returnUrl)
        {
            await Task.Run(() => _cart.RemoveItems(id));
            return Redirect(returnUrl);
        }
    }
}
