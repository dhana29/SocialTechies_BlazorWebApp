using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialTechies_BlazorWebApp.Data.Aws {
    public class AwsService {
        public Task<EcsMetrics.CpuUtilization> GetCpuUtilization(string instanceId, int period, DateTime startTime, DateTime endTime) {
            string startTimeString = startTime.ToString("s");
            string endTimeString = endTime.ToString("s");
            return Task.FromResult(
                JsonConvert.DeserializeObject<EcsMetrics.CpuUtilization>(RunAwsProcessAsync($"cloudwatch get-metric-statistics --namespace AWS/EC2 --metric-name CPUUtilization --period {period} --statistics Maximum --dimensions Name=InstanceId,Value=${instanceId} --start-time {startTimeString} --end-time {endTimeString}").Result)
            );
        }


        private Task<string> RunAwsProcessAsync(string parameters) {
            var tcs = new TaskCompletionSource<string>();
            System.Diagnostics.Process process = new System.Diagnostics.Process() {
                StartInfo = {
                    FileName = "cmd.exe",
                    Arguments = $"/C aws {parameters}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                    //CreateNoWindow = true,
                    //WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
                },
                EnableRaisingEvents = true
            };
            process.Exited += (sender, args) => {
                //string output = process.StandardOutput.ReadToEnd();
                var output = process.StandardOutput.ReadToEndAsync();
                tcs.SetResult(output.Result);
                process.Dispose();
            };

            process.Start();
            process.BeginOutputReadLine();
            process.WaitForExit();

            return tcs.Task;
        }
    }
}
