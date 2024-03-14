using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEB_153503_SHMIDT.Domain.Models;
using WEB_153503_SHMIDT.Extensions;

namespace WEB_153503_SHMIDT.Components
{
    public class CartViewComponent: ViewComponent
    {
        private readonly Cart _cart;

        public CartViewComponent(Cart cart)
        {
            _cart = cart;
        }

        public IViewComponentResult Invoke()
        {
            return View(_cart);
        }
    }
}
