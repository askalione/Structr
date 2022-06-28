using Structr.Abstractions.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Structr.Abstractions.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="IQueryable{T}"/>.
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Applies pagination to <see cref="IQueryable"/> instance skipping and taking specified number of elements.
        /// </summary>
        /// <typeparam name="T">Type of queryable objects.</typeparam>
        /// <param name="query">Query.</param>
        /// <param name="skip">Number of elements which must be skipped. Must be greater or equal 0.</param>
        /// <param name="take">Number of elements which must be taken. Must be greater or equal 1.</param>
        /// <returns>
        /// An <see cref="IQueryable{out T}"/> that contains specified number of elements that occur after the
        /// specified index in the input sequence.
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static IQueryable<T> PageBy<T>(this IQueryable<T> query, int skip, int take)
        {
            Ensure.NotNull(query, nameof(query));
            Ensure.GreaterThan(skip, 0, nameof(skip));
            Ensure.GreaterThan(take, 1, nameof(take));

            return query
                .Skip(skip)
                .Take(take);
        }

        /// <summary>
        /// Sorts the elements of a sequence in order and by properties provided via dictionary.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="query">A sequence of values to order.</param>
        /// <param name="sort">Dictionary with property names and correspounding orders to sort by.</param>
        /// <returns>
        /// Sorted <see cref="IQueryable{out T}"/>.
        /// </returns>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, IReadOnlyDictionary<string, SortOrder> sort)
        {
            Ensure.NotNull(query, nameof(query));
            Ensure.NotNull(sort, nameof(sort));

            if (sort.Count == 0)
            {
                return (IOrderedQueryable<T>)query;
            }

            foreach (string propertyName in sort.Keys)
            {
                if (string.IsNullOrWhiteSpace(propertyName))
                {
                    throw new InvalidOperationException("Invalid property name to sort");
                }

                query = propertyName == sort.Keys.First()
                    ? query.OrderBy(propertyName, sort[propertyName])
                    : query.ThenBy(propertyName, sort[propertyName]);
            }

            return (IOrderedQueryable<T>)query;
        }

        /// <summary>
        /// Sorts the elements of a sequence by specified property name and order.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="query">A sequence of values to order.</param>
        /// <param name="order">Ascending flag.</param>
        /// <returns>
        /// Sorted <see cref="IQueryable{out T}"/>.
        /// </returns>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string propertyName, SortOrder order)
        {
            return Order(query, propertyName, order == Abstractions.SortOrder.Asc ? OrderMethod.OrderBy : OrderMethod.OrderByDescending);
        }

        /// <summary>
        /// Sorts the elements of a sequence by specified property name.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="query">A sequence of values to order.</param>
        /// <returns>
        /// Sorted <see cref="IQueryable{out T}"/>.
        /// </returns>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string propertyName)
        {
            return Order(query, propertyName, OrderMethod.OrderBy);
        }

        /// <summary>
        /// Sorts the elements of a sequence in descending order by specified property name.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="query">A sequence of values to order.</param>
        /// <returns>
        /// Sorted <see cref="IQueryable{out T}"/>.
        /// </returns>
        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> query, string propertyName)
        {
            return Order(query, propertyName, OrderMethod.OrderByDescending);
        }

        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence by specified property name and order.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="query">A sequence of values to order.</param>
        /// <param name="order">Ascending flag.</param>
        /// <returns>
        /// Sorted <see cref="IQueryable{out T}"/>.
        /// </returns>
        public static IOrderedQueryable<T> ThenBy<T>(this IQueryable<T> query, string propertyName, SortOrder order)
        {
            return Order(query, propertyName, order == Abstractions.SortOrder.Asc ? OrderMethod.ThenBy : OrderMethod.ThenByDescending);
        }

        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence by specified property name.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="query">A sequence of values to order.</param>
        /// <returns>
        /// Sorted <see cref="IQueryable{out T}"/>.
        /// </returns>
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> query, string propertyName)
        {
            return Order(query, propertyName, OrderMethod.ThenBy);
        }

        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in descending order by specified property name.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="query">A sequence of values to order.</param>
        /// <returns>
        /// Sorted <see cref="IQueryable{out T}"/>.
        /// </returns>
        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> query, string propertyName)
        {
            return Order(query, propertyName, OrderMethod.ThenByDescending);
        }

        /// <summary>
        /// Apply ordering to sort queryable result.
        /// </summary>
        /// <typeparam name="T">Type of queryable objects.</typeparam>
        /// <param name="query">Query.</param>
        /// <param name="propertyName">Name of property.</param>
        /// <param name="method">Flag of order method.</param>
        /// <returns>
        /// Sorted <see cref="IQueryable{out T}"/>.
        /// </returns>
        /// <remarks>
        /// Using reflection (not ComponentModel) to mirror LINQ.
        /// </remarks>
        private static IOrderedQueryable<T> Order<T>(IQueryable<T> query, string propertyName, OrderMethod method)
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

            // TODO make same as in EnumerableExtensions with more presice properties checking
            MethodInfo orderingMethod = typeof(Queryable)
                .GetMethods()
                .SingleOrDefault(m
                    => m.Name == method.ToString()
                    && m.IsGenericMethodDefinition
                    && m.GetGenericArguments().Length == 2
                    && m.GetParameters().Length == 2)?
                .MakeGenericMethod(typeof(T), type);

            if (orderingMethod == null)
            {
                throw new InvalidOperationException($"Ordering method \"{method}\" not found.");
            }

            object result = orderingMethod.Invoke(null, new object[] { query, lambda });

            return (IOrderedQueryable<T>)result;
        }
    }
}
