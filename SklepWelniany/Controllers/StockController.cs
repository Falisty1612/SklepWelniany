using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SklepWelniany.Controllers
{
    [Authorize(Roles=nameof(Roles.Admin))]
    public class StockController : Controller
    {
        private readonly IStockRepository _stockRepo;

        public StockController(IStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
        }

        public async Task<IActionResult> Index(string sTerm = "")
        {
            var stocks = await _stockRepo.GetStocks(sTerm);
            return View(stocks);
        }

        public async Task<IActionResult> ManageStock(int productId)
        {
            var existingStock = await _stockRepo.GetStockByProductId(productId);
            var stock = new StockDTO
            {
                ProductId = productId,
                Quantity = existingStock != null ? existingStock.Quantity : 0
            };
            return View(stock);
        }

        [HttpPost]
        public async Task<IActionResult> ManageStock(StockDTO stock)
        {
            if (!ModelState.IsValid)
                return View(stock);
            try
            {
                await _stockRepo.ManageStock(stock);
                TempData["successMessage"] = "Magazyn zaktualizowany pomyślnie.";
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = $"Wystąpił błąd podczas aktualizacji magazynu: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
