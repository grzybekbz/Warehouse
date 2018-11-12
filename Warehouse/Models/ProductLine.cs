using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse.Models {

    public class ProductLine {

        public int ProductLineID { get; set; }

        [ForeignKey("Activity")]
        public int ActivityID { get; set; }

        [ForeignKey("Product")]
        public int ProductID { get; set; }

        public int Quantity { get; set; }
    }
}
