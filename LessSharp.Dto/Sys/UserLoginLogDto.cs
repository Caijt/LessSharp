using System;
using System.Collections.Generic;
using System.Text;

namespace LessSharp.Dto.Sys
{
    public class UserLoginLogDto
    {
        public int Id { get; set; }
        public string IpAddress { get; set; }
        public DateTime CreateTime { get; set; }
        public int UserId { get; set; }
        public string UserLoginName { get; set; }
    }
}
