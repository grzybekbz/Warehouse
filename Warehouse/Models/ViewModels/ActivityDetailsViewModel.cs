using System.Collections.Generic;

namespace Warehouse.Models {

    public class ActivityDetailsViewModel {

        public Activity Activity { get; set; }

        public IEnumerable<ProductAndQuantity> Products { get; set; }
    }

    public struct ProductAndQuantity {

        public string Product { get; set; }
        public int Quantity { get; set; }
    }
}
