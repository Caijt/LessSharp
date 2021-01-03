using LessSharp.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using LessSharp.Data;

namespace LessSharp.Service.EntityHandlers
{
    public class DeleteTimeEntityHandler<TEntity> : EntityHandler<TEntity, IDeleteTime>
    {
        private readonly AppDbContext _appDbContext;
        public DeleteTimeEntityHandler(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        protected override void OnCreating(IDeleteTime entity)
        {
            entity.DeleteTime = null;
        }

        public override void OnUpdating(IDeleteTime entity, List<Expression<Func<IDeleteTime, object>>> excludeFields, bool excludeField)
        {
            if (excludeField)
            {
                excludeFields.Add(e => e.DeleteTime);
            }

        }
        public override void OnSoftDeleting(IDeleteTime entity, List<Expression<Func<IDeleteTime, object>>> includeProps)
        {
            entity.DeleteTime = DateTime.Now;
            includeProps.Add(e => e.DeleteTime);
        }
    }
}
