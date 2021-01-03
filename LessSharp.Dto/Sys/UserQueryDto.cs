using System;
using System.Collections.Generic;
using System.Text;

namespace LessSharp.Dto.Sys
{
    public class UserQueryDto : QueryDto
    {
        public string EmployeeName { get; set; }
        public string LoginName { get; set; }
        public string RoleName { get; set; }
        public bool? IsDisabled { get; set; }
    }
}
