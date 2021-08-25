using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ZazerkalieProduce.SwfWorkers
{
    abstract class BaseSwfWorker
    {
        public readonly string XmlName = null;
        public readonly string XmlResultName = null;
        protected Options options;
        protected readonly string TempPath = "Temp";
        private static string lastId = null;

        public abstract string SwfName { get; set; }

        public BaseSwfWorker(Options options)
        {
            this.options = options;
            this.XmlName = this.SwfName.Replace(".swf", ".xml");
            this.XmlResultName = this.SwfName.Replace(".swf", "Result.xml");
            if (!Directory.Exists(TempPath))
            {
                Directory.CreateDirectory(TempPath);
            }
        }

        public virtual XDocument ProduceXmlFromSwf()
        {
            var swfpath = Path.Combine(this.options.InputPath, this.SwfName);
            var xmlpath = Path.Combine(this.TempPath, XmlName);
            var args = string.Format("-jar \"{0}\" -swf2xml \"{1}\" \"{2}\"", this.options.FfdecPath, swfpath, xmlpath);
            if (this.options.Verbose)
            {
                Console.WriteLine("java " + args);
            }
            var resultcode = SystemConsole.ExecuteCommand(this.options.JavaPath, args, this.options.SystemConsoleLog ?? false);
            if (resultcode != 0)
            {
                if (this.options.Verbose)
                {
                    Console.WriteLine("Execute exited with code=" + resultcode);
                }
                throw new ApplicationException("Critical error. Process stopped.");
            }
            return XDocument.Load(xmlpath);
        }

        public abstract void DoUnmirror(ref XDocument xdoc);

        public virtual void SaveXmlResult(XDocument xdoc)
        {
            var xmlpath = Path.Combine(this.TempPath, XmlResultName);
            using (Stream stream = File.Create(xmlpath))
            {
                xdoc.Save(stream);
            }
        }

        public virtual void ProduceOutputFromXml()
        {
            var xmlpath = Path.Combine(this.TempPath, XmlResultName);
            var outpath = Path.Combine(this.options.OutputPath, SwfName);
            if (!Directory.Exists(this.options.OutputPath))
            {
                Directory.CreateDirectory(this.options.OutputPath);
            }
            else if (File.Exists(outpath))
            {
                File.Delete(outpath);
            }


            var args = string.Format("-jar \"{0}\" -xml2swf \"{1}\" \"{2}\"", this.options.FfdecPath, xmlpath, outpath);
            if (this.options.Verbose)
            {
                Console.WriteLine("java " + args);
            }

            var resultcode = SystemConsole.ExecuteCommand(this.options.JavaPath, args, this.options.SystemConsoleLog ?? false);
            if (resultcode != 0)
            {
                if (this.options.Verbose)
                {
                    Console.WriteLine("Execute exited with code=" + resultcode);
                }
                throw new ApplicationException("Fatal error during processing. Application stopped.");
            }
        }

        protected void LogSprite(XElement element, string text)
        {
            var SpriteId = element.Parent.Parent.Attribute("spriteId");
            if (SpriteId != null)
            {
                if (this.options.Verbose && lastId != SpriteId.Value)
                {
                    lastId = SpriteId.Value;
                    Console.WriteLine(text + "=" + SpriteId.Value);
                }
                Console.WriteLine("Element name=" + element.Attribute("name").Value);
            }
            else
            {
                if (this.options.Verbose)
                {
                    Console.WriteLine(text + " not found");
                    throw new ApplicationException("Critical error. Process stopped.");
                }
            }
        }

        public virtual void DoWholeJob()
        {
            var xml = this.ProduceXmlFromSwf();
            this.DoUnmirror(ref xml);
            this.SaveXmlResult(xml);
            this.ProduceOutputFromXml();
        }
    }
}
