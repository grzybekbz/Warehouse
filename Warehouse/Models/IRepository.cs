using System.Collections.Generic;
using System.Linq;

namespace Warehouse.Models {

    public interface IRepository {

        IQueryable<Product> Products { get; }
        IQueryable<Activity> Activities { get; }
        IQueryable<ProductLine> ProductLines { get; }

        void SaveProduct(Product product);
        void SaveActivity(Activity activity);
        void SaveProductsInActivity(List<ProductValues> products,
                                           Activity activity);
        }
}
