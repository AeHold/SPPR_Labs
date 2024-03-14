using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_153503_SHMIDT.Domain.Entities;

namespace WEB_153503_SHMIDT.Domain.Models
{
    public class Cart
    {
        /// <summary>
        /// Список объектов в корзине
        /// key - идентификатор объекта
        /// </summary>
        public Dictionary<int, CartItem> CartItems { get; set; } = new();


        /// <summary>
        /// Добавить объект в корзину
        /// </summary>
        /// <param name="dish">Добавляемый объект</param>
        public virtual void AddToCart(Cocktail cocktail)
        {
            if (CartItems.ContainsKey(cocktail.Id))
            {
                CartItems[cocktail.Id].Count++;
            }
            else
            {
                CartItems[cocktail.Id] = new CartItem()
                {
                    Cocktail = cocktail,
                    Count = 1
                };
            }
        }


        /// <summary>
        /// Удалить объект из корзины
        /// </summary>
        /// <param name="id"> id удаляемого объекта</param>
        public virtual void RemoveItems(int id)
        {
            CartItems.Remove(id);
        }


        /// <summary>
        /// Очистить корзину
        /// </summary>
        public virtual void ClearAll()
        {
            CartItems.Clear();
        }


        /// <summary>
        /// Количество объектов в корзине
        /// </summary>
        public int Count
        {
            get => CartItems.Sum(item => item.Value.Count);
        }


        /// <summary>
        /// Общее количество калорий
        /// </summary>
        public double TotalPrice
        {
            get => (double)CartItems.Sum(item => item.Value.Cocktail.Price * item.Value.Count);
        }
    }
}
