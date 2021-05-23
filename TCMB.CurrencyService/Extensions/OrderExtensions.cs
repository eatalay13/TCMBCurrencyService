using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using TCMB.CurrencyService.Dtos;

namespace TCMB.CurrencyService.Extensions
{
    public static class OrderExtensions
    {
        public static IOrderedQueryable<T> DynamicOrder<T>(this IQueryable<T> source, string property, OrderDirection orderDirection)
        {
            Type type = typeof(T);

            if (string.IsNullOrWhiteSpace(property))
                property = type.GetProperties().First(c => c.CanRead && c.CanWrite).Name;

            string[] props = property.Split('.');
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;

            foreach (string prop in props)
            {
                PropertyInfo pi = type.GetProperty(prop);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }

            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            var methodName = orderDirection == OrderDirection.DESC ? "OrderByDescending" : "OrderBy";

            object result =
                typeof(Queryable).GetMethods().Single(
                    method =>
                    method.Name == methodName && method.IsGenericMethodDefinition &&
                    method.GetGenericArguments().Length == 2 && method.GetParameters().Length == 2).MakeGenericMethod(
                        typeof(T), type).Invoke(null, new object[] { source, lambda });

            return (IOrderedQueryable<T>)result;
        }
    }
}
