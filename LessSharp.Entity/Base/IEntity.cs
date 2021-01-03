using System;
using System.Collections.Generic;
using System.Text;

namespace LessSharp.Entity
{
    public interface IEntity { }
    public interface IEntity<T> : IEntity
    {
        T Id { get; set; }
    }
}
