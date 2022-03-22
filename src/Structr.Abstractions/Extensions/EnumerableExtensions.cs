using Structr.Abstractions.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Structr.Abstractions.Extensions
{
    public static class EnumerableExtensions
    {
        public static IOrderedEnumerable<T> OrderBy<T>(this IEnumerable<T> source, IReadOnlyDictionary<string, Order> sort)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(sort, nameof(sort));

            if (sort.Count == 0)
                return (IOrderedEnumerable<T>)source;

            foreach (string propertyName in sort.Keys)
            {
                if (string.IsNullOrWhiteSpace(propertyName))
                    throw new InvalidOperationException("Invalid property name to sort");

                source = propertyName == sort.Keys.First()
                    ? source.OrderBy(propertyName, sort[propertyName])
                    : source.ThenBy(propertyName, sort[propertyName]);
            }

            return (IOrderedEnumerable<T>)source;
        }

        public static IOrderedEnumerable<T> OrderBy<T>(this IEnumerable<T> source, string propertyName, Order order)
        {
            return Order(source, propertyName, order == Abstractions.Order.Asc ? EOrderMethod.OrderBy : EOrderMethod.OrderByDescending);
        }

        public static IOrderedEnumerable<T> OrderBy<T>(this IEnumerable<T> source, string propertyName)
        {
            return Order(source, propertyName, EOrderMethod.OrderBy);
        }

        public static IOrderedEnumerable<T> OrderByDescending<T>(this IEnumerable<T> source, string propertyName)
        {
            return Order(source, propertyName, EOrderMethod.OrderByDescending);
        }

        public static IOrderedEnumerable<T> ThenBy<T>(this IEnumerable<T> source, string propertyName, Order order)
        {
            return Order(source, propertyName, order == Abstractions.Order.Asc ? EOrderMethod.ThenBy : EOrderMethod.ThenByDescending);
        }

        public static IOrderedEnumerable<T> ThenBy<T>(this IOrderedEnumerable<T> source, string propertyName)
        {
            return Order(source, propertyName, EOrderMethod.ThenBy);
        }

        public static IOrderedEnumerable<T> ThenByDescending<T>(this IOrderedEnumerable<T> source, string propertyName)
        {
            return Order(source, propertyName, EOrderMethod.ThenByDescending);
        }

        private static IOrderedEnumerable<T> Order<T>(IEnumerable<T> source, string propertyName, EOrderMethod method)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotEmpty(propertyName, nameof(propertyName));

            string[] propertyNameParts = propertyName.Split('.');
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (string propertyNamePart in propertyNameParts)
            {
                // Use reflection (not ComponentModel) to mirror LINQ
                PropertyInfo propertyInfo = type.GetProperty(propertyNamePart);
                if (propertyInfo == null)
                    throw new InvalidOperationException($"Nested property with name {propertyName} for type {typeof(T).Name} was not found.");
                expr = Expression.Property(expr, propertyInfo);
                type = propertyInfo.PropertyType;
            }
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            object result = typeof(Enumerable).GetMethods().Single(
                    m => m.Name == method.ToString()
                            && m.IsGenericMethodDefinition
                            && m.GetGenericArguments().Length == 2
                            && m.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), type)
                    .Invoke(null, new object[] { source, lambda.Compile() });
            return (IOrderedEnumerable<T>)result;
        }

        public static T PickRandom<T>(this IEnumerable<T> source)
        {
            Ensure.NotNull(source, nameof(source));

            return source.PickRandom(1).Single();
        }

        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
        {
            Ensure.NotNull(source, nameof(source));

            return source.Shuffle().Take(count);
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            Ensure.NotNull(source, nameof(source));

            return source.OrderBy(x => Guid.NewGuid());
        }
    }
}
