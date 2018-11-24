using System.Collections.Generic;

namespace Warehouse.Models {

    public class ShowAcivitiesViewModel {

        public string ActivityType { get; set; }

        public IEnumerable<Activity> Activities { get; set; }
    }
}
