using System;
using System.Collections.Generic;
using System.Text;

namespace LessSharp.Entity.Sys
{
    public class Token : IEntity<int>, ICreateTime
    {
        public int Id { get; set; }
        public string AccessToken { get; set; }
        public DateTime AccessExpire { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Ip { get; set; }
        public string RefreshToken { get; set; }
        public bool IsDisabled { get; set; }
        public DateTime RefreshExpire { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
