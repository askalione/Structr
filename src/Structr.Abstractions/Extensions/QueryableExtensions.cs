using Structr.Abstractions.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Structr.Abstractions.Extensions
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// Pagination query by skip and take variables. 
        /// </summary>
        /// <typeparam name="T">Type of queryable objects.</typeparam>
        /// <param name="query">Query.</param>
        /// <param name="skip">Number on elements wich must be skipped.</param>
        /// <param name="take">Number of elements which must be taken.</param>
        /// <returns>
        /// Paged query.
        /// </returns>
        public static IQueryable<T> PageBy<T>(this IQueryable<T> query, int skip, int take)
        {
            Ensure.NotNull(query, nameof(query));

            if (skip > 0)
                query = query.Skip(skip);
            if (take > 0)
                query = query.Take(take);

            return query;
        }

        /// <summary>
        /// Sort query by dictionary of properties names.
        /// </summary>
        /// <typeparam name="T">Type of queryable objects.</typeparam>
        /// <param name="query">Query.</param>
        /// <param name="sort">Dictionary of properties names and ascending sort flags.</param>
        /// <returns>
        /// Ordered query (IOrderedQueryable).
        /// </returns>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, IReadOnlyDictionary<string, EOrder> sort)
        {
            Ensure.NotNull(query, nameof(query));
            Ensure.NotNull(sort, nameof(sort));

            if (sort.Count == 0)
                return (IOrderedQueryable<T>)query;

            foreach (string propertyName in sort.Keys)
            {
                if (string.IsNullOrWhiteSpace(propertyName))
                    throw new InvalidOperationException("Invalid property name to sort");

                query = propertyName == sort.Keys.First()
                    ? query.OrderBy(propertyName, sort[propertyName])
                    : query.ThenBy(propertyName, sort[propertyName]);
            }

            return (IOrderedQueryable<T>)query;
        }

        /// <summary>
        /// Sort query by specified property name and ascending flag.
        /// </summary>
        /// <typeparam name="T">Type of queryable objects.</typeparam>
        /// <param name="query">Query.</param>
        /// <param name="propertyName">Name of property for sorting.</param>
        /// <param name="ascending">Ascending flag.</param>
        /// <returns>
        /// Ordered query (IOrderedQueryable).
        /// </returns>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string propertyName, EOrder order)
        {
            return Order(query, propertyName, order == EOrder.Asc ? EOrderMethod.OrderBy : EOrderMethod.OrderByDescending);
        }

        /// <summary>
        /// Ascending sort query by specified property name.
        /// </summary>
        /// <typeparam name="T">Type of queryable objects.</typeparam>
        /// <param name="query">Query.</param>
        /// <param name="propertyName">Name of property for sorting.</param>
        /// <returns>
        /// Ordered query (IOrderedQueryable).
        /// </returns>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string propertyName)
        {
            return Order(query, propertyName, EOrderMethod.OrderBy);
        }

        /// <summary>
        /// Descending sort query by specified property name.
        /// </summary>
        /// <typeparam name="T">Type of queryable objects.</typeparam>
        /// <param name="query">Query.</param>
        /// <param name="propertyName">Name of property for sorting.</param>
        /// <returns>
        /// Ordered query (IOrderedQueryable).
        /// </returns>
        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> query, string propertyName)
        {
            return Order(query, propertyName, EOrderMethod.OrderByDescending);
        }

        /// <summary>
        /// Subsequent sort query by specified property name and ascending flag.
        /// </summary>
        /// <typeparam name="T">Type of queryable objects.</typeparam>
        /// <param name="query">Query.</param>
        /// <param name="propertyName">Name of property for sorting.</param>
        /// <param name="ascending">Ascending flag.</param>
        /// <returns>
        /// Ordered query (IOrderedQueryable).
        /// </returns>
        public static IOrderedQueryable<T> ThenBy<T>(this IQueryable<T> query, string propertyName, EOrder order)
        {
            return Order(query, propertyName, order == EOrder.Asc ? EOrderMethod.ThenBy : EOrderMethod.ThenByDescending);
        }

        /// <summary>
        /// Subsequent ascending sort query by specified property name.
        /// </summary>
        /// <typeparam name="T">Type of queryable objects.</typeparam>
        /// <param name="query">Query.</param>
        /// <param name="propertyName">Name of property for sorting.</param>
        /// <returns>
        /// Ordered query (IOrderedQueryable).
        /// </returns>
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> query, string propertyName)
        {
            return Order(query, propertyName, EOrderMethod.ThenBy);
        }

        /// <summary>
        /// Subsequent descending sort query by specified property name.
        /// </summary>
        /// <typeparam name="T">Type of queryable objects.</typeparam>
        /// <param name="query">Query.</param>
        /// <param name="propertyName">Name of property for sorting.</param>
        /// <returns>
        /// Ordered query (IOrderedQueryable).
        /// </returns>
        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> query, string propertyName)
        {
            return Order(query, propertyName, EOrderMethod.ThenByDescending);
        }

        /// <summary>
        /// Apply ordering to sort queryable result.
        /// </summary>
        /// <typeparam name="T">Type of queryable objects.</typeparam>
        /// <param name="query">Query.</param>
        /// <param name="propertyName">Name of property.</param>
        /// <param name="method">Flag of order method.</param>
        /// <returns>
        /// Ordered queryable object (IOrderedQueryable).
        /// </returns>
        /// <remarks>
        /// Use reflection (not ComponentModel) to mirror LINQ.
        /// </remarks>
        private static IOrderedQueryable<T> Order<T>(IQueryable<T> query, string propertyName, EOrderMethod method)
        {
            Ensure.NotNull(query, nameof(query));
            Ensure.NotEmpty(propertyName, nameof(propertyName));

            string[] propertyNameParts = propertyName.Split('.');
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expression = arg;
            foreach (string propertyNamePart in propertyNameParts)
            {
                PropertyInfo propertyInfo = type.GetProperty(propertyNamePart, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo == null)
                    throw new InvalidOperationException($"Nested property with name {propertyName} for type {typeof(T).Name} was not found.");
                expression = Expression.Property(expression, propertyInfo);
                type = propertyInfo.PropertyType;
            }
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expression, arg);

            object result = typeof(Queryable).GetMethods().Single(
                    m => m.Name == method.ToString()
                            && m.IsGenericMethodDefinition
                            && m.GetGenericArguments().Length == 2
                            && m.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), type)
                    .Invoke(null, new object[] { query, lambda });

            return (IOrderedQueryable<T>)result;
        }
    }
}
