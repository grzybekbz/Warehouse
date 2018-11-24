using System.ComponentModel.DataAnnotations;
using System;

namespace Warehouse.Models {

    public class Activity {

        public int ActivityID { get; set; }

        [Required]
        [RegularExpression("(^Shipment$)|(^Admission$)", 
            ErrorMessage = "Nieprawidłowy typ danych")]
        public string Type { get; set; }

        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Podaj miejsce")]
        public string Where { get; set; }

        [Required(ErrorMessage = "Podaj opis")]
        public string Description { get; set; }
    }
}
