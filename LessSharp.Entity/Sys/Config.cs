using System;
using System.Collections.Generic;
using System.Text;

namespace LessSharp.Entity.Sys
{
    public class Config : IEntity
    {
        public ConfigKey Key { get; set; }
        public string Value { get; set; }
        public ConfigType Type { get; set; }
        public string Name { get; set; }
    }

    public enum ConfigType
    {
        STRING,
        BOOL,
        NUMBER
    }
    public enum ConfigKey
    {
        /// <summary>
        /// 系统标题
        /// </summary>
        SYSTEM_TITLE,
        /// <summary>
        /// 菜单栏标题
        /// </summary>
        MENU_BAR_TITLE,
        /// <summary>
        /// 网页版本号
        /// </summary>
        VERSION,
        /// <summary>
        /// 系统是否在维护
        /// </summary>
        IS_REPAIR,
        /// <summary>
        /// 布局
        /// </summary>
        LAYOUT,
        /// <summary>
        /// 菜单默认图标
        /// </summary>
        MENU_DEFAULT_ICON,
        /// <summary>
        /// 是否显示菜单图标
        /// </summary>
        SHOW_MENU_ICON,
        /// <summary>
        /// 列表默认页容量
        /// </summary>
        LIST_DEFAULT_PAGE_SIZE,
        /// <summary>
        /// 登录需要验证码
        /// </summary>
        LOGIN_VCODE,
        /// <summary>
        /// 是否使用多页面标签
        /// </summary>
        PAGE_TABS,

    }
}
