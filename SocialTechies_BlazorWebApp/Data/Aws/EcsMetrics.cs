using SocialTechies_BlazorWebApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SocialTechies_BlazorWebApp.Data.Aws.AwsService;

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
        public string chartName { get; set; }
        public AwsInstance instance { get; set; }
        public EcsMetrics.CpuUtilization cpuUtilization { get; set; }
        public EcsMetrics.CpuUtilization.Datapoint mostRecentDatapoint { get; set; }
        public CpuGraph cpuGraphComponent { get; set; }
    }
}
