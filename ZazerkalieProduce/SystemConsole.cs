using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZazerkalieProduce
{
    static class SystemConsole
    {
        public static int ExecuteCommand(string exePath, string args, bool writeToConsole = true)
        {
            var processInfo = new ProcessStartInfo(exePath, args);
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;

            var process = Process.Start(processInfo);

            if (writeToConsole)
            {
                process.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
                    Console.WriteLine("output>>" + e.Data);
                process.BeginOutputReadLine();

                process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
                    Console.WriteLine("error>>" + e.Data);
                process.BeginErrorReadLine();
            }
            process.WaitForExit();

            int exitCode = process.ExitCode;
            if (writeToConsole)
            {
                Console.WriteLine("ExitCode: {0}", exitCode);
            }
            process.Close();
            return exitCode;
        }
    }
}
