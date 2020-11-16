using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DanmakuToAss.Protobuf
{
    public class DanmakuParser
    {
        private static List<Danmaku> LoadProto(Stream proto)
        {
            var msg = DmSegMobileReply.Parser.ParseFrom(proto);
            var list = msg.Elems.ToList().Select(e => new Danmaku()
            {
                Colour = Convert.ToInt32(e.Color),
                Content = e.Content,
                Id = e.Id,
                PoolType = (DanmakuPoolType)e.Pool,
                SenderUIDHash = e.MidHash,
                SendTime = e.Ctime,
                ShowTime = (float)TimeSpan.FromMilliseconds(e.Progress).TotalSeconds,
                Size = (DanmakuSize)e.Fontsize,
                Type = (DanmakuType)e.Mode,
                Weight = e.Weight
            }).ToList();
            //var list = new List<Danmaku>();
            //foreach (var e in msg.Elems.ToList())
            //{
            //    var d = new Danmaku();
            //    d.Colour = Convert.ToInt32(e.Color);
            //    d.Content = e.Content;
            //    d.Id = e.Id;
            //    d.PoolType = (DanmakuPoolType)e.Pool;
            //    d.SenderUIDHash = e.MidHash;
            //    d.SendTime = e.Ctime;
            //    d.ShowTime = (float)TimeSpan.FromMilliseconds(e.Progress).TotalSeconds;
            //    d.Size = (DanmakuSize)e.Fontsize;
            //    d.Type = (DanmakuType)e.Mode;
            //    d.Weight = e.Weight;
            //    list.Add(d);
            //}
            //list = list.OrderBy(d => d.ShowTime).ToList();
            //var i = 0;
            //list.ForEach(d => { d.Index = i; i++; });
            return list;
        }
        public static List<Danmaku> LoadMutipleProto(params Stream[] proto)
        {
            var list = new List<Danmaku>();
            foreach (var danmaku in proto)
            {
                list = list.Concat(LoadProto(danmaku)).ToList();
            }
            list = list.OrderBy(d => d.ShowTime).ToList();
            var i = 0;
            list.ForEach(d => { d.Index = i; i++; });
            return list;
        }
    }
}
