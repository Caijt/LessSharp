using AutoMapper;
using LessSharp.Data;
using LessSharp.Dto;
using LessSharp.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;

namespace LessSharp.Service
{
    /// <summary>
    /// 查询服务类
    /// </summary>
    /// <typeparam name="TDto">显示Dto类型</typeparam>
    /// <typeparam name="TQueryDto">查询参数Dto类型</typeparam>
    /// <typeparam name="TQuery">查询对象类型</typeparam>
    public abstract class QueryService<TDto, TQueryDto, TQuery>
        where TQuery : class
        where TQueryDto : QueryDto
    {
        /// <summary>
        /// 查询表达式缓存数组
        /// </summary>
        private readonly Dictionary<TQueryDto, List<Expression<Func<TQuery, bool>>>> _queryCache = new Dictionary<TQueryDto, List<Expression<Func<TQuery, bool>>>>();

        /// <summary>
        /// 表达式缓存数组，用来缓存GetExpressionByField方法组装的表达式
        /// </summary>
        private readonly Dictionary<string, Expression> _expressionCache = new Dictionary<string, Expression>();

        /// <summary>
        /// 数据库上下文
        /// </summary>
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// 对象映射操作类
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// 查询缓存类
        /// </summary>
        private IQueryable<TQuery> _dbQuery;

        /// <summary>
        /// 数据库操作上下文
        /// </summary>
        protected AppDbContext DbContext => _dbContext;

        /// <summary>
        /// 对象映射操作类
        /// </summary>
        protected IMapper Mapper => _mapper;

        /// <summary>
        /// 查询类
        /// </summary>
        protected IQueryable<TQuery> DbQuery
        {
            get
            {
                if (_dbQuery == null)
                {
                    if (!typeof(IQuery).IsAssignableFrom(typeof(TQuery)))
                    {
                        throw new Exception(typeof(TQuery).Name + "未实现IQuery接口，请传入实现IQuery接口的对象，或在构造函数中指定DbQuery属性值！");
                    }
                    IQueryable<TQuery> query = _dbContext.Set<TQuery>();
                    this.QueryFilters.ForEach(filter =>
                    {
                        query = filter.Filter(query, this);
                    });
                    _dbQuery = query;
                }
                return _dbQuery;
            }
            set => _dbQuery = value;
        }

        /// <summary>
        /// 是否默认Desc排序
        /// </summary>
        protected bool OrderDescDefaultValue { get; set; } = true;

        /// <summary>
        /// 默认排序字段
        /// </summary>
        protected Expression<Func<TQuery, object>> OrderDefaultField { get; set; }

        /// <summary>
        /// 汇总表达式，用于获取列表带汇总信息的列表
        /// </summary>
        protected Expression<Func<IGrouping<int, TQuery>, dynamic>> SummaryExpression { get; set; }

        /// <summary>
        /// 查询过滤器列表
        /// </summary>
        protected List<IQueryFilter<TQuery>> QueryFilters { get; set; } = new List<IQueryFilter<TQuery>>();

        /// <summary>
        /// 查询对象映射表达式，赋值时会采用这个映射规则，不赋值会使用autoMapper进行自动映射
        /// </summary>
        protected Expression<Func<TQuery, TDto>> QueryToDtoMapExpression { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="appDbContext"></param>
        /// <param name="mapper"></param>
        /// <param name="queryFilters"></param>
        public QueryService(AppDbContext appDbContext, IMapper mapper, IEnumerable<IQueryFilter<TQuery>> queryFilters)
        {
            this._dbContext = appDbContext;
            this._mapper = mapper;
            if (queryFilters != null)
            {
                this.QueryFilters.AddRange(queryFilters);
            }
        }

        #region 服务内部方法        

        /// <summary>
        /// 构建查询对象，将传入的查询对象根据查询参数进行条件过滤、字段排序、分页、对象映射处理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="queryDto"></param>
        /// <param name="selectExp"></param>
        /// <returns></returns>
        protected IQueryable<T> BuildQuery<T>(IQueryable<TQuery> query, TQueryDto queryDto, Expression<Func<TQuery, T>> selectExp = null)
        {
            if (queryDto == null)
            {
                if (selectExp == null)
                {
                    return Select<T>(query);
                }
                else
                {
                    return query.Select(selectExp);
                }
            }
            query = Where(query, queryDto);
            bool orderDesc = queryDto.OrderDesc.GetValueOrDefault(OrderDescDefaultValue);
            #region 默认排序
            if (OrderDefaultField != null)
            {
                query = orderDesc ? query.OrderByDescending(OrderDefaultField) : query.OrderBy(OrderDefaultField);
            }
            query = OnSort(query, queryDto.OrderField, orderDesc);
            #endregion

            Expression<Func<TQuery, object>> orderExpression = null;
            Expression<Func<T, object>> orderExpression2 = null;
            #region 排序表达式
            if (!string.IsNullOrEmpty(queryDto.OrderField))
            {
                //先判断是否有自定义排序表达式，有就直接按自定义排序表达式进行排序
                orderExpression = OnGetSortExpression(queryDto.OrderField);
                if (orderExpression == null)
                {
                    //没有的话就判断T对象是否有排序字段OrderField值同名属性（不区分大小写），有就按T同名属性排序
                    orderExpression2 = GetExpressionByField<T, object>(queryDto.OrderField);
                    if (orderExpression2 == null)
                    {
                        //没有的话就判断TQuery对象是否有排序字段OrderField值的同名属性（不区分大小写），有就按TQuery同名属性排序
                        orderExpression = GetExpressionByField<TQuery, object>(queryDto.OrderField);
                    }
                }
            }
            #endregion
            if (orderExpression != null)
            {
                query = orderDesc ? query.OrderByDescending(orderExpression) : query.OrderBy(orderExpression);
            }
            IQueryable<T> query2 = null;
            if (selectExp == null)
            {
                query2 = Select<T>(query);
            }
            else
            {
                query2 = query.Select(selectExp);
            }
            if (orderExpression2 != null)
            {
                query2 = orderDesc ? query2.OrderByDescending(orderExpression2) : query2.OrderBy(orderExpression2);
            }
            query2 = Page(query2, queryDto.PageIndex, queryDto.PageSize, queryDto.PageContinuity);
            return query2;
        }

        /// <summary>
        /// [异步] 转换成列表数据，将传入的查询对象根据查询参数进行条件过滤、字段排序、分页、对象映射处理并转换成列表数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="queryDto"></param>
        /// <param name="page"></param>
        /// <param name="selectExp">自定义映射表达式</param>
        /// <returns></returns>
        protected async Task<List<T>> QueryToListAsync<T>(
            IQueryable<TQuery> query, TQueryDto queryDto, Expression<Func<TQuery, T>> selectExp = null)
        {
            return await this.BuildQuery(query, queryDto, selectExp).ToListAsync();
        }

        /// <summary>
        /// 查询过滤方法
        /// </summary>
        /// <param name="query"></param>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        protected IQueryable<TQuery> Where(IQueryable<TQuery> query, TQueryDto queryDto)
        {
            //这里采用查询缓存，避免同一个查询参数对象多次进行查询处理
            if (_queryCache.TryGetValue(queryDto, out List<Expression<Func<TQuery, bool>>> whereExpressions))
            {
                foreach (var exp in whereExpressions)
                {
                    query = query.Where(exp);
                }
                return query;
            }
            query = OnFilter(query, queryDto);
            whereExpressions = new List<Expression<Func<TQuery, bool>>>();
            ExpressionToWhereExpressions(whereExpressions, query.Expression);
            _queryCache.Add(queryDto, whereExpressions);
            return query;
        }

        /// <summary>
        /// 将传入的查询对象经查询过滤器处理
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        protected IQueryable<TQuery> QueryFilter(IQueryable<TQuery> query)
        {
            this.QueryFilters.ForEach(filter =>
            {
                query = filter.Filter(query, this);
            });
            return query;
        }

        /// <summary>
        /// 将查询对象映射为T类型的查询对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        protected IQueryable<T> Select<T>(IQueryable<TQuery> query)
        {
            if (typeof(T) == typeof(TQuery))
            {
                return (IQueryable<T>)query;
            }
            return Mapper.ProjectTo<T>(query);
        }

        /// <summary>
        /// 列表分页方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        protected IQueryable<T> Page<T>(IQueryable<T> query, int pageIndex, int pageSize, bool isPageContinuity = false)
        {
            if (pageIndex == 0 && pageSize == 0)
            {
                return query;
            }
            int skipTotal;
            if (isPageContinuity)
            {
                skipTotal = pageIndex;
            }
            else
            {
                pageIndex = pageIndex - 1;
                if (pageIndex < 0) pageIndex = 0;
                skipTotal = pageIndex * pageSize;
            }
            return query.Skip(skipTotal).Take(pageSize);
        }

        /// [可重写] 查询过滤时处理方法
        /// </summary>
        /// <param name="query"></param>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        protected virtual IQueryable<TQuery> OnFilter(IQueryable<TQuery> query, TQueryDto queryDto)
        {
            return query;
        }

        /// <summary>
        /// [可重写] 查询列表字段排序处理方法
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual Expression<Func<TQuery, object>> OnGetSortExpression(string field)
        {
            return null;
        }

        /// <summary>
        /// [可重写] 查询列表字段排序处理方法
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual IQueryable<TQuery> OnSort(IQueryable<TQuery> query, string field, bool orderDesc) => query;

        /// <summary>
        /// [异步] 转换成列表数量，将传入的查询对象根据查询参数进行条件过滤处理并统计列表数量
        /// </summary>
        /// <param name="query"></param>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        protected async Task<int> QueryToTotalAsync(IQueryable<TQuery> query, TQueryDto queryDto)
        {
            query = query.AsNoTracking();
            query = Where(query, queryDto);
            return await query.CountAsync();
        }

        /// <summary>
        /// [异步] 获取列表汇总对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="queryDto"></param>
        /// <param name="summaryExpression"></param>
        /// <returns></returns>
        protected async Task<T> QueryToSummary<T>(IQueryable<TQuery> query, TQueryDto queryDto, Expression<Func<IGrouping<int, TQuery>, T>> summaryExpression)
        {
            query = query.AsNoTracking();
            query = Where(query, queryDto);
            return await query.GroupBy(e => 1).Select(summaryExpression).SingleOrDefaultAsync();
        }
        /// <summary>
        /// [异步] 获取分页列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="queryDto"></param>
        /// <param name="selectExp"></param>
        /// <returns></returns>
        protected async Task<PageListDto<T>> QueryToPageListAsync<T>(
            IQueryable<TQuery> query, TQueryDto queryDto, Expression<Func<TQuery, T>> selectExp = null)
        {
            return new PageListDto<T>
            {
                List = await this.QueryToListAsync<T>(query, queryDto, selectExp),
                Total = await this.QueryToTotalAsync(query, queryDto)
            };
        }
        /// <summary>
        /// [异步] 获取单个记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="expression"></param>
        /// <param name="selectExp">自定义映射表达式</param>
        /// <returns></returns>
        protected async Task<T> QueryToSingleAsync<T>(IQueryable<TQuery> query,
            Expression<Func<TQuery, bool>> expression, Expression<Func<TQuery, T>> selectExp = null)
        {
            var whereQuery = query.Where(expression);
            if (typeof(T) == typeof(TQuery))
            {
                return await (whereQuery as IQueryable<T>).FirstOrDefaultAsync();

            }
            IQueryable<T> selectQuery = null;
            if (selectExp == null)
            {
                selectQuery = Select<T>(whereQuery);
            }
            else
            {
                selectQuery = whereQuery.Select(selectExp);
            }
            return await selectQuery.FirstOrDefaultAsync();
        }

        /// <summary>
        /// [异步] 检查对象是否存在
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        protected async Task<bool> CheckExistAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> expression)
        {
            return await query.Where(expression).AnyAsync();
        }

        /// <summary>
        /// 将表达式里面的Where查询表达式组装进一个List对象里
        /// </summary>
        /// <param name="whereExpressions"></param>
        /// <param name="expression"></param>
        private void ExpressionToWhereExpressions(List<Expression<Func<TQuery, bool>>> whereExpressions, Expression expression)
        {
            var methodExpression = expression as MethodCallExpression;
            if (methodExpression == null)
            {
                return;
            }
            if (methodExpression.Method.Name == "Where")
            {
                var e = (methodExpression.Arguments[1] as UnaryExpression)?.Operand as Expression<Func<TQuery, bool>>;
                if (e != null)
                {
                    whereExpressions.Add(e);
                }
            }
            ExpressionToWhereExpressions(whereExpressions, methodExpression.Arguments.FirstOrDefault());
        }

        /// <summary>
        /// 根据字段构建lambda表达式
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <typeparam name="TField"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        protected Expression<Func<TObject, TField>> GetExpressionByField<TObject, TField>(string field)
        {
            if (string.IsNullOrEmpty(field))
            {
                return null;
            }
            string key = typeof(TObject).FullName + "_" + typeof(TField).FullName + "_" + field;
            Expression<Func<TObject, TField>> exp = null;
            if (_expressionCache.TryGetValue(key, out Expression expression))
            {
                return expression as Expression<Func<TObject, TField>>;
            }
            var propInfo = typeof(TObject).GetProperties().FirstOrDefault(p => string.Equals(p.Name, field, StringComparison.OrdinalIgnoreCase));
            if (propInfo != null)
            {
                var paramExp = Expression.Parameter(typeof(TObject));

                Expression body = Expression.PropertyOrField(paramExp, propInfo.Name);
                if (propInfo.PropertyType != typeof(TField))
                {
                    body = Expression.Convert(body, typeof(TField));
                }
                exp = Expression.Lambda<Func<TObject, TField>>(body, paramExp);
            }
            _expressionCache.Add(key, exp);
            return exp;
        }

        #endregion


        #region 服务公开方法

        /// <summary>
        /// [可重写][异步] 获取列表数据
        /// </summary>
        /// <param name="queryDto">查询参数对象</param>
        /// <returns></returns>
        public virtual async Task<List<TDto>> GetListAsync(TQueryDto queryDto)
        {
            queryDto.PageIndex = 0;
            queryDto.PageSize = 0;
            return await QueryToListAsync<TDto>(DbQuery, queryDto, this.QueryToDtoMapExpression);
        }
        /// <summary>
        /// [异步] 获取定列表数据，主要是给其它服务类使用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync<T>(TQueryDto queryDto, Expression<Func<TQuery, T>> selectExp = null)
        {
            queryDto.PageIndex = 0;
            queryDto.PageSize = 0;
            return await QueryToListAsync<T>(DbQuery, queryDto, selectExp);
        }

        /// <summary>
        /// [可重写][异步] 获取列表数据
        /// </summary>
        /// <returns></returns>
        public virtual List<TDto> GetList(TQueryDto queryDto)
        {
            return GetListAsync(queryDto).Result;
        }
        /// <summary>
        /// [可重写][异步] 获取分页列表数据
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public virtual async Task<PageListDto<TDto>> GetPageListAsync(TQueryDto queryDto)
        {
            return await QueryToPageListAsync<TDto>(DbQuery, queryDto, this.QueryToDtoMapExpression);
        }
        /// <summary>
        /// [异步] 获取分页列表数据，可自定义映射的对象类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<PageListDto<T>> GetPageListAsync<T>(TQueryDto queryDto, Expression<Func<TQuery, T>> selectExp = null)
        {
            return await QueryToPageListAsync<T>(DbQuery, queryDto, selectExp);
        }
        /// <summary>
        /// [可重写] 获取分页列表数据
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public virtual PageListDto<TDto> GetPageList(TQueryDto queryDto)
        {
            return GetPageListAsync(queryDto).Result;
        }

        /// <summary>
        /// [可重写][异步] 获取分页数据并带汇总信息
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public virtual async Task<PageListSummaryDto<TDto>> GetPageListWithSummaryAsync(TQueryDto queryDto)
        {
            if (SummaryExpression == null)
            {
                throw new Exception("服务类SummaryExpression属性为Null，请重写SummaryExpression属性值！");
            }
            var query = DbQuery;
            var list = await QueryToListAsync<TDto>(query, queryDto, this.QueryToDtoMapExpression);
            var total = await QueryToTotalAsync(query, queryDto);
            var summary = await QueryToSummary(query, queryDto, SummaryExpression);
            return new PageListSummaryDto<TDto>
            {
                List = list,
                Total = total,
                Summary = summary
            };
        }

        /// <summary>
        /// [可重写] 获取分页数据并带汇总信息
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public virtual PageListSummaryDto<TDto> GetPageListWithSummary(TQueryDto queryDto)
        {
            return GetPageListWithSummaryAsync(queryDto).Result;
        }
        #endregion
    }

