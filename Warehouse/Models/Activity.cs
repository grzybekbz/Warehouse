using System;

namespace Warehouse.Models {

    public class Activity {

        public int ActivityID { get; set; }

        public string Type { get; set; }

        public DateTime Date { get; set; }

        public string Where { get; set; }

        public string Description { get; set; }
    }
}
