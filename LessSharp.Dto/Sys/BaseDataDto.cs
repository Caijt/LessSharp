using System;
using System.Collections.Generic;
using System.Text;

namespace LessSharp.Dto.Sys
{
    public class BaseDataDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public bool IsDisabled { get; set; }
        public DateTime CreateTime { get; set; }
    }

    /// <summary>
    /// 来源渠道
    /// </summary>
    public class SourceChannelDto : BaseDataDto { }
    /// <summary>
    /// 灭害种类
    /// </summary>
    public class MetacilTypeDto : BaseDataDto { }
    /// <summary>
    /// 职务
    /// </summary>
    public class PostDto : BaseDataDto { }
    /// <summary>
    /// 岗位
    /// </summary>
    public class JobDto : BaseDataDto { }
    /// <summary>
    /// 客户类型
    /// </summary>
    public class CustomerTypeDto : BaseDataDto { }
    /// <summary>
    /// 投诉类型
    /// </summary>
    public class ComplainTypeDto : BaseDataDto { }
    /// <summary>
    /// 收款类型
    /// </summary>
    public class ReceiptTypeDto : BaseDataDto { }
    /// <summary>
    /// 收款方式
    /// </summary>
    public class ReceiptMethodDto : BaseDataDto { }
    /// <summary>
    /// 业务来源
    /// </summary>
    public class BusinessSourceDto : BaseDataDto { }

    /// <summary>
    /// 工资类型
    /// </summary>
    public class WageTypeDto : BaseDataDto { }

    /// <summary>
    /// 款项类别
    /// </summary>
    public class MoneyTypeDto : BaseDataDto { }
}
