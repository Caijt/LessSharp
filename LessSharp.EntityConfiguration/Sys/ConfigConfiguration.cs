using LessSharp.Entity.Sys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LessSharp.EntityConfiguration.Sys
{
    public class ConfigConfiguration : IEntityTypeConfiguration<Config>
    {
        public void Configure(EntityTypeBuilder<Config> builder)
        {
            builder.HasKey(e => e.Key);
            builder.Property(e => e.Type).HasConversion<string>();
            builder.Property(e => e.Key).HasConversion<string>();
            //初始数据
            builder.HasData(
                new Config { Key = ConfigKey.SYSTEM_TITLE, Name = "系统标题", Value = "LessAdmin快速开发框架", Type = ConfigType.STRING },
                new Config { Key = ConfigKey.MENU_BAR_TITLE, Name = "菜单栏标题", Value = "LessAdmin", Type = ConfigType.STRING },
                new Config { Key = ConfigKey.VERSION, Name = "版本号", Value = "20200414001", Type = ConfigType.STRING },
                new Config { Key = ConfigKey.IS_REPAIR, Name = "网站维护", Value = "OFF", Type = ConfigType.BOOL },
                new Config { Key = ConfigKey.LAYOUT, Name = "后台布局", Value = "leftRight", Type = ConfigType.STRING },
                new Config { Key = ConfigKey.LIST_DEFAULT_PAGE_SIZE, Name = "列表默认页容量", Value = "10", Type = ConfigType.NUMBER },
                new Config { Key = ConfigKey.MENU_DEFAULT_ICON, Name = "菜单默认图标", Value = "el-icon-menu", Type = ConfigType.STRING },
                new Config { Key = ConfigKey.SHOW_MENU_ICON, Name = "是否显示菜单图标", Value = "OFF", Type = ConfigType.BOOL },
                new Config { Key = ConfigKey.LOGIN_VCODE, Name = "登录需要验证码", Value = "OFF", Type = ConfigType.BOOL },
                new Config { Key = ConfigKey.PAGE_TABS, Name = "使用多页面标签", Value = "ON", Type = ConfigType.BOOL }
            );
        }
    }
}
