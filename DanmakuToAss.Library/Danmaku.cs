namespace DanmakuToAss.Library
{
    /*
        ### 属性 p

        字符串内每项用逗号`,`分隔

        | 项   | 含义               | 类型   | 备注                                                         |
        | ---- | ------------------ | ------ | ------------------------------------------------------------ |
        | 0    | 视频内弹幕出现时间 | float  | 秒                                                           |
        | 1    | 弹幕类型           | int32  | 1 2 3：普通弹幕<br />4：底部弹幕<br />5：顶部弹幕<br />6：逆向弹幕<br />7：高级弹幕<br />8：代码弹幕<br />9：BAS弹幕（`pool`必须为2） |
        | 2    | 弹幕字号           | int32  | 18：小<br />25：标准<br />36：大                             |
        | 3    | 弹幕颜色           | int32  | 十进制RGB888值                                               |
        | 4    | 弹幕发送时间       | int32  | 时间戳                                                       |
        | 5    | 弹幕池类型         | int32  | 0：普通池<br />1：字幕池<br />2：特殊池（代码/BAS弹幕）      |
        | 6    | 发送者UID的HASH    | string | 用于屏蔽用户和查看用户发送的所有弹幕   也可反查用户ID        |
        | 7    | 弹幕dmID           | int64  | 唯一  可用于操作参数                                         |
     */
    public class Danmaku
    {
        /// <summary>
        /// 出现时间
        /// </summary>
        public float ShowTime { get; set; }
        /// <summary>
        /// 弹幕类型
        /// </summary>
        public DanmakuType Type { get; set; }
        /// <summary>
        /// 字体大小
        /// </summary>
        public DanmakuSize Size { get; set; }
        /// <summary>
        /// 十进制颜色
        /// </summary>
        public int Colour { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public int SendTime { get; set; }
        /// <summary>
        /// 弹幕池类型
        /// </summary>
        public DanmakuPoolType PoolType { get; set; }
        /// <summary>
        /// 发送者uid的哈希
        /// </summary>
        public string SenderUIDHash { get; set; }
        /// <summary>
        /// 弹幕id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 按照时间排序后的索引
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 文字内容
        /// </summary>
        public string Content { get; set; }
    }
    public enum DanmakuType
    {
        Normal = 1,
        Normal2 = 2,
        Normal3 = 3,
        Bottom = 4,
        Top = 5,
        Reverse = 6,
        Advenced = 7,
        Code = 8,
        Bas = 9
    }
    public enum DanmakuSize
    {
        Small = 18,
        Normal = 25,
        Large = 36
    }
    public enum DanmakuPoolType
    {
        Normal = 0,
        SubTitle = 1,
        Special = 2
    }
}
