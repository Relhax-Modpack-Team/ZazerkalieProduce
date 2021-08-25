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
            normal,
            notext
        }

        [Option('j', "java", HelpText = "Full path to the java.exe. Example: \"C:\\Program Files (x86)\\Java\\jre1.8.0_25\\bin\\java.exe\"")]
        public string JavaPath { get; set; }

        [Option('f', "ffdec", HelpText = "Full path to the ffdec.jar. Example: \"C:\\Program Files (x86)\\FFDec\\ffdec.jar\"")]
        public string FfdecPath { get; set; }

        [Option('i', "input", Required = true, HelpText = "Full path to the directory with .swf. Example: \"D:\\Games\\World_of_Tanks_clean\\Wot Tank Icon Maker\\Icons\\Zazerkalie_lab\\swfs\"")]
        public string InputPath { get; set; }

        [Option('o', "ouput", Required = true, HelpText = "Full path to the otput directory where .swf should be placed. Example: \"D:\\Games\\World_of_Tanks_clean\\Wot Tank Icon Maker\\Icons\\Zazerkalie_by_BufferOverflow\\gui\\flash\"")]
        public string OutputPath { get; set; }

        [Option('s', "swf", Required = true, HelpText = "Name of flash file. Ex: battleLoading.swf")]
        public string SwfName { get; set; }

        [Option('m', "mode", DefaultValue = "normal", HelpText = "Switch between normal and notext mode")]
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
                return OutputMode.normal;
            }
        }

        [Option('l', "logffdec", DefaultValue = null, HelpText = "Enable/disable logging of convertation to xml or swf")]
        public bool? SystemConsoleLog { get; set; }

        [Option('v', "verbose", DefaultValue = true,
          HelpText = "Prints all messages to standard output.")]
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
