using System;
using System.Collections.Generic;
using System.Text;

namespace LessSharp.Dto
{
    public class OptionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public bool IsDisabled { get; set; }
    }
}
