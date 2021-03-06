﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("DanmakuToAss.Protobuf")]
namespace DanmakuToAss.Library
{
    internal class AssSubtitle
    {
        private readonly Danmaku danmaku;
        private readonly Dictionary<int, float> bottomSubtitles;
        private readonly Dictionary<int, float> topSubtitles;
        private readonly int videoWidth;
        private readonly int videoHeight;
        private readonly int baseFontSize;
        private readonly int lineCount;
        private readonly int bottomMargin;
        private readonly float tuneSeconds;
        private readonly int textLength;
        private readonly float startTime;
        private readonly float endTime;
        private readonly int fontSize;
        private readonly Position position;
        private readonly string styledText;

        public AssSubtitle(Danmaku danmaku,Dictionary<int,float> bottomSubtitles,Dictionary<int,float> topSubtitles, int videoWidth, int videoHeight, int baseFontSize, int lineCount, int bottomMargin, float tuneSeconds)
        {
            this.danmaku = danmaku;
            this.bottomSubtitles = bottomSubtitles;
            this.topSubtitles = topSubtitles;
            this.videoWidth = videoWidth;
            this.videoHeight = videoHeight;
            this.baseFontSize = baseFontSize;
            this.lineCount = lineCount;
            this.bottomMargin = bottomMargin;
            this.tuneSeconds = tuneSeconds;

            this.textLength = danmaku.Content.Length;
            this.startTime = danmaku.ShowTime;
            this.endTime = GetEndTime(danmaku.ShowTime, this.tuneSeconds);
            this.fontSize = (int)danmaku.Size - 25 + baseFontSize;
            this.position = GetPosition();
            this.styledText = GetStyledText();
        }
        public override string ToString()
        {
                string format(string s)
                {
                    if (!s.Contains('.')) return s.Insert(s.Length, ".00");
                    if (s.Substring(s.LastIndexOf('.')).Length == 2) return s + "0";
                    if (s.Substring(s.LastIndexOf('.')).Length > 3)
                    {
                        var index = s.LastIndexOf('.');
                        return s.Remove(index + 3, s.Length - index - 3);
                    }
                    return s;
                }
                var start = format(TimeSpan.FromSeconds(this.startTime).ToString("c"));
                var end = format(TimeSpan.FromSeconds(this.endTime).ToString("c"));
                return $"Dialogue: 3,{start},{end},AcplayDefault,,0000,0000,0000,,{this.styledText}";
        }

        private string GetStyledText()
        {
            string colourMarkup;
            string borderMarkup;
            string fontSizeMarkup;
            string styleMarkup;
            if (this.danmaku.Colour == 16777215) colourMarkup = "";
            else colourMarkup = $"\\c&H{this.danmaku.Colour.ToHexString()}";
            if (NeedWhiteBorder(this.danmaku)) borderMarkup = "\\3c&HFFFFFF";
            else borderMarkup = "";
            if (this.fontSize == this.baseFontSize) fontSizeMarkup = "";
            else fontSizeMarkup = $"\\fs{this.fontSize}";
            if (this.danmaku.Type == DanmakuType.Normal || this.danmaku.Type == DanmakuType.Normal2
                || this.danmaku.Type == DanmakuType.Normal3 || this.danmaku.Type == DanmakuType.Reverse)
                styleMarkup = $"\\move({this.position.X1},{this.position.Y1},{this.position.X2},{this.position.Y2})";
            else 
                styleMarkup = $"\\a6\\pos({this.position.X1},{this.position.Y1})";
            return $"{{{string.Join("", styleMarkup, colourMarkup, borderMarkup, fontSizeMarkup)}}}{this.danmaku.Content}";
        }

        private bool NeedWhiteBorder(Danmaku danmaku)
        {
            var colour = danmaku.Colour;
            if (colour == 0) return true;
            var hls = danmaku.Colour.ToRgb().ToHls();
            if (hls.H > 30 && hls.H < 210 && hls.L < 33) return true;
            if ((hls.L < 30 || hls.H > 210) && hls.L < 66) return true;
            return false;
        }

