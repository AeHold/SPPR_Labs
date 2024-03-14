using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace WEB_153503_SHMIDT.Controllers
{

    public class ListDemo
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["CurrentLab"] = "Лабораторная работа №3";

            List<ListDemo> list = new();
            for(int i = 0; i < 5; i++)
            {
                list.Add(new ListDemo() { Id = i, Name = $"Item {i}" });
            }

            SelectList selectList = new SelectList(list, "Id", "Name");

            return View(selectList);
        }
    }
}
