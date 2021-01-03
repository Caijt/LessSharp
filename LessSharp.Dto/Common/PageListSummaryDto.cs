using System;
using System.Collections.Generic;
using System.Text;

namespace LessSharp.Dto
{
    public class PageListSummaryDto<T> : PageListDto<T>
    {
        public object Summary { get; set; }
    }
}
