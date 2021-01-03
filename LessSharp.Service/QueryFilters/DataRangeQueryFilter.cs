using LessSharp.Common;
using LessSharp.Data;
using LessSharp.Entity.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace LessSharp.Service.QueryFilters
{
    public interface IDataRangeQueryFilter<TEntity>
    {
        Expression<Func<TEntity, int>> CreateUserIdField { get; set; }
    }
    public class DataRangeQueryFilter<TEntity> : QueryFilter<TEntity, IDataRangeQueryFilter<TEntity>>
    {
        private readonly AuthContext _identityContext;
        private readonly AppDbContext _appDbContext;
        public DataRangeQueryFilter(AuthContext identityContext, AppDbContext dbContext)
        {
            this._identityContext = identityContext;
            _appDbContext = dbContext;
        }

        protected override IQueryable<TEntity> OnFilter(IQueryable<TEntity> query, IDataRangeQueryFilter<TEntity> service)
        {
            if (service.CreateUserIdField == null)
            {
                return query;
            }
            //var isAllRange = _appDbContext.Set<User>().Where(e => e.Id == _identityContext.UserId).Select(e => e.Role.IsAllRange).FirstOrDefault();
            var isAllRange = false;
            //if (_identityContext.UserId == -1)
            //{
            //    return query;
            //}
            if (isAllRange)
            {
                return query;
            }
            Expression<Func<TEntity, int>> CreateUserIdField = service.CreateUserIdField;
            var constant = Expression.Constant(_identityContext.UserId, _identityContext.UserId.GetType());
            var body = Expression.Equal(CreateUserIdField.Body, constant);
            var whereExp = Expression.Lambda<Func<TEntity, bool>>(body, CreateUserIdField.Parameters);
            return query.Where(whereExp);
        }

    }
}
