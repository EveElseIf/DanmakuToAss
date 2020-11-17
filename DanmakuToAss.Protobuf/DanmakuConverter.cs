using DanmakuToAss.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DanmakuToAss.Protobuf
{
    public static class DanmakuConverter
    {
        /// <summary>
        /// 将弹幕列表转换为ass字符串
        /// </summary>
        /// <param name="danmakus">弹幕列表，使用DanmakuParser类获取</param>
        /// <param name="videoWidth">视频宽度</param>
        /// <param name="videoHeight">视频高度</param>
        /// <param name="fontName">字体名称，可以保持默认</param>
        /// <param name="fontSize">字体大小，可以保持默认</param>
        /// <param name="lineCount">同屏行数，可以保持默认</param>
        /// <param name="bottomMargin">底编剧，可以保持默认</param>
        /// <param name="shift">偏移量</param>
        /// <returns>ass字符串</returns>
        public static string ConvertToAss(IEnumerable<Library.Danmaku> danmakus, int videoWidth, int videoHeight, string fontName = "Microsoft YaHei", int fontSize = 64, int lineCount = 14, int bottomMargin = 180, float shift = 0.0f)
        {
            var header = $@"[Script Info]
ScriptType: v4.00+
Collisions: Normal
PlayResX: {videoWidth}
PlayResY: {videoHeight}

[V4+ Styles]
Format: Name, Fontname, Fontsize, PrimaryColour, SecondaryColour, OutlineColour, BackColour, Bold, Italic, Underline, StrikeOut, ScaleX, ScaleY, Spacing, Angle, BorderStyle, Outline, Shadow, Alignment, MarginL, MarginR, MarginV, Encoding
Style: AcplayDefault, {fontName}, {fontSize}, &H00FFFFFF, &H00FFFFFF, &H00000000, &H00000000, 0, 0, 0, 0, 100, 100, 0.00, 0.00, 1, 1, 0, 2, 20, 20, 20, 0

[Events]
Format: Layer, Start, End, Style, Name, MarginL, MarginR, MarginV, Effect, Text
";
            var dic1 = new Dictionary<int, float>();
            var dic2 = new Dictionary<int, float>();
            var asses = danmakus
                .Where(d => d.Type == DanmakuType.Normal || d.Type == DanmakuType.Normal2 || d.Type == DanmakuType.Normal3
                || d.Type == DanmakuType.Top || d.Type == DanmakuType.Bottom || d.Type == DanmakuType.Reverse)
                .Select(d => new AssSubtitle(d, dic1, dic2, videoWidth, videoHeight, fontSize, lineCount, bottomMargin, shift));
            return header + string.Join("\n", asses);
        }
        public static string ToAss(this IEnumerable<Library.Danmaku> danmakus, int videoWidth, int videoHeight, string fontName = "Microsoft YaHei", int fontSize = 64, int lineCount = 14, int bottomMargin = 180, float shift = 0.0f)
        {
            return ConvertToAss(danmakus, videoWidth, videoHeight, fontName, fontSize, lineCount, bottomMargin, shift);
        }
        /// <summary>
        /// 将弹幕列表转换成xml字符转
        /// </summary>
        /// <param name="danmakus">弹幕列表</param>
        /// <param name="cid">视频的cid，可以不管</param>
        /// <returns>弹幕xml字符串</returns>
        public static string ConvertToXml(IEnumerable<Library.Danmaku> danmakus,int cid=0)
        {
            var xdoc = new XDocument(new XDeclaration("1.0", "utf-8", ""));
            xdoc.Add(new XElement("i",
                new XElement("chatserver", "chat.bilibili.com"),
                new XElement("chatid", cid),
                new XElement("mission", 0),
                new XElement("state", 0),
                new XElement("real_name", 0),
                new XElement("source", "k-v"),
                danmakus.Select(d =>
                new XElement("d",
                new XAttribute("p",
                $"{string.Format("{0:F5}", d.ShowTime)},{Convert.ToInt32(d.Type)},{Convert.ToInt32(d.Size)},{d.Colour},{d.SendTime},{Convert.ToInt32(d.PoolType)},{d.SenderUIDHash},{d.Id}"),
                d.Content))));
            return xdoc.ToString();
        }
        public static string ToXml(this IEnumerable<Library.Danmaku> danmakus,int cid = 0)
        {
            return ConvertToXml(danmakus, cid);
        }
    }
}
