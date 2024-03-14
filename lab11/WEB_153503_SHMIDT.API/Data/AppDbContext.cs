using Microsoft.EntityFrameworkCore;
using WEB_153503_SHMIDT.Domain.Entities;

namespace WEB_153503_SHMIDT.API.Data
{
    public class AppDbContext: DbContext
    {
        public DbSet<Cocktail> Cocktails { get; set; }

        public DbSet<CocktailType> CocktailTypes { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {

        }
    }
}
