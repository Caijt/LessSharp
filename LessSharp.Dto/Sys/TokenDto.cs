using System;
using System.Collections.Generic;
using System.Text;

namespace LessSharp.Dto.Sys
{
    public class TokenDto
    {
        public string AccessToken { get; set; }
        public DateTime AccessExpire { get; set; }
        public int UserId { get; set; }
        public string Ip { get; set; }
        public string UserLoginName { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshExpire { get; set; }
        public DateTime CreateTime { get; set; }
        public bool IsDisabled { get; set; }
    }
}
