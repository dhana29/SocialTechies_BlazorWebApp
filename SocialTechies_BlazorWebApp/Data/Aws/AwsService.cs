using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialTechies_BlazorWebApp.Data.Aws
{
    public class AwsService
    {
        public Task<EcsMetrics.CpuUtilization> GetCpuUtilizationForInstance(string instanceId, int period, DateTime startTime, DateTime endTime)
        {
            string startTimeString = startTime.ToString("s");
            string endTimeString = endTime.ToString("s");
            return Task.FromResult(
                JsonConvert.DeserializeObject<EcsMetrics.CpuUtilization>(RunAwsProcessAsync($"cloudwatch get-metric-statistics --namespace AWS/EC2 --metric-name CPUUtilization --period {period} --statistics Maximum --dimensions Name=InstanceId,Value={instanceId} --start-time {startTimeString} --end-time {endTimeString}").Result)
            );
        }

        public Task<EcsMetrics.CpuUtilization> GetCpuUtilizationForAutoscalingGroup(string autoScalingGroup, int period, DateTime startTime, DateTime endTime)
        {
            string startTimeString = startTime.ToString("s");
            string endTimeString = endTime.ToString("s");
            return Task.FromResult(
                JsonConvert.DeserializeObject<EcsMetrics.CpuUtilization>(RunAwsProcessAsync($"cloudwatch get-metric-statistics --namespace AWS/EC2 --metric-name CPUUtilization --period {period} --statistics Maximum --dimensions Name=AutoScalingGroupName,Value={autoScalingGroup} --start-time {startTimeString} --end-time {endTimeString}").Result)
            );
        }

        public Task<Deploy.DeploymentGroups> GetDeploymentGroupsFromApplicationName(string applicationName) {
            return Task.FromResult(
                JsonConvert.DeserializeObject<Deploy.DeploymentGroups>(RunAwsProcessAsync($"deploy list-deployment-groups --application-name {applicationName}").Result)
            );
        }

        public Task<List<AwsInstance>> GetInstanceInfo(string productName) {
            string response = RunAwsProcessAsync($"ec2 describe-instances --filters Name=instance-state-name,Values=running Name=tag:Product,Values={productName} --query \"Reservations[*].Instances[*].{{ InstanceId: InstanceId,PublicIP: PublicIpAddress,Name: Tags[?Key == 'Name'] |[0].Value,Status: State.Name}}").Result;
            return Task.FromResult(
                    JsonConvert.DeserializeObject<List<List<AwsInstance>>>(response).Select(i => i[0]).ToList()
                //JsonConvert.DeserializeObject<List<List<AwsInstance>>>(RunAwsProcessAsync($"ec2 describe-instances --filters Name=instance-state-name,Values=running Name=tag:Product,Values={productName} --query \"Reservations[*].Instances[*].{{ InstanceId: InstanceId,PublicIP: PublicIpAddress,Name: Tags[?Key == 'Name'] |[0].Value,Status: State.Name}}").Result)[0]
            );
        }


        private Task<string> RunAwsProcessAsync(string parameters)
        {
            var tcs = new TaskCompletionSource<string>();
            System.Diagnostics.Process process = new System.Diagnostics.Process()
            {
                StartInfo = {
                    FileName = "cmd.exe",
                    Arguments = $"/C aws {parameters}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                },
                EnableRaisingEvents = true
            };

            string data = String.Empty;
            process.OutputDataReceived += (sender, args) => {
                data += args.Data;
            };

            process.Start();
            process.BeginOutputReadLine();
            process.WaitForExit();

            tcs.SetResult(data);
            return tcs.Task;
        }
    }


    public class AwsInstance {
        public string PublicIP { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string InstanceId { get; set; }
    }
}
