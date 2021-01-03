using LessSharp.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace LessSharp.Service.EntityHandlers
{
    public class UpdateTimeEntityHandler<TEntity> : EntityHandler<TEntity, IUpdateTime> 
    {
        public override void OnUpdating(IUpdateTime entity, List<Expression<Func<IUpdateTime, object>>> excludeFields,bool excludeField)
        {
            entity.UpdateTime = DateTime.Now;
        }

        protected override void OnCreating(IUpdateTime entity)
        {
            entity.UpdateTime = DateTime.Now;
        }
    }
}