        private Position GetPosition()
        {
            var position = new Position();
            if (this.danmaku.Type == DanmakuType.Normal || this.danmaku.Type == DanmakuType.Normal2 || this.danmaku.Type == DanmakuType.Normal3)
            {
                position.X1 = this.videoWidth + this.baseFontSize * this.textLength / 2;
                position.X2 = -(this.baseFontSize * this.textLength) / 2;
                int y = (this.danmaku.Index % this.lineCount + 1) * this.baseFontSize;
                if (y < this.fontSize) y = this.fontSize;
                position.Y1 = position.Y2 = y;
            }
            else if (this.danmaku.Type == DanmakuType.Bottom)
            {
                var lineIndex = ChooseLineCount(this.bottomSubtitles, this.startTime);
                if (this.bottomSubtitles.Keys.Contains(lineIndex))
                    this.bottomSubtitles[lineIndex] = this.endTime;
                else
                    this.bottomSubtitles.Add(lineIndex, this.endTime);
                int x = this.videoWidth / 2;
                int y = this.videoHeight - (this.baseFontSize * lineIndex + this.bottomMargin);
                position.X1 = position.X2 = x;
                position.Y1 = position.Y2 = y;
            }
            else if (this.danmaku.Type == DanmakuType.Top)
            {
                var lineIndex = ChooseLineCount(this.topSubtitles, this.startTime);
                if (this.topSubtitles.Keys.Contains(lineIndex)) 
                    this.topSubtitles[lineIndex] = this.endTime;
                else
                    this.topSubtitles.Add(lineIndex, this.endTime);
                int x = this.videoWidth / 2;
                int y = this.baseFontSize * lineIndex + 1;
                position.X1 = position.X2 = x;
                position.Y1 = position.Y2 = y;
            }
            else if (this.danmaku.Type==DanmakuType.Reverse)
            {
                position.X2 = this.videoWidth + this.baseFontSize * this.textLength / 2;
                position.X1 = -(this.baseFontSize * this.textLength) / 2;
                int y = (this.danmaku.Index % this.lineCount + 1) * this.baseFontSize;
                if (y < this.fontSize) y = this.fontSize;
                position.Y1 = position.Y2 = y;
            }
            else
            {
                throw new ArgumentException($"不支持的弹幕类型:内容为{this.danmaku.Content}，类型为{this.danmaku.Type}");
            }
            return position;
        }
        private int ChooseLineCount(Dictionary<int, float> subtitles, float startTime)
        {
                int lineIndex;
                var toRemove = new List<int>();
                foreach (var item in subtitles)
                {
                    if (item.Value <= startTime) toRemove.Add(item.Key);
                }
                toRemove.ForEach(s => subtitles.Remove(s));
                if (subtitles.Count == 0)
                    lineIndex = 0;
                else if (subtitles.Count == subtitles.Keys.Max())
                    lineIndex = subtitles.Keys.Min();
                else
                {
                    lineIndex = 0;
                    for (int i = 0; i < subtitles.Count; i++)
                    {
                        if (!subtitles.ContainsKey(i))
                        {
                            lineIndex = i;
                            break;
                        }
                    }
                    if (lineIndex == 0)
                        lineIndex = subtitles.Count;
                }
                return lineIndex;
        }

        private float GetEndTime(float showTime, float offset)
        {
            if (this.danmaku.Type == DanmakuType.Bottom || this.danmaku.Type == DanmakuType.Top)
                return showTime + 4;
            float endTime;
            if (this.textLength < 5) endTime = showTime + 7 + this.textLength / 1.5f;
            else if (this.textLength < 12) endTime = showTime + 7 + this.textLength / 2;
            else endTime = showTime + 13;
            endTime += offset;
            return endTime;
        }
        private struct Position
        {
            public int X1 { get; set; }
            public int Y1 { get; set; }
            public int X2 { get; set; }
            public int Y2 { get; set; }
        }
    }
}
