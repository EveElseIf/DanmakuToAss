using DanmakuToAss.Library;
using ProtobufParser = DanmakuToAss.Protobuf.DanmakuParser;
using ProtobufConverter = DanmakuToAss.Protobuf.DanmakuConverter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DanmakuToAss
{
    class Program
    {
        private readonly static Config config = new Config();
        static void Main(string[] args)
        {
            Console.WriteLine("XML/Protobuf弹幕->ASS字幕转换工具 v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
            Console.WriteLine("https://github.com/EveElseIf/DanmakuToAss");
            Console.WriteLine("输入-help获取帮助");
            var argList = args.ToList();
            if (argList.Any())
            {
                Console.WriteLine("命令参数:");
                argList.ForEach(Console.WriteLine);
                var fileList = ExtractParameter(argList);//在这里不try，因为命令行参数有问题就终止程序
                if (config.ProtobufMode)
                {
                    try
                    {
                        Console.WriteLine("使用proto模式");
                        fileList = fileList.Where(f => f.ToUpper().Contains(".BIN")).ToList();
                        var danmakus = ProtobufParser.LoadMutipleProto(fileList.Select(f => File.Open(f, FileMode.Open)).ToArray());
                        var ass = ProtobufConverter.ConvertToAss(danmakus, config.Width, config.Height, "Microsoft YaHei", 64, config.Line, config.Bottom, config.Shift);
                        var outputPath = config.OutputPath.Contains('\\') ? config.OutputPath : Path.Combine(Path.GetDirectoryName(fileList[0]), config.OutputPath);
                        File.WriteAllText(outputPath, ass, Encoding.UTF8);
                    }
                    catch(FileNotFoundException ex)
                    {
                        Console.WriteLine("找不到文件 " + ex.Message);
                        throw;
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                }
                else
                {
                    try
                    {
                        Console.WriteLine("使用xml模式");
                        fileList.AsParallel().ForAll(ConvertXml);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                }
            }
        }
        private static void ConvertXml(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("文件名为空");
            if (!File.Exists(path)) throw new FileNotFoundException("文件不存在：" + path);
            var extension = Path.GetExtension(path);
            if (string.IsNullOrWhiteSpace(extension) || (extension.ToUpper() != ".XML" && extension.ToUpper() != ".BIN"))
                throw new InvalidOperationException("文件拓展名不是.xml或.bin:" + path);
            try
            {
                var danmakus = DanmakuParser.LoadXmlFromFile(path);
                var ass = DanmakuConverter.ConvertToAss(danmakus, config.Width, config.Height, "Microsoft YaHei", 64, config.Line, config.Bottom, config.Shift);
                File.WriteAllText(Path.ChangeExtension(path, ".ass"), ass, Encoding.UTF8);
            }
            catch
            {
                throw new Exception("处理文件时出错:" + path);
            }
        }

        private static List<string> ExtractParameter(IList<string> args)
        {
            var fileList = new List<string>();
            for (int i = 0; i < args.Count(); i++)
            {
                try
                {
                    var para = args[i];
                    switch (para.ToUpper())
                    {
                        case "-HELP":
                            Console.WriteLine("可用参数列表:");
                            Console.WriteLine("[-help] [-p|--proto output] [-width xxx] [-height xxx] [-line xxx] [-bottom xxx] [-shift xxx.xxx] File [File2,File3...]");
                            Console.WriteLine("-help\t显示此提示");
                            Console.WriteLine("-p或--proto\t使用protobuf模式，设置输出文件名称，由于protobuf弹幕采用分包，需要合并，此时将忽略传入的xml文件");
                            Console.WriteLine("-width xxx\t设置视频宽度为xxx，默认为1920");
                            Console.WriteLine("-height xxx\t设置视频高度为xxx，默认为1080");
                            Console.WriteLine("-bottom x\t设置为底部保留x分之一的区域，默认为6");
                            Console.WriteLine("-shift xxx.xxx\t设置弹幕晚出现x秒，默认为0");
                            Console.WriteLine("XmlFile\t文件完整路径，多个文件用空格隔开，包含空格的路径使用双引号括起来");
                            break;
                        case "-WIDTH":
                            config.Width = int.Parse(args[i + 1]);
                            i++;
                            break;
                        case "-HEIGHT":
                            config.Height = int.Parse(args[i + 1]);
                            i++;
                            break;
                        case "-LINE":
                            config.Line = int.Parse(args[i + 1]);
                            i++;
                            break;
                        case "-BOTTOM":
                            config.Bottom = int.Parse(args[i + 1]);
                            i++;
                            break;
                        case "-SHIFT":
                            config.Shift = float.Parse(args[i + 1]);
                            i++;
                            break;
                        case "-P":
                        case "--PROTO":
                            config.OutputPath = args[i + 1];
                            i++;
                            if(Path.GetExtension(config.OutputPath).ToUpper()!=".ASS")
                                Console.WriteLine($"警告:输出文件设置为{config.OutputPath}，拓展名不为.ass，请检查是否输入错误");
                            config.ProtobufMode = true;
                            break;
                        default:
                            fileList.Add(para);
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("参数错误");
                    throw;
                }
            }
            return fileList;
        }
    }
    class Config
    {
        public int Width { get; set; } = 1920;
        public int Height { get; set; } = 1080;
        public int Line { get; set; } = 14;
        public float Shift { get; set; } = 0.0f;
        public int Bottom { get; set; } = 6;
        public bool ProtobufMode { get; set; } = false;
        public string OutputPath { get; set; } = "";
    }
}