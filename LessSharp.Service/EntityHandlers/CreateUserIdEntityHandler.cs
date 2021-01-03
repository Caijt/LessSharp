using LessSharp.Common;
using LessSharp.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace LessSharp.Service.EntityHandlers
{
    public class CreateUserIdEntityHandler<TEntity> : EntityHandler<TEntity, ICreateUserId>
    {
        private readonly AuthContext _authContext;
        public CreateUserIdEntityHandler(AuthContext authContext)
        {
            _authContext = authContext;
        }

        public override void OnUpdating(ICreateUserId entity, List<Expression<Func<ICreateUserId, object>>> excludeOrIncludeFields, bool excludeField)
        {
            if (excludeField)
            {
                excludeOrIncludeFields.Add(e => e.CreateUserId);
            }

        }

        protected override void OnCreating(ICreateUserId entity)
        {
            entity.CreateUserId = _authContext.UserId;
        }
    }
}
