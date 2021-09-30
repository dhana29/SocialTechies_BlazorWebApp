using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialTechies_BlazorWebApp.Data.Aws {
    public class EcsMetrics {
        public class CpuUtilization {
            public class Datapoint {
                public DateTime TimeStamp { get; set; }
                public float Maximum { get; set; }
                public string Unit { get; set; }
            }
            public List<Datapoint> Datapoints;
        }
    }

    public class Deploy {
        public class DeploymentGroups {
            public List<string> deploymentGroups { get; set; }
        }
    }


    public class ApplicationContext {
        public string name { get; set; }
        public string chartName { get; set; }
        public string deploymentGroup { get; set; }
        public EcsMetrics.CpuUtilization cpuUtilization { get; set; }
        public EcsMetrics.CpuUtilization.Datapoint mostRecentDatapoint { get; set; }
    }
}
