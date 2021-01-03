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
    public interface ICompanyQueryFilter<TEntity>
    {
        Expression<Func<TEntity, object>> CompanyField { get; }      
    }
    public class CompanyQueryFilter<TEntity> : QueryFilter<TEntity,  ICompanyQueryFilter<TEntity>>
    {
        private readonly AuthContext _identityContext;
        private readonly AppDbContext _dbContext;
        public CompanyQueryFilter(AuthContext identityContext, AppDbContext dbContext)
        {
            this._identityContext = identityContext;
            this._dbContext = dbContext;
        }

        protected override IQueryable<TEntity> OnFilter(IQueryable<TEntity> query,  ICompanyQueryFilter<TEntity> service)
        {
            if (service.CompanyField == null )
            {
                return query;
            }
            if (_identityContext.UserId == -1)
            {
                return query;
            }
            var companyStrIds = _dbContext.Set<User>()
                .Where(e => e.Id == _identityContext.UserId)
                .Select(e=>e.LoginName)
                .FirstOrDefault();
            if (string.IsNullOrWhiteSpace(companyStrIds))
            {
                return query.Where(e => false);
            }
            var companyIds = companyStrIds.Split(",").Select(i => int.Parse(i)).ToList();
            var constant = Expression.Constant(companyIds, companyIds.GetType());
            var method = companyIds.GetType().GetMethod("Contains");
            var body = Expression.Call(constant, method, service.CompanyField.Body);
            var whereExp = Expression.Lambda<Func<TEntity, bool>>(body, service.CompanyField.Parameters);
            return query.Where(whereExp);
        }

    }
}
