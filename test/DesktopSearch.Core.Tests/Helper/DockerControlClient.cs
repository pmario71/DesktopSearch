using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DesktopSearch.Core.Tests.Helper
{
    public class DockerControlClient
    {
        private const string connectionString = "localhost";
        private const string redisName = "redis-instance";
        private const string docker = "docker";

        public static Task<InstanceDescriptor> Start(string dockerImageName, string arguments)
        {
            InstanceDescriptor desc = null;
            var t = Task.Run(() =>
            {
                string dockerInstanceName = $"{dockerImageName}Node";

                string args = $"run --name {dockerInstanceName} {arguments} -d {dockerImageName}";
                var startcmd = Process.Start(docker, args);
                startcmd.WaitForExit();

                desc = new InstanceDescriptor(dockerInstanceName);
            });
            return t.ContinueWith(r => 
            {
                Task.Delay(2000);
                return desc;
            });
        }

        public static Task Stop(InstanceDescriptor instanceDescriptor)
        {
            return Task.Run(() =>
            {
                var stopcmd = Process.Start(docker, $"stop {instanceDescriptor.Name} ");
                stopcmd.WaitForExit();

                var msg = $"Failed to stop container instance '{instanceDescriptor.Name}' with ExitCode: {stopcmd.ExitCode}";
                CheckError(instanceDescriptor, stopcmd, msg);

                var rmcmd = Process.Start(docker, $"rm {instanceDescriptor.Name} ");
                rmcmd.WaitForExit();

                msg = $"Failed to remove container instance '{instanceDescriptor.Name}' with ExitCode: {stopcmd.ExitCode}";
                CheckError(instanceDescriptor, rmcmd, msg);
            });
        }

        private static void CheckError(InstanceDescriptor instanceDescriptor, Process stopcmd, string msg)
        {
            if (stopcmd.ExitCode != 0)
            {
                Console.WriteLine(msg);
                throw new Exception(msg);
            }
        }
    }

    public class InstanceDescriptor
    {
        private string dockerInstanceName;

        public InstanceDescriptor(string dockerInstanceName)
        {
            this.dockerInstanceName = dockerInstanceName;
        }

        public string Name { get; private set; }
    }
}
