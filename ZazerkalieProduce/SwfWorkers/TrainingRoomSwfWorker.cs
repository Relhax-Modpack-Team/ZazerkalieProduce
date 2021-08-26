using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ZazerkalieProduce.SwfWorkers
{
    class TrainingRoomSwfWorker : BaseSwfWorker
    {
        public TrainingRoomSwfWorker(Options options)
            : base(options)
        {
        }
        
        public override string SwfName { get; } = "trainingRoom.swf";

        public override void DoUnmirror(ref XDocument xdoc)
        {
            var elements =
                xdoc.Descendants()
                    .Where(x => x.Attribute("name") != null && x.Attribute("name").Value == "iconLoader");
            foreach (var element in elements)
            {
                LogSprite(element, "Unmirroring sprite Id");
                var matrix = element.Descendants().FirstOrDefault(x => x.Name.LocalName == "matrix");
                var scaleX = matrix.Attribute("scaleX");
                scaleX.SetValue(scaleX.Value.Replace("-", ""));
                var TranslateX = matrix.Attribute("translateX");
                int trX = int.Parse(TranslateX.Value);
                if (options.Mode == Options.OutputMode.showAll)
                {
                    TranslateX.SetValue((trX - 1600).ToString());
                }
                else
                {
                    TranslateX.SetValue((trX - 500).ToString());
                }
            }

            if (options.Mode == Options.OutputMode.clearAll || options.Mode == Options.OutputMode.clearTier)
            {
                if (options.Mode == Options.OutputMode.clearTier)
                {
                    elements =
                    xdoc.Descendants()
                        .Where(x => x.Attribute("name") != null && (x.Attribute("name").Value == "vehicleLevelField"));
                }
                else
                {
                    elements =
                    xdoc.Descendants()
                        .Where(x => x.Attribute("name") != null && (x.Attribute("name").Value == "vehicleLevelField"
                            || x.Attribute("name").Value == "vehicleField"));
                }

                foreach (var element in elements)
                {
                    LogSprite(element, "Untexting sprite Id");
                    element.SetAttributeValue("placeFlagHasVisible", "true");
                    element.SetAttributeValue("visible", "0");
                }
            }
        }
    }
}
