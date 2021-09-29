using System;

namespace SocialTechies_BlazorWebApp.Data
{
    public class AWSData
    {
        public DateTime Timestamp { get; set; }

        public double Maximum { get; set; }

        // Percent
        public string Unit { get; set; }
    }

    public class AWSDataMain
    {
        public string Label { get; set; }

        // Percent
        public AWSData[] Datapoints { get; set; }
    }

    public class AWSDataMain2
    {
        // Percent
        public AWSDataMain data { get; set; }
    }
}
