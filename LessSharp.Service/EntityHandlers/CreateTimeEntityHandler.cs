using LessSharp.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace LessSharp.Service.EntityHandlers
{
    public class CreateTimeEntityHandler<TEntity> : EntityHandler<TEntity, ICreateTime>
    {
        public override void OnDeleting(ICreateTime entity) { }

        public override void OnSaving(ICreateTime entity) { }

        public override void OnUpdating(ICreateTime entity, List<Expression<Func<ICreateTime, object>>> excludeProps, bool excludeField)
        {
            if (excludeField)
            {
                excludeProps.Add(e => e.CreateTime);
            }
        }

        protected override void OnCreating(ICreateTime entity)
        {
            entity.CreateTime = DateTime.Now;
        }
    }
}
