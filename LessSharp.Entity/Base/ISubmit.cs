using System;
using System.Collections.Generic;
using System.Text;

namespace LessSharp.Entity
{
    public interface ISubmitUserId
    {
        int SubmitUserId { get; set; }
    }
    public interface ISubmitDate
    {
        DateTime? SubmiteDate { get; set; }
    }
    public interface ISubmit : ISubmitUserId, ISubmitDate
    { }
}
