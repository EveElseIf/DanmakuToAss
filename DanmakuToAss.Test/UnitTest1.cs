using DanmakuToAss.Library;
using System;
using System.IO;
using Xunit;

namespace DanmakuToAss.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var ass = DanmakuParser.LoadXmlFromString(File.ReadAllText("test1.xml")).ToAss(1920, 1080);
            Assert.Contains("Script Info", ass);
        }
    }
}
