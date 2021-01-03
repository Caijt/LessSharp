using System;
using System.Collections.Generic;
using System.Text;

namespace LessSharp.Entity
{
    public interface IUpdateUserId
    {
        int UpdateUserId { get; set; }
    }
    public interface IUpdateTime
    {
        DateTime UpdateTime { get; set; }
    }
    public interface IUpdate : IUpdateUserId, IUpdateTime
    {

    }
}
