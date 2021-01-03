using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace LessSharp.Service
{
    public abstract class EntityHandler<TEntity, TFieldEntity> : IEntityHandler<TEntity> where TFieldEntity : class
    {
        private bool IsEnabled
        {
            get
            {
                return typeof(TFieldEntity).IsAssignableFrom(typeof(TEntity));
            }
        }
        public void Creating(TEntity entity)
        {
            if (IsEnabled)
            {
                OnCreating(entity as TFieldEntity);
            }
        }
        protected virtual void OnCreating(TFieldEntity entity) { }
        public void Updating(TEntity entity, List<Expression<Func<TEntity, object>>> excludeFields, bool excludeField)
        {
            if (IsEnabled)
            {
                List<Expression<Func<TFieldEntity, object>>> expressions = new List<Expression<Func<TFieldEntity, object>>>();
                OnUpdating(entity as TFieldEntity, expressions, excludeField);
                expressions.ForEach(e =>
                {
                    excludeFields.Add(Expression.Lambda<Func<TEntity, object>>(e.Body, e.Parameters));
                });
            }
        }
        public virtual void OnUpdating(TFieldEntity entity, List<Expression<Func<TFieldEntity, object>>> excludeProps, bool excludeField) { }

        public void Saving(TEntity entity)
        {
            if (IsEnabled)
            {
                OnSaving(entity as TFieldEntity);
            }
        }
        public virtual void OnSaving(TFieldEntity entity) { }
        public void Deleting(TEntity entity)
        {
            if (IsEnabled)
            {
                OnDeleting(entity as TFieldEntity);
            }
        }
        public virtual void OnDeleting(TFieldEntity entity) { }


        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="includeProps"></param>
        public void SoftDeleting(TEntity entity, List<Expression<Func<TEntity, object>>> includeProps)
        {
            if (IsEnabled)
            {
                List<Expression<Func<TFieldEntity, object>>> expressions = new List<Expression<Func<TFieldEntity, object>>>();
                OnSoftDeleting(entity as TFieldEntity, expressions);
                expressions.ForEach(e =>
                {
                    includeProps.Add(Expression.Lambda<Func<TEntity, object>>(e.Body, e.Parameters));
                });
            }
        }
        public virtual void OnSoftDeleting(TFieldEntity entity, List<Expression<Func<TFieldEntity, object>>> includeProps)
        {

        }
    }
}
