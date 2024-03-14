using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_153503_SHMIDT.Domain.Entities
{
    public class Cocktail
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public CocktailType? Type { get; set; }
        public int TypeId { get; set; }
        public decimal Price { get; set; }
        public string? Path { get; set; }
    }
}
