using Microsoft.AspNetCore.Mvc;
using SklepWelniany.Models;
using SklepWelniany.Models.DTOs;
using System.Diagnostics;

namespace SklepWelniany.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homeRepository;

        public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository)
        {
            _logger = logger;
            _homeRepository = homeRepository; // Iniekcja homeRepository w kontrolerze
        }

        public async Task<IActionResult> Index(string sterm="",int typeId=0)
        {
            IEnumerable<Product> products = await _homeRepository.GetProducts(sterm,typeId);
            IEnumerable<Models.Type> types = await _homeRepository.Types();
            ProductDisplayModel productModel = new ProductDisplayModel
            {
                Products = products,
                Types = types
            };


            return View(productModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
