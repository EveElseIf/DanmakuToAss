using System.Collections.Generic;
using System.Linq;

namespace DanmakuToAss.Library
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
        public static string ConvertToAss(IEnumerable<Danmaku> danmakus, int videoWidth, int videoHeight, string fontName = "Microsoft YaHei", int fontSize = 64, int lineCount = 14, int bottomMargin = 180, float shift = 0.0f)
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
        public static string ToAss(this IEnumerable<Danmaku> danmakus, int videoWidth, int videoHeight, string fontName = "Microsoft YaHei", int fontSize = 64, int lineCount = 14, int bottomMargin = 180, float shift = 0.0f)
        {
            return ConvertToAss(danmakus, videoWidth, videoHeight, fontName, fontSize, lineCount, bottomMargin, shift);
        }
    }
}
