using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPTool
{
    /// <summary>
    /// 定义的MQ消息类型
    /// </summary>
    public class TextMessage
    {
        public string Text { get; set; }
        public string Type { get; set; }
        public string SiteId { get; set; }
    }
}
