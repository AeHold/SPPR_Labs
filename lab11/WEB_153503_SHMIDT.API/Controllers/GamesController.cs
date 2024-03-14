using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_153503_SHMIDT.API.Data;
using WEB_153503_SHMIDT.API.Services.CocktailTypeService;
using WEB_153503_SHMIDT.API.Services.CocktailServices;
using WEB_153503_SHMIDT.Domain.Entities;
using WEB_153503_SHMIDT.Domain.Models;

namespace WEB_153503_SHMIDT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CocktailsController : ControllerBase
    {
        private readonly ICocktailService _cocktailService;

        public CocktailsController(ICocktailService cocktailService, IHttpContextAccessor httpContextAccessor)
        {
            _cocktailService = cocktailService;

            var _httpContext = httpContextAccessor.HttpContext!;
            var token = _httpContext.GetTokenAsync("access_token").Result;
        }

        // GET: api/Cocktails
        [HttpGet("{pageNo:int}")]
        [HttpGet("{cocktailType?}/{pageNo:int?}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Cocktail>>> GetCocktails(string? cocktailType, int pageNo = 1, int pageSize = 3)
        {
            var response = await _cocktailService.GetCocktailListAsync(cocktailType, pageNo, pageSize);
            return response.Success ? Ok(response) : BadRequest(response);
        }


        // GET: api/Cocktails/cocktail-5
        [HttpGet("cocktail-{id}")]
        [Authorize]
        public async Task<ActionResult<Cocktail>> GetCocktail(int id)
        {
            var response = await _cocktailService.GetCocktailByIdAsync(id);
            return response.Success ? Ok(response) : NotFound(response);
        }

        // PUT: api/Cocktails/cocktail-5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("cocktail-{id}")]
        public async Task<IActionResult> PutCocktail(int id, Cocktail cocktail)
        {
            try
            {
                await _cocktailService.UpdateCocktailAsync(id, cocktail);
            }
            catch (Exception ex)
            {
                return NotFound(new ResponseData<Cocktail>()
                {
                    Success = false,
                    ErrorMessage = ex.Message
                });
            }

            return Ok(new ResponseData<Cocktail>()
            {
                Data = cocktail,
                Success = true,
            });
        }

        // POST: api/Cocktails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Cocktail>> PostCocktail(Cocktail cocktail)
        {
            var response = await _cocktailService.CreateCocktailAsync(cocktail);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        // DELETE: api/Cocktails/cocktail-5
        [Authorize]
        [HttpDelete("cocktail-{id}")]
        public async Task<IActionResult> DeleteCocktail(int id)
        {
            try
            {
                await _cocktailService.DeleteCocktailAsync(id);
            }
            catch (Exception ex)
            {
                return NotFound(new ResponseData<Cocktail>()
                {
                    Success = false,
                    ErrorMessage = ex.Message
                });
            }

            return NoContent();
        }

        // POST: api/Tools/5
        [Authorize]
        [HttpPost("{id}")]
        public async Task<ActionResult<ResponseData<string>>> PostImage(int id, IFormFile formFile)
        {
            var response = await _cocktailService.SaveImageAsync(id, formFile);
            if (response.Success)
                return Ok(response);

            return NotFound(response);
        }

        private async Task<bool> CocktailExists(int id)
        {
            return (await _cocktailService.GetCocktailByIdAsync(id)).Success;
        }
    }
}
