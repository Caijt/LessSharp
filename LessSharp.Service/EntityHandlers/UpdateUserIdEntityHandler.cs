using LessSharp.Common;
using LessSharp.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace LessSharp.Service.EntityHandlers
{
    public class UpdateUserIdEntityHandler<TEntity> : EntityHandler<TEntity, IUpdateUserId> 
    {
        private readonly AuthContext _authContext;
        public UpdateUserIdEntityHandler(AuthContext authContext)
        {
            _authContext = authContext;
        }

        public override void OnUpdating(IUpdateUserId entity, List<Expression<Func<IUpdateUserId, object>>> excludeOrIncludeFields,bool excludeField)
        {
            entity.UpdateUserId = _authContext.UserId;
        }

        protected override void OnCreating(IUpdateUserId entity)
        {
            entity.UpdateUserId = _authContext.UserId;
        }
    }
}
