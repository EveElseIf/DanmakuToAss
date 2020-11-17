using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DanmakuToAss.Library
{
    public class DanmakuParser
    {
        /// <summary>
        /// 加载xml文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>弹幕列表</returns>
        public static IList<Danmaku> LoadXmlFromFile(string path)
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
        /// <summary>
        /// 通过xml字符串加载xml
        /// </summary>
        /// <param name="xmlContent">xml字符串</param>
        /// <returns>弹幕列表</returns>
        public static IList<Danmaku> LoadXmlFromString(string xmlContent)
        {
            var list = new List<Danmaku>();
            foreach (var item in XDocument.Load(xmlContent).Element("i").Elements("d"))
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
