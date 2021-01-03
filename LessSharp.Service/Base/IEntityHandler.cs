using LessSharp.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace LessSharp.Service
{
    public interface IEntityHandler<TEntity>
    {
        void Saving(TEntity entity);
        void Creating(TEntity entity);
        void Updating(TEntity entity, List<Expression<Func<TEntity, object>>> excludeOrIncludeFields, bool excludeField = true);

        void Deleting(TEntity entity);
        void SoftDeleting(TEntity entity, List<Expression<Func<TEntity, object>>> includeProps);
    }


}
