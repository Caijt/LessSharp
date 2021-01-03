using System;
using System.Collections.Generic;
using System.Text;

namespace LessSharp.Dto.Common
{
    public class ChangePasswordDto
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
