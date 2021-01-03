using System;
using System.Collections.Generic;
using System.Text;

namespace LessSharp.Dto.Sys
{
    public class UserSaveDto
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string LoginName { get; set; }
        public string LoginPassword { get; set; }
        
        public bool ChangePassword { get; set; }
        public bool IsDisabled { get; set; }
    }
}
