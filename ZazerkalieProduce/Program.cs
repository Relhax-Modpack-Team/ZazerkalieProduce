using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
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
            AttachAssemblyResolver();
            ActuallyRunProgram(args);
        }

        public static void ActuallyRunProgram(string[] args)
        {
            var options = new Options();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                //if options not specified in command line, then get them from the application settings here
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

                //get a list of all swf worker types that exist in the program
                var swfWorkerTypes = (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                                      from assemblyType in domainAssembly.GetTypes()
                                      where typeof(BaseSwfWorker).IsAssignableFrom(assemblyType) && !assemblyType.IsAbstract
                                      select assemblyType).ToArray();

                //select the swf worker to use based on the command line options
                var swfWorker =
                    swfWorkerTypes.Select(x => Activator.CreateInstance(x, options))
                        .Cast<BaseSwfWorker>()
                        .FirstOrDefault(x => x.SwfName.Equals(options.SwfName));

                if (swfWorker != null)
                {
                    swfWorker.DoWholeJob();
                }
            }
        }

        public static void AttachAssemblyResolver()
        {
            //handle any assembly resolves
            //https://stackoverflow.com/a/19806004/3128017
            AppDomain.CurrentDomain.AssemblyResolve += (sender2, bargs) =>
            {
                string dllName = new AssemblyName(bargs.Name).Name + ".dll";
                Assembly assem = Assembly.GetExecutingAssembly();
                string resourceName = assem.GetManifestResourceNames().FirstOrDefault(rn => rn.EndsWith(dllName));
                using (Stream stream = assem.GetManifestResourceStream(resourceName))
                {
                    byte[] assemblyData = new byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    Console.WriteLine("An assembly was loaded via AssemblyResolve: {0}", dllName);
                    return Assembly.Load(assemblyData);
                }
            };
        }
    }
}
