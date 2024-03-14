using Microsoft.EntityFrameworkCore;
using WEB_153503_SHMIDT.Domain.Entities;

namespace WEB_153503_SHMIDT.API.Data
{
    public class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {

            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await context.Database.MigrateAsync();

            var cocktailTypes = new List<CocktailType>
            {
               new CocktailType { Id = 1, Name = "Легкие коктейли", NormalizedName = "lightCocktails"},
               new CocktailType { Id = 2, Name = "Крепкие коктейли", NormalizedName = "strongCocktails"},
            };

            await context.CocktailTypes.AddRangeAsync(cocktailTypes);
            await context.SaveChangesAsync();


            string imageRoot = $"{app.Configuration["AppUrl"]!}/images";

            var cocktails = new List<Cocktail>
            {

                new Cocktail
                {
                    Id = 1,
                    Name = "Дайкири",
                    Description = "ром + сок лайма + сахар/сахарный сироп",
                    Type = new CocktailType { Id = 1, Name = "Легкие коктейли", NormalizedName = "lightCocktails" },
                    Price = 49.99m,
                    Path = "/images/daikiri.png"
                },
                new Cocktail
                {
                    Id = 2,
                    Name = "Космополитен",
                    Description = "водка + ликер куантро + лимонный сок + клюквенный сок",
                    Type = new CocktailType { Id = 2, Name = "Легкие коктейли", NormalizedName = "lightCocktails" },
                    Price = 39.99m,
                    Path = "/images/cosmopolitan.png"
                },
                new Cocktail
                {
                    Id = 3,
                    Name = "Кайпиринья",
                    Description = "кашаса + сахарный сироп + лайм",
                    Type = new CocktailType { Id = 3, Name = "Легкие коктейли", NormalizedName = "lightCocktails" },
                    Price = 29.99m,
                    Path = "/images/caipirinia.png"
                },
                new Cocktail
                {
                    Id = 4,
                    Name = "Кровавая Мэри",
                    Description = "водка + томатный сок + сок лимона + молотый перец + вустерский соус + табаско + смесь соли с порошком сельдерея",
                    Type = new CocktailType { Id = 4, Name = "Легкие коктейли", NormalizedName = "lightCocktails" },
                    Price = 34.99m,
                    Path = "/images/bloody.png"
                },
                new Cocktail
                {
                    Id = 5,
                    Name = "Лонг-Айленд",
                    Description = "водка + текила + трипл сек + джин + лимонный сок + кока-кола",
                    Type = new CocktailType { Id = 1, Name = "Легкие коктейли", NormalizedName = "lightCocktails" },
                    Price = 59.99m,
                    Path = "/images/long.png"
                },
                new Cocktail
                {
                    Id = 6,
                    Name = "margarita",
                    Description = "белый ром + сахарный сироп + содовая + мята + лайм",
                    Type = new CocktailType { Id = 4, Name = "Легкие коктейли", NormalizedName = "lightCocktails" },
                    Price = 49.99m,
                    Path = "/images/mohito.png"
                },
                new Cocktail
                {
                    Id = 7,
                    Name = "Апероль-шприц",
                    Description = "апероль + просекко + содовая + апельсин + лед в кубиках",
                    Type = new CocktailType { Id = 3, Name = "Крепкие коктейли", NormalizedName = "strongCocktails" },
                    Price = 39.99m,
                    Path = "/images/aperol.png"
                },
                new Cocktail
                {
                    Id = 8,
                    Name = "Беллини",
                    Description = "просекко + сахарный сироп + лимонный сок + персиковое пюре + персик + лед в кубиках",
                    Type = new CocktailType { Id = 2, Name = "Крепкие коктейли", NormalizedName = "strongCocktails" },
                    Price = 29.99m,
                    Path = "/images/belini.png"
                },
                new Cocktail
                {
                    Id = 9,
                    Name = "Френч 75",
                    Description = "лондонский сухой джин + просекко + сахарный сироп + лимонный сок + лед в кубиках",
                    Type = new CocktailType { Id = 2, Name = "Крепкие коктейли", NormalizedName = "strongCocktails" },
                    Price = 29.99m,
                    Path = "/images/french.png"
                },
                new Cocktail
                {
                    Id = 10,
                    Name = "Американо",
                    Description = "красный вермут + красный биттер + содовая + апельсиновая цедра + лед в кубиках",
                    Type = new CocktailType { Id = 2, Name = "Крепкие коктейли", NormalizedName = "strongCocktails" },
                    Price = 29.99m,
                    Path = "/images/americano.png"
                },
                new Cocktail
                {
                    Id = 11,
                    Name = "Беллини",
                    Description = "водка + апельсиновый сок + апельсин + лед в кубиках",
                    Type = new CocktailType { Id = 2, Name = "Крепкие коктейли", NormalizedName = "strongCocktails" },
                    Price = 29.99m,
                    Path = "/images/otvertka.png"
                }

            };

            await context.Cocktails.AddRangeAsync(cocktails);
            await context.SaveChangesAsync();

        }
    }
}
