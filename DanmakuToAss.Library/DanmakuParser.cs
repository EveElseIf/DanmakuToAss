using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DanmakuToAss.Library
{
    public class DanmakuParser
    {
        public static IEnumerable<Danmaku> LoadXml(string path)
        {
            if (!File.Exists(path)) throw new FileNotFoundException();
            var list = new List<Danmaku>();
            foreach (var item in XDocument.Load(path).Element("i").Elements("d"))
            {
                var attributes = item.Attribute("p").Value;
                var attributeList = attributes.Split(',');
                list.Add(new Danmaku()
                {
                    ShowTime = float.Parse(attributeList[0]),
                    Type = (DanmakuType)int.Parse(attributeList[1]),
                    Size = (DanmakuSize)int.Parse(attributeList[2]),
                    Colour = int.Parse(attributeList[3]),
                    SendTime = int.Parse(attributeList[4]),
                    PoolType = (DanmakuPoolType)int.Parse(attributeList[5]),
                    SenderUIDHash = attributeList[6],
                    Id = long.Parse(attributeList[7]),
                    Content = item.Value
                });
            }
            var i = 0;
            list = list.OrderBy(d => d.ShowTime).ToList();
            list.ForEach(d => { d.Index = i; i++; });
            return list;
        }
    }
}
