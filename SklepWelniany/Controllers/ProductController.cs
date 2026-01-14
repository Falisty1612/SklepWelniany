using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SklepWelniany.Shared;

namespace SklepWelniany.Controllers
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class ProductController : Controller
    {

        private readonly IProductRepository _productRepo;
        private readonly ITypeRepository _typeRepo;
        private readonly IFileService _fileService;

        public ProductController(IProductRepository productRepo, IFileService fileService, ITypeRepository typeRepo)
        {
            _productRepo = productRepo;
            _typeRepo = typeRepo;
            _fileService = fileService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productRepo.GetProducts();
            return View(products);
        }

        public async Task<IActionResult> AddProduct()
        {
            var typeSelectList = (await _typeRepo.GetTypes())
                .Select(type => new SelectListItem
                {
                    Text = type.ProductType,
                    Value = type.Id.ToString()
                });
            ProductDTO productToAdd = new() { TypeList = typeSelectList };
            return View(productToAdd);
        }


        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductDTO productToAdd)
        {
            var typeSelectList = (await _typeRepo.GetTypes())
                    .Select(type => new SelectListItem
                    {
                        Text = type.ProductType,
                        Value = type.Id.ToString()
                    });
            productToAdd.TypeList = typeSelectList;

            if(!ModelState.IsValid)
                return View(productToAdd);

            try
            {
                if(productToAdd.ImageFile != null)
                {
                    if (productToAdd.ImageFile.Length > 100 * 1024 * 1024)
                        throw new InvalidOperationException("Image file exceeds 100MB");
                

                    string[] allowedExtensions = [".jpeg", ".jpg", ".png", ".webm"];
                    string imageName = await _fileService.SaveFile(productToAdd.ImageFile,allowedExtensions);
                    productToAdd.Image = imageName;
                }

                Product product = new()
                {
                    Id = productToAdd.Id,
                    ProductName = productToAdd.ProductName,
                    Image = productToAdd.Image,
                    TypeId = productToAdd.TypeId,
                    Price = productToAdd.Price
                };
                await _productRepo.AddProduct(product);
                TempData["successMessage"] = "Produkt dodany pomyślnie";
                return RedirectToAction(nameof(AddProduct));
            }
            catch(InvalidOperationException ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View(productToAdd);
            }
            catch(FileNotFoundException ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View(productToAdd);
            }
            catch(Exception ex)
            {
                TempData["errorMessage"] = "Błąd w zapisie danych";
                return View(productToAdd);
            }
        }


        public async Task<IActionResult> UpdateProduct(int id)
        {
            var product = await _productRepo.GetProductById(id);
            if (product == null)
            {
                TempData["errorMessage"] = $"Produkt z ID: {id} nie został znaleziony.";
                return RedirectToAction(nameof(Index));
            }
            var typeSelectList = (await _typeRepo.GetTypes())
                    .Select(type => new SelectListItem
                    {
                        Text = type.ProductType,
                        Value = type.Id.ToString(),
                        Selected = type.Id==product.TypeId
                    });
            ProductDTO productToUpdate = new()
            {
                TypeList = typeSelectList,
                ProductName = product.ProductName,
                TypeId = product.TypeId,
                Price = product.Price,
                Image = product.Image
            };
            return View(productToUpdate);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateProduct(ProductDTO productToUpdate)
        {
            var typeSelectList = (await _typeRepo.GetTypes()).Select(type => new SelectListItem
            {
                Text = type.ProductType,
                Value = type.Id.ToString(),
                Selected = type.Id == productToUpdate.TypeId
            });
            productToUpdate.TypeList = typeSelectList;

            if (!ModelState.IsValid)
                return View(productToUpdate);

            try
            {
                string oldImage = "";
                if (productToUpdate.ImageFile != null)
                {
                    if (productToUpdate.ImageFile.Length > 100 * 1024 * 1024)
                        throw new InvalidOperationException("Image file exceeds 100MB");


                    string[] allowedExtensions = [".jpeg", ".jpg", ".png", ".webm"];
                    string imageName = await _fileService.SaveFile(productToUpdate.ImageFile, allowedExtensions);

                    oldImage = productToUpdate.Image;
                    productToUpdate.Image = imageName;
                }
                Product product = new()
                {
                    Id = productToUpdate.Id,
                    ProductName = productToUpdate.ProductName,
                    Image = productToUpdate.Image,
                    TypeId = productToUpdate.TypeId,
                    Price = productToUpdate.Price
                };

                await _productRepo.UpdateProduct(product);
                if (!string.IsNullOrWhiteSpace(oldImage))
                    _fileService.DeleteFile(oldImage);

                TempData["successMessage"] = "Produkt zaktualizowany pomyślnie.";
                return RedirectToAction(nameof(Index));
            }
            catch(InvalidOperationException ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View(productToUpdate);
            }
            catch (FileNotFoundException ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View(productToUpdate);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Błąd w zapisie danych";
                return View(productToUpdate);
            }
        }

        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _productRepo.GetProductById(id);
                if (product == null)
                    TempData["errorMessage"] = $"Produkt z ID: {id} nie został znaleziony.";

                else
                {
                    await _productRepo.DeleteProduct(product);
                    if (!string.IsNullOrWhiteSpace(product.Image))
                        _fileService.DeleteFile(product.Image);

                }
            }
            catch (InvalidOperationException ex)
            {
                TempData["errorMessage"] = ex.Message;
            }
            catch (FileNotFoundException ex)
            {
                TempData["errorMessage"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Error on deleting the data";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
