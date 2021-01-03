using System;
using System.Collections.Generic;
using System.Text;

namespace LessSharp.Entity.Sys
{
    public class Role : IEntity<int>, ICreateTime, IUpdateTime
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Remarks { get; set; }
        public List<RoleMenu> RoleMenus { get; set; }
        public List<User> Users { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }

    }
}
