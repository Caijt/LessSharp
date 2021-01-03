using System;
using System.Collections.Generic;
using System.Text;

namespace LessSharp.Entity.Sys
{
    public class RoleMenu : IEntity
    {
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public int MenuId { get; set; }
        public Menu Menu { get; set; }

        public bool CanRead { get; set; }
        public bool CanWrite { get; set; }
        public bool CanReview { get; set; }

    }
}
