using System;
using System.Collections.Generic;
using System.Text;

namespace LessSharp.Option
{
    public class UploadOption
    {
        /// <summary>
        /// 公开附件上传路径
        /// </summary>
        public string PublicUploadPath { get; set; }

        /// <summary>
        /// 公开附件路径前缀
        /// </summary>
        public string PublicPathPrefix { get; set; }

        /// <summary>
        /// 私密附件上传路径
        /// </summary>
        public string PrivateUploadPath { get; set; }
    }
}
