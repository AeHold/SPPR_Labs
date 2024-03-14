using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_153503_SHMIDT.API.Data;
using WEB_153503_SHMIDT.API.Services.CocktailTypeService;
using WEB_153503_SHMIDT.Domain.Entities;
using WEB_153503_SHMIDT.Domain.Models;

namespace WEB_153503_SHMIDT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CocktailTypesController : ControllerBase
    {
        private readonly ICocktailTypeService _cocktailTypeService;

        public CocktailTypesController(ICocktailTypeService cocktailTypeService)
        {
            _cocktailTypeService = cocktailTypeService;
        }

        // GET: api/CocktailTypes
        [HttpGet]
        public async Task<ActionResult<ResponseData<CocktailType>>> GetCocktailTypes()
        {
            var responce = await _cocktailTypeService.GetCocktailTypeListAsync();
            return responce.Success ? Ok(responce) : BadRequest(responce);
        }

    }
}
