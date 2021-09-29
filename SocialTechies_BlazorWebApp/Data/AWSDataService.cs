using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SocialTechies_BlazorWebApp.Data
{
    public class AWSDataService
    {
        private static readonly string[] awsDatacPU = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public Task<AWSData[]> CPUUtilizationAsync(DateTime startDate)
        {
            try {
                var jsonText = File.ReadAllText("C:\\Users\\jbarragan\\Downloads\\asReportsSampleData.json");
                var res = JsonConvert.DeserializeObject<AWSDataMain>(jsonText);
                //Object data = JObject.Parse(File.ReadAllText("C:\\Users\\jbarragan\\Downloads\\asReportsSampleData.json"));

                //var res = data as AWSDataMain;

                if (res == null) 
                    return null;

                return Task.FromResult(
                   res.Datapoints
                    );
            }
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine($"Errors: {ex.Message} {ex.StackTrace}");
            }

            return null;

        }
    }
}
