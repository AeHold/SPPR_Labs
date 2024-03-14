using Microsoft.AspNetCore.Mvc;

namespace WEB_153503_SHMIDT.ViewComponents
{
    public class CartComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
