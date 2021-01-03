using System;
using System.Collections.Generic;
using System.Text;

namespace LessSharp.Entity
{
    public class Attach : IEntity<int>, ICreate,IDelete
    {
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public int CreateUserId { get; set; }
        /// <summary>
        /// 附件原始文件名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 附件大小，单位字节
        /// </summary>
        public int Size { get; set; }
        /// <summary>
        /// 附件扩展名
        /// </summary>
        public string Ext { get; set; }
        /// <summary>
        /// 附件保存的路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 该附件关联的实体名或表名
        /// </summary>
        public string EntityName { get; set; }
        /// <summary>
        /// 附件关联的实体Guid
        /// </summary>
        public Guid EntityGuid { get; set; }
        /// <summary>
        /// 附件类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 该附件是否是公开附件
        /// </summary>
        public bool IsPublic { get; set; }
        public int? DeleteUserId { get; set; }
        public DateTime? DeleteTime { get; set; }
    }
}
