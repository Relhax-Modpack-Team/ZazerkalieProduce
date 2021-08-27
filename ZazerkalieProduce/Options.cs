using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace ZazerkalieProduce
{
    class Options
    {
        public enum OutputMode
        {
            showAll,
            clearTier,
            clearAll
        }

        [Option('j', "java", HelpText = "Full path to the java.exe. Example: \"C:\\Program Files (x86)\\Java\\jre1.8.0_25\\bin\\java.exe\"")]
        public string JavaPath { get; set; }

        [Option('f', "ffdec", HelpText = "Full path to the ffdec.jar. Example: \"C:\\Program Files (x86)\\FFDec\\ffdec.jar\"")]
        public string FfdecPath { get; set; }

        [Option('i', "input", Required = true, HelpText = "Full path to the directory with .swf. Example: \"D:\\Games\\World_of_Tanks_clean\\Wot Tank Icon Maker\\Icons\\Zazerkalie_lab\\swfs\"")]
        public string InputPath { get; set; }

        [Option('o', "output", Required = true, HelpText = "Full path to the output directory where .swf should be placed. Example: \"D:\\Games\\World_of_Tanks_clean\\Wot Tank Icon Maker\\Icons\\Zazerkalie_by_BufferOverflow\\gui\\flash\"")]
        public string OutputPath { get; set; }

        [Option('s', "swf", Required = true, HelpText = "Name of flash file. Ex: battleLoading.swf")]
        public string SwfName { get; set; }

        [Option('m', "mode", Required = true, HelpText = "Switch between showAll, clearTier and clearAll mode")]
        public string ModeStr { get; set; }

        public OutputMode Mode
        {
            get
            {
                OutputMode mode;
                if (Enum.TryParse(this.ModeStr, true, out mode))
                {
                    return mode;
                }
                throw new ArgumentException("-mode was specified, but the value was invalid");
            }
        }

        [Option('l', "logffdec", DefaultValue = null, HelpText = "Enable/disable logging of conversion to xml or swf")]
        public bool? SystemConsoleLog { get; set; }

        [Option('v', "verbose", DefaultValue = true, HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
