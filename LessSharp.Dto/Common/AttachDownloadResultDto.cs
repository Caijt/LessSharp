using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LessSharp.Dto
{
    public class AttachDownloadResultDto
    {
        public Stream Stream { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; } = "application/octet-stream";
    }
}
