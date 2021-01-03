using System;
using System.Collections.Generic;
using System.Text;

namespace LessSharp.Dto
{
    public class AttachDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public string PublicPath { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
