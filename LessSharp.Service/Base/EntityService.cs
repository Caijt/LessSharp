using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Microsoft.EntityFrameworkCore.Internal;
using LessSharp.Dto;
using LessSharp.Data;
using LessSharp.Entity;

namespace LessSharp.Service
{
    /// <summary>
    /// 实体操作服务类
    /// </summary>
    /// <typeparam name="TCreateDto">创建Dto类型</typeparam>
    /// <typeparam name="TUpdateDto">更新Dto类型</typeparam>
    /// <typeparam name="TDto">显示Dto类型</typeparam>
    /// <typeparam name="TDetailsDto">详情Dto类型</typeparam>
    /// <typeparam name="TQueryDto">查询参数Dto类型</typeparam>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TId">实体Id类型</typeparam>
    public abstract class EntityService<TCreateDto, TUpdateDto, TDto, TQueryDto, TEntity, TId> : QueryService<TDto, TQueryDto, TEntity, TId>
    where TEntity : class, IEntity, new()
    where TQueryDto : QueryDto
    where TCreateDto : class
    where TUpdateDto : class
    {

        /// <summary>
        /// 实体查询缓存类
        /// </summary>
        private DbSet<TEntity> _dbSet;

        /// <summary>
        /// 实体操作类
        /// </summary>
        protected DbSet<TEntity> DbSet
        {
            get
            {
                if (_dbSet == null)
                {
                    _dbSet = DbContext.Set<TEntity>();
                }
                return _dbSet;
            }
            set
            {
                _dbSet = value;
            }
        }

        /// <summary>
        /// 实体处理器列表
        /// </summary>
        protected List<IEntityHandler<TEntity>> EntityHandlers { get; set; } = new List<IEntityHandler<TEntity>>();

        /// <summary>
        /// 是否自动进行保存
        /// </summary>
        public bool AutoSaveChanges { get; set; } = true;
        /// <summary>
        /// 是否在更新时先查出数据库里面的实体出来
        /// </summary>
        protected bool IsFindOldEntity { get; set; } = false;

        /// <summary>
        /// 更新操作中的fields是用来排除字段修改，还是仅修改props里面的字段，默认为true
        /// </summary>
        protected bool DefaultUpdateExcludeField { get; set; } = true;

        /// <summary>
        /// 创建对象映射成实体表达式，赋值时会采用这个映射规则，不赋值会使用autoMapper进行自动映射
        /// </summary>
        protected Expression<Func<TCreateDto, TEntity>> CreateDtoToEntityMapExpression { get; set; }

        /// <summary>
        /// 更新对象映射成实体表达式，赋值时会采用这个映射规则，不赋值会使用autoMapper进行自动映射
        /// </summary>
        protected Expression<Func<TUpdateDto, TEntity>> UpdateDtoToEntityMapExpression { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="appDbContext"></param>
        /// <param name="mapper"></param>
        /// <param name="entityHandlers"></param>
        /// <param name="queryFilters"></param>
        public EntityService(
            AppDbContext appDbContext,
            IMapper mapper,
            IEnumerable<IEntityHandler<TEntity>> entityHandlers,
            IEnumerable<IQueryFilter<TEntity>> queryFilters
            ) : base(appDbContext, mapper, queryFilters)
        {
            DbQuery = DbSet.AsQueryable();
            if (entityHandlers != null)
            {
                this.EntityHandlers.AddRange(entityHandlers);
            }
        }

        #region 服务内部使用方法

        /// <summary>
        /// [异步] 创建或更新实体，会经过实体处理器及事件
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="forceCreateOrUpdate">强制创建或更新，null为自动判断，false为强制创建，true为强制更新</param>
        /// <param name="mapExpression"></param>
        /// <returns></returns>
        private async Task<TId> CreateOrUpdateAsync<T>(T dto, bool? forceCreateOrUpdate = null, Expression<Func<T, TEntity>> mapExpression = null)
        {
            TEntity newEntity;
            if (mapExpression == null)
            {
                newEntity = Mapper.Map<TEntity>(dto);
            }
            else
            {
                newEntity = mapExpression.Compile().Invoke(dto);
            }
            TEntity oldEntity = null;
            bool isUpdate = false;
            if (forceCreateOrUpdate.HasValue)
            {
                isUpdate = forceCreateOrUpdate.Value;
            }
            else
            {
                isUpdate = DbContext.Entry(newEntity).IsKeySet;
            }
            if (IsFindOldEntity && isUpdate)
            {
                var id = this.IdFieldExpression.Compile().Invoke(newEntity);
                if (!id.Equals(default(TId)))
                {
                    oldEntity = QueryToSingleByIdAsync<TEntity>(DbQuery, id).Result;
                }
            }
            if (forceCreateOrUpdate == null)
            {
                foreach (var handler in this.EntityHandlers)
                {
                    handler.Saving(newEntity);
                }
                OnSaving(newEntity, oldEntity, dto as TUpdateDto);
            }

            if (!isUpdate)
            {
                //创建实体
                var createDto = dto as TCreateDto;
                foreach (var handler in this.EntityHandlers)
                {
                    handler.Creating(newEntity);
                }
                OnCreating(newEntity, createDto);
                await CreateEntityAsync(newEntity);
                OnCreated(newEntity, createDto);
            }
            else
            {
                //更新实体
                var updateDto = dto as TUpdateDto;
                var excludeProps = new List<Expression<Func<TEntity, object>>>();
                foreach (var handler in this.EntityHandlers)
                {
                    handler.Updating(newEntity, excludeProps, this.DefaultUpdateExcludeField);
                }
                OnUpdating(newEntity, oldEntity, updateDto, excludeProps, this.DefaultUpdateExcludeField);
                if (oldEntity != null)
                {
                    DbContext.Entry(oldEntity).CurrentValues.SetValues(newEntity);
                    newEntity = oldEntity;
                }
                await UpdateEntityAsync(newEntity, excludeProps, this.DefaultUpdateExcludeField);
                OnUpdated(newEntity, updateDto);
            }
            OnSaved(newEntity, dto as TUpdateDto);
            if (IdFieldExpression == null)
            {
                throw new Exception("请在构造函数中指定IdField属性值！");
            }
            return IdFieldExpression.Compile().Invoke(newEntity);
        }

        /// <summary>
        /// [异步] 根据传输对象创建实体，会经过实体处理器及事件
        /// </summary>
        /// <param name="createDto"></param>
        /// <param name="mapExpression">映射表达式</param>
        /// <returns></returns>
        protected async Task<TId> CreateAsync(TCreateDto createDto, Expression<Func<TCreateDto, TEntity>> mapExpression)
        {
            return await CreateOrUpdateAsync(createDto, false, mapExpression);
        }

        /// <summary>
        /// [异步] 根据传输对象更新实体，会经过实体处理器及事件
        /// </summary>
        /// <param name="updateDto"></param>
        /// <param name="mapExpression"></param>
        /// <returns></returns>
        protected async Task<TId> UpdateAsync(TUpdateDto updateDto, Expression<Func<TUpdateDto, TEntity>> mapExpression)
        {
            return await CreateOrUpdateAsync(updateDto, true, mapExpression);
        }

        /// <summary>
        /// [异步] 根据传输对象保存实体，会经过实体处理器及事件
        /// </summary>
        /// <param name="updateDto"></param>
        /// <param name="mapExpression">映射表达式</param>
        /// <returns></returns>
        protected async Task<TId> SaveAsync(TUpdateDto updateDto, Expression<Func<TUpdateDto, TEntity>> mapExpression)
        {
            if (typeof(TCreateDto) != typeof(TUpdateDto))
            {
                throw new Exception("创建Dto跟更新Dto不是同一类型的实体，无法使用Save");
            }
            return await CreateOrUpdateAsync(updateDto, null, mapExpression);
        }

        /// <summary>
        /// [异步] 删除实体，会经过实体处理器及事件
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected async Task<int> DeleteAsync(TEntity entity)
        {
            foreach (var handler in this.EntityHandlers)
            {
                handler.Deleting(entity);
            }
            OnDeleting(entity);
            if (!DbContext.Entry(entity).IsKeySet)
            {
                return await Task.FromResult(0);
            }
            int res = await DeleteEntityAsync(entity);
            OnDeleted(entity);
            return res;
        }

        /// <summary>
        /// [异步] 根据Id值删除实体，会经过实体处理器及事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">Id值</param>
        /// <returns></returns>
        protected async Task<int> DeleteEntityByIdAsync(TId id)
        {
            TEntity entity;
            if (IsFindOldEntity)
            {
                entity = QueryToSingleByIdAsync<TEntity>(DbQuery, id).Result;
                if (entity == null)
                {
                    return 0;
                }
            }
            else
            {
                entity = new TEntity();
                var idFieldExp = this.IdFieldExpression;
                var constant = Expression.Constant(id, id.GetType());
                var body = Expression.Assign(idFieldExp.Body, constant);
                var exp = Expression.Lambda<Action<TEntity>>(body, idFieldExp.Parameters);
                exp.Compile()(entity);
            }
            return await this.DeleteAsync(entity);
        }

        /// <summary>
        /// [异步] 根据Id数组批量删除实体
        /// </summary>
        /// <param name="ids"></param>
        protected async Task<int> DeleteEntitiesByIdsAsync(TId[] ids)
        {
            var temp = this.AutoSaveChanges;
            this.AutoSaveChanges = false;
            foreach (var id in ids)
            {
                await this.DeleteByIdAsync(id);
            }
            this.AutoSaveChanges = temp;
            if (this.AutoSaveChanges)
            {
                return await DbContext.SaveChangesAsync();
            }
            return 0;
        }

        /// <summary>
        /// [异步] 软删除，会经过实体处理器及事件
        /// </summary>
        /// <param name="entity"></param>
        protected async Task<int> SoftDeleteAsync(TEntity entity)
        {
            var includeProps = new List<Expression<Func<TEntity, object>>>();
            foreach (var handler in this.EntityHandlers)
            {
                handler.SoftDeleting(entity, includeProps);
            }
            OnSoftDeleting(entity, includeProps);
            if (DbContext.Entry(entity).State == EntityState.Detached)
            {
                return await UpdateEntityAsync(entity, includeProps, false);
            }
            int res = await UpdateEntityAsync(entity);
            OnSoftDeleted(entity);
            return res;
        }

        /// <summary>
        /// [异步] 根据Id值软删除实体，会经过实体处理器及事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        protected async Task<int> SoftDeleteEntityByIdAsync(TId id)
        {
            TEntity entity;
            if (IsFindOldEntity)
            {
                entity = QueryToSingleByIdAsync<TEntity>(DbQuery, id).Result;
            }
            else
            {
                entity = new TEntity();
                var idFieldExp = this.IdFieldExpression;
                var constant = Expression.Constant(id, id.GetType());
                var body = Expression.Assign(idFieldExp.Body, constant);
                var exp = Expression.Lambda<Action<TEntity>>(body, idFieldExp.Parameters);
                exp.Compile()(entity);
            }
            return await SoftDeleteAsync(entity);
        }

        /// <summary>
        /// [异步] 根据Id数组批量软删除实体
        /// </summary>
        /// <param name="ids"></param>
        protected async Task<int> SoftDeleteEntitiesByIdsAsync(TId[] ids)
        {
            var temp = this.AutoSaveChanges;
            this.AutoSaveChanges = false;
            foreach (var id in ids)
            {
                await this.SoftDeleteEntityByIdAsync(id);
            }
            this.AutoSaveChanges = temp;
            if (this.AutoSaveChanges)
            {
                return await DbContext.SaveChangesAsync();
            }
            return 0;

        }

        /// <summary>
        /// [异步] 直接创建实体，不会经过实体处理器及事件
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected async Task<int> CreateEntityAsync<T>(T entity)
        {
            DbContext.Add(entity);
            if (AutoSaveChanges)
            {
                return await DbContext.SaveChangesAsync();
            }
            return 0;
        }

        /// <summary>
        /// [异步] 直接更新实体，不会经过实体处理器及事件，不会更新关联实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="excludeOrIncludeFields">排除或只包含更新的字段数组</param>
        /// <param name="excludeField">true代表为排除字段，false代表为只更新字段</param>
        /// <returns></returns>
        protected async Task<int> UpdateEntityAsync<T>(T entity,
            List<Expression<Func<T, object>>> excludeOrIncludeFields = null, bool excludeField = true) where T : class
        {
            var entry = DbContext.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                DbContext.Entry(entity).State = EntityState.Modified;
            }
            if (excludeField)
            {
                if (excludeOrIncludeFields != null && excludeOrIncludeFields.Count > 0)
                {
                    foreach (var field in excludeOrIncludeFields)
                    {
                        DbContext.Entry<T>(entity).Property(field).IsModified = false;
                    }
                }
            }
            else
            {
                foreach (var prop in DbContext.Entry(entity).Properties)
                {
                    if (excludeOrIncludeFields == null || !excludeOrIncludeFields.Any(e => DbContext.Entry(entity).Property(e).Metadata.Name == prop.Metadata.Name))
                    {
                        prop.IsModified = false;
                    }
                }
            }
            if (AutoSaveChanges)
            {
                return await DbContext.SaveChangesAsync();
            }
            return 0;
        }

        /// <summary>
        /// [异步] 直接删除实体，不会经过实体处理器及事件
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected async Task<int> DeleteEntityAsync<T>(T entity)
        {
            DbContext.Remove(entity);
            if (AutoSaveChanges)
            {
                return await DbContext.SaveChangesAsync();
            }
            return 0;
        }

        /// <summary>
        /// 保存实体数组，传入一个新数组数据跟一个数据库的旧数组数据，把数据库的数据同步为新数据，需在调用后手动进行SaveChange
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="newEntities">新数组数据</param>
        /// <param name="oldEntities">旧数组数据</param>
        /// <param name="entityKey">实体主键表达式</param>
        /// <param name="afterUpdate">更新后实体后事件</param>
        /// <param name="afterDelete">删除实体后事件</param>
        protected void SaveEntities<T, TKey>(IEnumerable<T> newEntities,
             IEnumerable<T> oldEntities,
             Expression<Func<T, TKey>> entityKey,
             Action<T> afterUpdate = null,
             Action<T> afterDelete = null)
        {
            Func<T, T, bool> condition = (newEntity, oldEntity) =>
            {
                return entityKey.Compile().Invoke(newEntity).Equals(entityKey.Compile().Invoke(oldEntity));
            };
            SaveEntities<T>(newEntities, oldEntities, condition, afterUpdate, afterDelete);
        }

        /// <summary>
        /// 保存实体数组，传入一个新数组数据跟一个数据库的旧数组数据，把数据库的数据同步为新数据，需在调用后手动进行SaveChange
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newEntities">新数组数据</param>
        /// <param name="oldEntities">旧数组数据</param>
        /// <param name="condition">判断条件，第一个是新对象，第二个是旧对象</param>
        /// <param name="afterUpdate">更新后实体后事件</param>
        /// <param name="afterDelete">删除实体后事件</param>
        protected void SaveEntities<T>(IEnumerable<T> newEntities,
             IEnumerable<T> oldEntities,
             Func<T, T, bool> condition,
             Action<T> afterUpdate = null,
             Action<T> afterDelete = null)
        {
            if (oldEntities != null)
            {
                foreach (var oldEntity in oldEntities)
                {
                    //var relatedKey = relatedEntityKey.Compile().Invoke(relatedEntity);
                    //var constant = Expression.Constant(relatedKey);
                    //var body = Expression.Equal(relatedEntityKey.Body, constant);
                    //var exp = Expression.Lambda<Func<TRelatedEntity, bool>>(body, relatedEntityKey.Parameters);
                    if (newEntities == null || !newEntities.Any(e =>
                    {
                        return condition(e, oldEntity);
                    }))
                    {
                        DbContext.Remove(oldEntity);
                        afterDelete?.Invoke(oldEntity);
                    }
                }
            }
            if (newEntities != null)
            {
                foreach (var newEntity in newEntities)
                {
                    T oldEntity = default(T);
                    if (oldEntities != null && oldEntities.Count() > 0)
                    {
                        //var relatedKey = relatedEntityKey.Compile().Invoke(relatedEntity);
                        //var constant = Expression.Constant(relatedKey);
                        //var body = Expression.Equal(relatedEntityKey.Body, constant);
                        //var exp = Expression.Lambda<Func<TRelatedEntity, bool>>(body, relatedEntityKey.Parameters);
                        oldEntity = oldEntities.FirstOrDefault(e =>
                        {
                            return condition(newEntity, e);
                        });
                    }
                    if (oldEntity == null)
                    {
                        DbContext.Add(newEntity);
                    }
                    else
                    {
                        DbContext.Entry(oldEntity).CurrentValues.SetValues(newEntity);
                        if (afterUpdate != null)
                        {
                            afterUpdate(oldEntity);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// [可重写] 实体创建前处理事件
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="createDto"></param>
        protected virtual void OnCreating(TEntity entity, TCreateDto createDto) { }

        /// <summary>
        /// [可重写] 实体创建后处理事件
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="createDto"></param>
        protected virtual void OnCreated(TEntity entity, TCreateDto createDto) { }


        /// <summary>
        /// [可重写] 实体更新前处理事件
        /// </summary>
        /// <param name="newEntity">新实体，将要更新的数据</param>
        /// <param name="oldEntity">旧实体，在数据库的数据</param>
        /// <param name="updateDto"></param>
        /// <param name="excludeOrIncludeFields"></param>
        /// <param name="isExcludeField"></param>
        protected virtual void OnUpdating(TEntity newEntity, TEntity oldEntity, TUpdateDto updateDto, List<Expression<Func<TEntity, object>>> excludeOrIncludeFields, bool isExcludeField) { }

        /// <summary>
        /// [可重写] 实体更新后处理事件
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="updateDto"></param>
        protected virtual void OnUpdated(TEntity entity, TUpdateDto updateDto) { }

        /// <summary>
        /// [可重写] 保存实体前的处理事件
        /// </summary>
        /// <param name="newEntity">新实体，将要更新的数据</param>
        /// <param name="oldEntity">旧实体，数据库的数据</param>
        /// <param name="saveDto"></param>
        protected virtual void OnSaving(TEntity newEntity, TEntity oldEntity, TUpdateDto saveDto) { }

        /// <summary>
        /// [可重写] 保存实体后的处理事件
        /// </summary>
        protected virtual void OnSaved(TEntity entity, TUpdateDto saveDto)
        {
        }

        /// <summary>
        /// [可重写] 实体删除前处理事件
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual void OnDeleting(TEntity entity) { }

        /// <summary>
        /// [可重写] 实体删除后处理事件
        /// </summary>
        /// <param name="entity"></param>
        protected virtual void OnDeleted(TEntity entity) { }

        /// <summary>
        /// [可重写] 实体软删除前处理事件
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="includeProps"></param>
        protected virtual void OnSoftDeleting(TEntity entity, List<Expression<Func<TEntity, object>>> includeProps) { }

        /// <summary>
        /// [可重写] 实体软删除后处理事件
        /// </summary>
        /// <param name="entity"></param>
        protected virtual void OnSoftDeleted(TEntity entity) { }

        #endregion

        #region 服务提供方法
        /// <summary>
        /// [异步][可重写] 创建实体
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        public virtual async Task<TId> CreateAsync(TCreateDto createDto)
        {
            return await CreateAsync(createDto, this.CreateDtoToEntityMapExpression);
        }

        /// <summary>
        /// [可重写] 创建实体
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        public virtual TId Create(TCreateDto createDto)
        {
            return CreateAsync(createDto).Result;
        }

        /// <summary>
        /// [异步][可重写] 更新实体
        /// </summary>
        /// <param name="updateDto"></param>
        /// <returns></returns>
        public virtual async Task<TId> UpdateAsync(TUpdateDto updateDto)
        {
            return await UpdateAsync(updateDto, this.UpdateDtoToEntityMapExpression);
        }

        /// <summary>
        /// [可重写] 更新实体
        /// </summary>
        /// <param name="updateDto"></param>
        /// <returns></returns>
        public virtual TId Update(TUpdateDto updateDto)
        {
            return UpdateAsync(updateDto).Result;
        }

        /// <summary>
        /// [异步][可重写] 保存实体
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns>返回Id值</returns>
        public virtual async Task<TId> SaveAsync(TUpdateDto saveDto)
        {

            return await SaveAsync(saveDto, this.UpdateDtoToEntityMapExpression);
        }

        /// <summary>
        /// [可重写] 保存实体
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public virtual TId Save(TUpdateDto saveDto)
        {
            return SaveAsync(saveDto).Result;
        }

        /// <summary>
        /// [异步][可重写] 根据Id删除一个实体
        /// </summary>
        /// <param name="id"></param>
        public virtual async Task<int> DeleteByIdAsync(TId id)
        {
            return await DeleteEntityByIdAsync(id);
        }

        /// <summary>
        /// [可重写] 根据Id删除一个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual int DeleteById(TId id)
        {
            return DeleteByIdAsync(id).Result;
        }

        /// <summary>
        /// [异步][可重写] 根据Id数组批量删除实体
        /// </summary>
        /// <param name="ids"></param>
        public virtual async Task<int> DeleteByIdsAsync(TId[] ids)
        {
            return await this.DeleteEntitiesByIdsAsync(ids);
        }

        /// <summary>
        /// [可重写] 根据Id数组批量删除实体
        /// </summary>
        /// <param name="ids"></param>
        public virtual int DeleteByIds(TId[] ids)
        {
            return DeleteByIdsAsync(ids).Result;
        }

        /// <summary>
        /// [异步][可重写] 根据Id软删除一个实体
        /// </summary>
        /// <param name="id"></param>
        public virtual async Task<int> SoftDeleteByIdAsync(TId id)
        {
            return await SoftDeleteEntityByIdAsync(id);
        }

        /// <summary>
        /// [可重写] 根据Id软删除一个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual int SoftDeleteById(TId id)
        {
            return SoftDeleteByIdAsync(id).Result;
        }

        /// <summary>
        /// [异步][可重写] 根据Id数组批量软删除实体
        /// </summary>
        /// <param name="ids"></param>
        public virtual async Task<int> SoftDeleteByIdsAsync(TId[] ids)
        {
            return await this.SoftDeleteEntitiesByIdsAsync(ids);
        }

        /// <summary>
        /// [可重写] 根据Id数组批量软删除实体
        /// </summary>
        /// <param name="ids"></param>
        public virtual int SoftDeleteByIds(TId[] ids)
        {
            return SoftDeleteEntitiesByIdsAsync(ids).Result;
        }
        #endregion
    }
}
