using DanmakuToAss.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DanmakuToAss.Protobuf
{
    public class DanmakuParser
    {
        /// <summary>
        /// 通过二进制protobuf的stream加载
        /// </summary>
        /// <param name="proto">二进制protofub的stream</param>
        /// <returns>弹幕列表</returns>
        private static IList<Danmaku> LoadProto(Stream proto)
        {
            var msg = DmSegMobileReply.Parser.ParseFrom(proto);
            var list = msg.Elems.ToList().Select(e => new Danmaku()
            {
                Colour = Convert.ToInt32(e.Color),
                Content = e.Content,
                Id = e.Id,
                PoolType = (DanmakuPoolType)e.Pool,
                SenderUIDHash = e.MidHash,
                SendTime = Convert.ToInt32(e.Ctime),
                ShowTime = (float)TimeSpan.FromMilliseconds(e.Progress).TotalSeconds,
                Size = (DanmakuSize)e.Fontsize,
                Type = (DanmakuType)e.Mode,
                Weight = e.Weight
            }).ToList();
            return list;
        }
        /// <summary>
        /// 通过多个二进制protobuf的stream加载弹幕列表
        /// </summary>
        /// <param name="protos">多个二进制protobuf的stream</param>
        /// <returns>弹幕列表</returns>
        public static IList<Danmaku> LoadMutipleProto(params Stream[] protos)
        {
            var list = new List<Danmaku>();
            foreach (var danmaku in protos)
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
