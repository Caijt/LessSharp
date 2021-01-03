using System;
using System.Collections.Generic;
using System.Text;

namespace LessSharp.Dto
{
    public class AttachSaveDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public string Ext { get; set; }
        public string Path { get; set; }
        public string EntityName { get; set; }
        public Guid EntityGuid { get; set; }
        public bool IsPublic { get; set; }
        public string Type { get; set; }
    }
}
