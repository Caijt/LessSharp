using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LessSharp.Service
{
    public abstract class QueryFilter<TQuery,TFilter> : IQueryFilter<TQuery>
    {
        public IQueryable<TQuery> Filter(IQueryable<TQuery> query, object service)
        {
            if (typeof(TFilter).IsAssignableFrom(service.GetType()))
            {
                return OnFilter(query, (TFilter)service);
            }
            return query;
        }
        protected abstract IQueryable<TQuery> OnFilter(IQueryable<TQuery> query, TFilter service);
    }
}
