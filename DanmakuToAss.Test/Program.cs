using DanmakuToAss.Protobuf;
using System;
using System.IO;
using System.Linq;

namespace DanmakuToAss.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var files = new string[]{ "response1.bin", "response2.bin", "response3.bin" };
            var streams = files.Select(f => File.Open(f, FileMode.Open)).ToArray();
            var danmakus = DanmakuParser.LoadMutipleProto(streams);
            var xml = DanmakuConverter.ConvertToXml(danmakus);
            Console.WriteLine(xml);
        }
    }
}
