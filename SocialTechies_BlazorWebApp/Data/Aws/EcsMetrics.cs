using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialTechies_BlazorWebApp.Data.Aws {
    public class EcsMetrics {
        public class CpuUtilization {
            public class Datapoint {
                public DateTime TimeStamp;
                public float Maximum;
                public string Unit;
            }
            public List<Datapoint> Datapoints;
        }
    }
}
