# DanmakuToAss
一个可以将B站xml弹幕文件转换为ass文件的项目

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

# 其他说明
参考了 https://github.com/kaedei/danmu2ass 的代码

B站新增了许多复杂的弹幕种类，所以本项目尚未支持，日后慢慢研究。。。
