using System;
using System.Collections.Generic;
using System.Text;

namespace LessSharp.Dto.Sys
{
    public class MenuQueryDto : QueryDto
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public int? NotId { get; set; }
        public bool? IsPage { get; set; }
    }
}
