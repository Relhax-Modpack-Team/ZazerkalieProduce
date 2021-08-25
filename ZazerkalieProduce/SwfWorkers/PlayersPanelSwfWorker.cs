﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ZazerkalieProduce.SwfWorkers
{
    class PlayersPanelSwfWorker : BaseSwfWorker
    {
        public PlayersPanelSwfWorker(Options options)
            : base(options)
        {
        }

        public override string SwfName { get; set; } = "playersPanel.swf";

        public override void DoUnmirror(ref XDocument xdoc)
        {
            var elements =
                xdoc.Descendants()
                    .Where(x => x.Attribute("name") != null && x.Attribute("name").Value == "vehicleIcon")
                    .Take(1);
            foreach (var element in elements)
            {
                LogSprite(element, "Unmirroring sprite Id");
                var matrix = element.Descendants().FirstOrDefault(x => x.Name.LocalName == "matrix");
                var scaleX = matrix.Attribute("scaleX");
                scaleX.SetValue(scaleX.Value.Replace("-", ""));
                var TranslateX = matrix.Attribute("translateX");
                int trX = int.Parse(TranslateX.Value);
                TranslateX.SetValue((trX - 1600).ToString());
            }
            if (options.Mode == Options.OutputMode.notext)
            {
                elements =
                xdoc.Descendants()
                    .Where(x => x.Attribute("name") != null && x.Attribute("name").Value == "vehicleLevel");
                foreach (var element in elements)
                {
                    LogSprite(element, "Untexting sprite Id");
                    var matrix = element.Descendants().FirstOrDefault(x => x.Name.LocalName == "matrix");
                    var translateX = matrix.Attribute("translateX");
                    translateX.SetValue("999999");
                }
            }
        }
    }
}