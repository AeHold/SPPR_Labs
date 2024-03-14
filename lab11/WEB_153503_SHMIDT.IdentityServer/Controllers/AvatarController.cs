using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using WEB_153503_SHMIDT.IdentityServer.Models;

namespace WEB_153503_SHMIDT.IdentityServer.Controllers
{
    [Route("[controller]")]
    [Authorize] //
    [ApiController]
    public class AvatarController : ControllerBase
    {
        private IWebHostEnvironment _environment;
        private readonly UserManager<ApplicationUser> _userManager;

        public AvatarController(IWebHostEnvironment environment, UserManager<ApplicationUser> userManager)
        {
            _environment = environment;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Get()
        {
            var userId = _userManager.GetUserId(User);
            var imagesPath = Path.Combine(_environment.ContentRootPath, "Images");

            // Поиск всех файлов с расширениями изображений для данного пользователя
            var imageFiles = Directory.GetFiles(imagesPath, $"{userId}.*");

            if (imageFiles.Length > 0)
            {
                // Выбираем первый найденный файл 
                var imagePath = imageFiles[0];

                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(imagePath, out var contentType))
                {
                    contentType = "application/octet-stream";
                }

                var fileBytes = await System.IO.File.ReadAllBytesAsync(imagePath);
                return File(fileBytes, contentType);
            }
            else
            {
                // Если изображений нет, возвращаем файл-заменитель
                var placeholderPath = Path.Combine(imagesPath, "default-profile-picture.png");
                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(placeholderPath, out var contentType))
                {
                    contentType = "application/octet-stream";
                }

                var placeholderBytes = await System.IO.File.ReadAllBytesAsync(placeholderPath);
                return File(placeholderBytes, contentType);
            }
        }
    }
}
