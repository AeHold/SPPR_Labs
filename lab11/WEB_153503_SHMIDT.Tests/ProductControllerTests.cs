using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_153503_SHMIDT.Services.CocktailTypeService;
using WEB_153503_SHMIDT.Services.CocktailService;
using WEB_153503_SHMIDT.Controllers;
using WEB_153503_SHMIDT.Domain.Entities;
using WEB_153503_SHMIDT.Domain.Models;


namespace WEB_153503_SHMIDT.Tests
{
    public class ProductControllerTests
    {
        [Fact]
        public void IndexReturns404WhenTypesAreReceivedUnsuccessfully()
        {
            var typeService = new Mock<ICocktailTypeService>();
            typeService.Setup(m => m.GetCocktailTypeListAsync())
                .ReturnsAsync(new ResponseData<List<CocktailType>> { Success = false });

            var cocktailService = new Mock<ICocktailService>();
            cocktailService.Setup(m => m.GetCocktailListAsync(It.IsAny<string?>(), It.IsAny<int>()))
                .ReturnsAsync(new ResponseData<ListModel<Cocktail>> { Success = true });

            var controller = new ProductController(cocktailService.Object, typeService.Object);

            var result = controller.Index(null).Result;

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public void IndexReturns404WhenCocktailsAreReceivedUnsuccessfully()
        {
            var typeService = new Mock<ICocktailTypeService>();
            typeService.Setup(m => m.GetCocktailTypeListAsync())
                .ReturnsAsync(new ResponseData<List<CocktailType>> { Success = true });

            var cocktailService = new Mock<ICocktailService>();
            cocktailService.Setup(m => m.GetCocktailListAsync(It.IsAny<string?>(), It.IsAny<int>()))
                .ReturnsAsync(new ResponseData<ListModel<Cocktail>> { Success = false });

            var controller = new ProductController(cocktailService.Object, typeService.Object);

            var result = controller.Index(null).Result;

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public void IndexViewDataContainsValidCurrentTypeWhenTypeParameterIsNull()
        {
            var typeService = new Mock<ICocktailTypeService>();
            typeService.Setup(m => m.GetCocktailTypeListAsync())
                .ReturnsAsync(new ResponseData<List<CocktailType>> { Success = true });

            var cocktailService = new Mock<ICocktailService>();
            cocktailService.Setup(m => m.GetCocktailListAsync(It.IsAny<string?>(), It.IsAny<int>()))
                .ReturnsAsync(new ResponseData<ListModel<Cocktail>> { Success = true });

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(c => c.Request.Headers).Returns(new HeaderDictionary());

            var controller = new ProductController(cocktailService.Object, typeService.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext.Object
                }
            };

            var result = controller.Index(null).Result;

            var viewResult = Assert.IsType<ViewResult>(result);
            var fff = viewResult.ViewData["currentType"] as CocktailType;
            Assert.Null((viewResult.ViewData["currentType"] as CocktailType)!.NormalizedName);
        }

        [Fact]
        public void IndexViewDataContainsValidCurrentTypeWhenTypeParameterIsNotNull()
        {
            var testTypes = new List<CocktailType>
            {
                new CocktailType { Id = 1, Name = "Шутер", NormalizedName = "shooter"},
                new CocktailType { Id = 2, Name = "Гонки", NormalizedName = "race"},
            };

            var testCocktails = new List<Cocktail>
            {
                new Cocktail
                {
                    Id = 1,
                    Name = "Call of Duty: Modern Warfare",
                    Description = "Самый реалистичный шутер от первого лица",
                    TypeId = 1,
                    Price = 49.99m,
                },
                new Cocktail
                {
                    Id = 2,
                    Name = "Need for Speed: Heat",
                    Description = "Горячие гонки по улицам ночного города",
                    TypeId = 2,
                    Price = 39.99m,
                },         
            };

            var typeService = new Mock<ICocktailTypeService>();
            typeService.Setup(m => m.GetCocktailTypeListAsync())
                .ReturnsAsync(new ResponseData<List<CocktailType>>
                {
                    Success = true,
                    Data = testTypes
                });

            var cocktailService = new Mock<ICocktailService>();
            cocktailService.Setup(m => m.GetCocktailListAsync(It.IsAny<string?>(), It.IsAny<int>()))
                .ReturnsAsync(new ResponseData<ListModel<Cocktail>>
                {
                    Success = true,
                    Data = new() { Items = testCocktails }
                });


            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(c => c.Request.Headers).Returns(new HeaderDictionary());

            var controller = new ProductController(cocktailService.Object, typeService.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext.Object
                }
            };

            var result = controller.Index("shooter").Result;

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(testTypes.First(c => c.NormalizedName == "shooter"),(viewResult.ViewData["currentType"] as CocktailType));
        }

        [Fact]
        public void IndexRightModel()
        {
            var testTypes = new List<CocktailType>
            {
                new CocktailType { Id = 1, Name = "Шутер", NormalizedName = "shooter"},
                new CocktailType { Id = 2, Name = "Гонки", NormalizedName = "race"},
            };

            var testCocktails = new List<Cocktail>
            {
                new Cocktail
                {
                    Id = 1,
                    Name = "Call of Duty: Modern Warfare",
                    Description = "Самый реалистичный шутер от первого лица",
                    TypeId = 1,
                    Price = 49.99m,
                },
                new Cocktail
                {
                    Id = 2,
                    Name = "Need for Speed: Heat",
                    Description = "Горячие гонки по улицам ночного города",
                    TypeId = 2,
                    Price = 39.99m,
                },
            };

            var typeService = new Mock<ICocktailTypeService>();
            typeService.Setup(m => m.GetCocktailTypeListAsync())
                .ReturnsAsync(new ResponseData<List<CocktailType>>
                {
                    Success = true,
                    Data = testTypes
                });

            var model = new ListModel<Cocktail>() { Items = testCocktails };
            var cocktailService = new Mock<ICocktailService>();
            cocktailService.Setup(m => m.GetCocktailListAsync(It.IsAny<string?>(), It.IsAny<int>()))
                .ReturnsAsync(new ResponseData<ListModel<Cocktail>>
                {
                    Success = true,
                    Data = model
                });


            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(c => c.Request.Headers).Returns(new HeaderDictionary());

            var controller = new ProductController(cocktailService.Object, typeService.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext.Object
                }
            };

            var result = controller.Index(null).Result;

            var viewResult = Assert.IsType<ViewResult>(result);
            var modelResult = Assert.IsType<ListModel<Cocktail>>(viewResult.Model);
            Assert.Equal(model, modelResult);
        }
    }

}