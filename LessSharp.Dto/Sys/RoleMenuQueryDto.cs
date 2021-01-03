using System;
using System.Collections.Generic;
using System.Text;

namespace LessSharp.Dto.Sys
{
    public class RoleMenuQueryDto:QueryDto
    {
        public string RoleId { get; set; }
        public string MenuId { get; set; }
        public bool CanRead { get; set; }
        public bool CanWrite { get; set; }
        public bool CanReview { get; set; }
    }
}
