using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace LessSharp.Entity.Sys
{
    public class UserLoginLog : IEntity<int>, ICreateTime
    {
        public int Id { get; set; }
        public string IpAddress { get; set; }
        public DateTime CreateTime { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
