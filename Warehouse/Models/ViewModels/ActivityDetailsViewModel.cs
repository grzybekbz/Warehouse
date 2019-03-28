using System.Collections.Generic;

namespace Warehouse.Models {

    public class ActivityDetailsViewModel {

        public Activity Activity { get; set; }

        public IEnumerable<ProductLine> Products { get; set; }
    }
}
