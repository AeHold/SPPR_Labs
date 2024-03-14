using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_153503_SHMIDT.Domain.Entities;

namespace WEB_153503_SHMIDT.Domain.Models
{
    public class CartItem
    {
        public Cocktail Cocktail { get; set; } = null!;
        public int Count { get; set; }
    }
}
