# DanmakuToAss
一个可以将B站xml弹幕和protobuf弹幕转换为ass文件的项目

# How to use
先克隆源码。。。
## Use library
先在你的项目引用DanmakuToAss.Library

1. 读取xml弹幕文件
`
var danmakuList = DanmakuParser.LoadXml(filePath);
`

2. 生成ass字符串
`
var assString = DanmakuConverter.ConvertToAss(danmakuList,1920,1080);
`

## Use Console
使用.net core编译DanmakuToAss，直接运行生成的.exe可执行文件，可以直接将需要转换的文件托到.exe可执行文件上

如果要转换protobuf弹幕，需要使用命令行参数-p output file1 file2 file3，其中拓展名不是.bin的文件将被忽略，程序将把protobuf弹幕分包合并，并输出到output

# 其他说明
参考了 https://github.com/kaedei/danmu2ass 的代码

B站新增了许多复杂的弹幕种类，所以本项目尚未支持，日后慢慢研究。。。
