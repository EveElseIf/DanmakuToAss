using System.Collections.Generic;
using System.Linq;

namespace DanmakuToAss.Protobuf
{
    public class DanmakuConverter
    {
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
            var asses = danmakus.Select(d => new AssSubtitle(d, videoWidth, videoHeight, fontSize, lineCount, bottomMargin, shift));
            return header + string.Join("\n", asses);
        }
    }
}
