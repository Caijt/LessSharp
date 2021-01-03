using System;
using System.Collections.Generic;
using System.Text;

namespace LessSharp.Entity.Sys
{
    public class MenuApi:IEntity
    {
        public int ApiId { get; set; }
        public Api Api { get; set; }
        public int MenuId { get; set; }
        public Menu Menu { get; set; }
    }
}
