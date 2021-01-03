using System;
using System.Collections.Generic;
using System.Text;

namespace LessSharp.Entity.Sys
{
    public class Menu : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public string Path { get; set; }
        public string Icon { get; set; }
        public string ParentIds { get; set; }
        public int Order { get; set; }
        /// <summary>
        /// 是否在子页面
        /// </summary>
        public bool IsPage { get; set; }
        /// <summary>
        /// 是否能多开页面
        /// </summary>
        public bool CanMultipleOpen { get; set; }
        public bool HasRead { get; set; }
        public bool HasWrite { get; set; }
        public bool HasReview { get; set; }
        public  List<RoleMenu> RoleMenus { get; set; }
        public List<MenuApi> MenuApis { get; set; }
    }
}
