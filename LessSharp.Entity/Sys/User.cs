using System;
using System.Collections.Generic;
using System.Text;

namespace LessSharp.Entity.Sys
{
    public class User : IEntity<int>, ICreateTime, IUpdateTime
    {
        public int Id { get; set; }
        public string LoginName { get; set; }
        public string LoginPassword { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public bool IsDisabled { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }

    }
}
