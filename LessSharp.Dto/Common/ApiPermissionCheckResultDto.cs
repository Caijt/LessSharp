using System;
using System.Collections.Generic;
using System.Text;

namespace LessSharp.Dto.Common
{
    public class ApiPermissionCheckResultDto
    {
        public bool IsSuccess { get; set; }
        public string ApiName { get; set; }
    }
}
