using System;
using System.Collections.Generic;
using System.Text;

namespace LessSharp.Dto
{
    public class WeixinJssdkConfigDto
    {
        public string AppId { get; set; }
        public long Timestamp { get; set; }
        public string NonceStr { get; set; }
        public string Signature { get; set; }
    }
}
