using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Medit.Framework.Mappers
{
    /// <summary>
    /// 实体映射器
    /// </summary>
    /// <typeparam name="TInEntity"></typeparam>
    /// <typeparam name="TOutEntity"></typeparam>
    public static class EntityMapper<TInEntity, TOutEntity>
    {
        private static readonly Func<TInEntity, TOutEntity> ComplieMap = MapFunc();//表达式树的缓存，提高性能
        private static Func<TInEntity, TOutEntity> MapFunc()
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(TInEntity), "p");
            List<MemberBinding> memberBindingList = new List<MemberBinding>();
            foreach (var item in typeof(TOutEntity).GetProperties())
            {
                if (!item.CanWrite)
                    continue;

                PropertyInfo propertyInfo = typeof(TInEntity).GetProperty(item.Name);
                if (propertyInfo != null)
                {
                    MemberExpression property = Expression.Property(parameterExpression, propertyInfo);
                    MemberBinding memberBinding = Expression.Bind(item, property);
                    memberBindingList.Add(memberBinding);
                }
            }

            MemberInitExpression memberInitExpression = Expression.MemberInit(
                Expression.New(typeof(TOutEntity)),
                memberBindingList.ToArray());

            Expression<Func<TInEntity, TOutEntity>> expression = Expression.Lambda<Func<TInEntity, TOutEntity>>(
                memberInitExpression,
                new ParameterExpression[] { parameterExpression });
            return expression.Compile();
        }

        /// <summary>
        /// 实体相互映射，采用表达式树机制
        /// 适用于对象的深拷贝
        /// </summary>
        /// <param name="tInEntity"></param>
        /// <returns></returns>
        public static TOutEntity Map(TInEntity tInEntity)
        {
            return ComplieMap(tInEntity);
        }
    }
}
