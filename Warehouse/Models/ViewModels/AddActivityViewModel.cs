using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Warehouse.Models {

    public class AddActivityViewModel {

        public Activity ActivityToAdd { get; set; }

        public int ProductID { get; set; }
        public int ProductQuantity { get; set; }

        public SelectList ListOfProducts { get; set; }

        public List<ProductValues> ActivityProducts { get; set; }
    }

    public class ProductValues {

        public int ID { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
    }
}