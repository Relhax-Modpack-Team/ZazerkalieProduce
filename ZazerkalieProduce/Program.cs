using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using ZazerkalieProduce.SwfWorkers;

namespace ZazerkalieProduce
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                if (string.IsNullOrEmpty(options.FfdecPath))
                {
                    options.FfdecPath = ConfigurationManager.AppSettings["FfdecPath"];
                }
                if (string.IsNullOrEmpty(options.JavaPath))
                {
                    options.JavaPath = ConfigurationManager.AppSettings["JavaPath"];
                }
                if (options.SystemConsoleLog == null)
                {
                    options.SystemConsoleLog = ConfigurationManager.AppSettings["SystemConsoleLog"] == "true";
                }

                var swfWorkerTypes = (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                                from assemblyType in domainAssembly.GetTypes()
                                where typeof(BaseSwfWorker).IsAssignableFrom(assemblyType) && !assemblyType.IsAbstract
                                select assemblyType).ToArray();
                var swfWorker =
                    swfWorkerTypes.Select(x => Activator.CreateInstance(x, options))
                        .Cast<BaseSwfWorker>()
                        .FirstOrDefault(x => x.SwfName == options.SwfName);
                if (swfWorker != null)
                {
                    swfWorker.DoWholeJob();
                }
            }
#if DEBUG
            Console.ReadKey();
#endif
        }
    }
}