    /// <summary>
    /// 查询服务类（带Id值）
    /// </summary>
    /// <typeparam name="TDto">显示Dto类型</typeparam>
    /// <typeparam name="TDetailsDto">详情Dto类型</typeparam>
    /// <typeparam name="TQueryDto">查询参数Dto类型</typeparam>
    /// <typeparam name="TQuery">查询对象类型</typeparam>
    /// <typeparam name="TId">查询对象Id类型</typeparam>
    public abstract class QueryService<TDto, TQueryDto, TQuery, TId> : QueryService<TDto, TQueryDto, TQuery>
        where TQuery : class
        where TQueryDto : QueryDto
    {

        /// <summary>
        /// 查询实体Id值表达式
        /// </summary>
        protected Expression<Func<TQuery, TId>> IdFieldExpression { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="appDbContext"></param>
        /// <param name="mapper"></param>
        /// <param name="queryFilters"></param>
        public QueryService(AppDbContext appDbContext, IMapper mapper, IEnumerable<IQueryFilter<TQuery>> queryFilters) : base(appDbContext, mapper, queryFilters)
        {
            IdFieldExpression = GetExpressionByField<TQuery, TId>("Id");
            OrderDefaultField = GetExpressionByField<TQuery, object>("Id");
        }

        /// <summary>
        /// [异步] 根据传入的查询对象及Id值获取单个数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TId"></typeparam>
        /// <param name="query"></param>
        /// <param name="id"></param>
        /// <param name="selectExp">自定义映射表达式</param>
        /// <returns></returns>
        protected async Task<T> QueryToSingleByIdAsync<T>(IQueryable<TQuery> query, TId id, Expression<Func<TQuery, T>> selectExp = null)
        {
            var idFieldExp = this.IdFieldExpression;
            var constant = Expression.Constant(id, id.GetType());
            var body = Expression.Equal(idFieldExp.Body, constant);
            var exp = Expression.Lambda<Func<TQuery, bool>>(body, idFieldExp.Parameters);
            return await QueryToSingleAsync<T>(query, exp, selectExp);
        }
        /// <summary>
        /// [可重写][异步] 根据Id获取单个对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async virtual Task<TDto> GetByIdAsync(TId id)
        {
            return await QueryToSingleByIdAsync<TDto>(DbQuery, id, this.QueryToDtoMapExpression);
        }

        /// <summary>
        /// [可重写] 根据Id获取单个对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TDto GetById(TId id)
        {
            return GetByIdAsync(id).Result;
        }

        /// <summary>
        /// [异步] 根据Id获取单个对象，可自定义映射的对象类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> GetByIdAsync<T>(TId id, Expression<Func<TQuery, T>> selectExp = null)
        {
            return await QueryToSingleByIdAsync<T>(DbQuery, id, selectExp);
        }
    }
}
