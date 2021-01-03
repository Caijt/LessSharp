using System;
using System.Collections.Generic;
using System.Text;

namespace LessSharp.Entity
{
    public interface ICreateUserId
    {
        int CreateUserId { get; set; }
    }
    public interface ICreateTime
    {
        DateTime CreateTime { get; set; }
    }
    public interface ICreate : ICreateTime, ICreateUserId
    {

    }
}
