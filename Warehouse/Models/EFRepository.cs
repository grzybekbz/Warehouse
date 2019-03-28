using System.Collections.Generic;
using System.Linq;

namespace Warehouse.Models {

    public class EFRepository : IRepository {

        public ApplicationDbContext context;

        public IQueryable<Product> Products => context.Products;
        public IQueryable<Activity> Activities => context.Activities;
        public IQueryable<ProductLine> ProductLines => context.ProductLines;

        public EFRepository(ApplicationDbContext ctx) {

            context = ctx;
        }

        public void SaveProduct(Product product) {

            if (product.ProductID == 0) {

                context.Products.Add(product);

            } else {

                var dbEntry = context.Products
                    .FirstOrDefault(p => p.ProductID == product.ProductID);
                if (dbEntry != null) {

                    dbEntry.ProductID = product.ProductID;
                }
            }
            context.SaveChanges();
        }

        public void SaveActivity(Activity activity) {

            if (activity.ActivityID == 0) {

                context.Activities.Add(activity);

            } else {

                var dbEntry = context.Activities
                    .FirstOrDefault(p => p.ActivityID == activity.ActivityID);
                if (dbEntry != null) {

                    dbEntry.ActivityID = activity.ActivityID;
                }
            }

            context.SaveChanges();
        }

        public void SaveProductsInActivity(List<ProductValues> products,
                                           Activity activity) {

            foreach (var product in products) {

                ProductLine line = new ProductLine() {
                    ActivityID = activity.ActivityID,
                    ProductID = product.ID,
                    Quantity = product.Quantity
                };

                context.ProductLines.Add(line);
            }

            context.SaveChanges();
        }
    }
}
