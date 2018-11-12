using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Collections.Generic;
using Warehouse.Models;

namespace Warehouse.Controllers {

    public class ActivityController : Controller {

        private readonly IRepository repository;

        public ActivityController(IRepository repo) {

            repository = repo;
        }

        public ViewResult Index() => View();

        public ViewResult Admissions() {

            ViewBag.ActivityType = "Admission";

            return View("ShowActivities", repository.Activities
                .Where(p => p.Type.Equals("Admission")));
        }

        public ViewResult Shipments() {

            ViewBag.ActivityType = "Shipment";

            return View("ShowActivities", repository.Activities
                .Where(p => p.Type.Equals("Shipment")));
        }

        public ViewResult AddActivity(string activityType) {

            if (activityType != null &&
               (activityType.Equals("Admission") ||
                activityType.Equals("Shipment")))

                return View(new AddActivityViewModel() {
                    ListOfProducts = new SelectList(
                        repository.Products, "ProductID", "Name"),
                    ActivityToAdd = new Activity() { Type = activityType },
                    ActivityProducts = new List<ProductValues>()
                });
            else
                return View("Index");
        }

        [HttpPost]
        public IActionResult AddActivity(AddActivityViewModel activity) {

            activity.ActivityToAdd.Date = DateTime.Now;
            activity.ListOfProducts = new SelectList(
                repository.Products, "ProductID", "Name");

            if (activity.ActivityProducts == null) {

                ModelState.AddModelError("noproducts", "Nie dodano produktów");
                return View(activity);

            } else if (activity.ActivityToAdd.Description == null) {

                ModelState.AddModelError("ActivityToAdd.Description", "Podaj opis");
                return View(activity);

            } else if (activity.ActivityToAdd.Where == null) {

                ModelState.AddModelError("ActivityToAdd.Where", "Podaj miejsce");
                return View(activity);

            } else {

                repository.SaveActivity(activity.ActivityToAdd);
                repository.SaveProductsInActivity(activity.ActivityProducts,
                                                  activity.ActivityToAdd);

                if (activity.ActivityToAdd.Type.Equals("Admission")) {

                    TempData["message"] = $"Dodano przyjęcie na magazyn nr " +
                            $"{activity.ActivityToAdd.ActivityID}";
                    return RedirectToAction("Admissions");

                } else if (activity.ActivityToAdd.Type.Equals("Shipment")) {

                    TempData["message"] = $"Dodano wydanie z magazynu nr " +
                            $"{activity.ActivityToAdd.ActivityID}";
                    return RedirectToAction("Shipments");
                } else {

                    return View("Index");
                }
            }
        }

        [HttpPost]
        public ViewResult AddProductToActivity(AddActivityViewModel activity) {

            activity.ListOfProducts = new SelectList(
                        repository.Products, "ProductID", "Name");

            if (activity.ActivityProducts == null)
                activity.ActivityProducts = new List<ProductValues>();

            if (activity.ProductID == 0) {

                ModelState.AddModelError("noproduct", "Nie wybrano produktu");
                return View("AddActivity", activity);

            } else if (activity.ProductQuantity == 0) {

                ModelState.AddModelError("ProductQuantity", "Podaj ilość produktów");
                return View("AddActivity", activity);

            } else {

                Product prod = repository.Products
                            .Where(p => p.ProductID.Equals(activity.ProductID))
                            .FirstOrDefault();

                activity.ActivityProducts.Add(new ProductValues() {
                        ID = prod.ProductID,
                        Name = prod.Name,
                        Quantity = activity.ProductQuantity
                });

                return View("AddActivity", activity);
            }
        }

        [HttpPost]
        public ViewResult DeleteProductFromActivity(AddActivityViewModel activity,
                                                    int activityProductId) {

            activity.ListOfProducts = new SelectList(
                        repository.Products, "ProductID", "Name");

            activity.ActivityProducts.RemoveAt(activityProductId);

            return View("AddActivity", activity);
        }
    }
}