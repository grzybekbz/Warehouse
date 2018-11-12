using Microsoft.AspNetCore.Mvc;
using Warehouse.Models;

namespace Warehouse.Controllers {

    public class ProductController : Controller {

        private readonly IRepository repository;

        public ProductController(IRepository repo) {

            repository = repo;
        }

        public ViewResult Index() => View(repository.Products);

        [HttpPost]
        public IActionResult AddProduct() {

            Product p = new Product() {Name = "czipsy", Category = "jedzenie",
                    Description = "pyszne pieczone plasty ziemniaka",
                    Price = 1.5F, Weight = 0.003F };

            repository.SaveProduct(p);

            return RedirectToAction("Index");
        }
    }
}