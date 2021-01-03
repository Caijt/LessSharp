using LessSharp.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using LessSharp.Data;
using LessSharp.Common;

namespace LessSharp.Service.EntityHandlers
{
    public class DeleteUserIdEntityHandler<TEntity> : EntityHandler<TEntity, IDeleteUserId>
    {
        private readonly AuthContext _identityContext;
        public DeleteUserIdEntityHandler(AuthContext identityContext)
        {
            _identityContext = identityContext;
        }
        protected override void OnCreating(IDeleteUserId entity)
        {
            entity.DeleteUserId = null;
        }

        public override void OnUpdating(IDeleteUserId entity, List<Expression<Func<IDeleteUserId, object>>> excludeFields, bool excludeField)
        {
            if (excludeField)
            {
                excludeFields.Add(e => e.DeleteUserId);
            }

        }
        public override void OnSoftDeleting(IDeleteUserId entity, List<Expression<Func<IDeleteUserId, object>>> includeProps)
        {
            entity.DeleteUserId = _identityContext.UserId;
            includeProps.Add(e => e.DeleteUserId);
        }
    }
}
