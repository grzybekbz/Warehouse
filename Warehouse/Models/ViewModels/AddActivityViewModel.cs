using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Warehouse.Models {

    public class AddActivityViewModel {

        public Activity ActivityToAdd { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Nie wybrano produktu")]
        public int ProductID { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Podaj ilość produktów")]
        public int ProductQuantity { get; set; }

        public SelectList ListOfProducts { get; set; }

        [EnsureOneElement(ErrorMessage = "Nie dodano produktów")]
        public List<ProductValues> ActivityProducts { get; set; }
    }

    public class ProductValues {

        public int ID { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
    }
}