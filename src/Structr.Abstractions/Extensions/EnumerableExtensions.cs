using Structr.Abstractions.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Structr.Abstractions.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Enumerable"/>.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Sorts the elements of a sequence in order and by properties provided via dictionary.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="sort">Dictionary with property names and correspounding orders to sort by.</param>
        /// <returns>An System.Linq.IOrderedEnumerable`1 whose elements are sorted according to provided dictionary.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static IOrderedEnumerable<T> OrderBy<T>(this IEnumerable<T> source, IReadOnlyDictionary<string, Order> sort)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(sort, nameof(sort));

            if (sort.Count == 0)
            {
                return (IOrderedEnumerable<T>)source;
            }

            foreach (string propertyName in sort.Keys)
            {
                if (string.IsNullOrWhiteSpace(propertyName))
                {
                    throw new InvalidOperationException("Invalid property name to sort");
                }

                source = propertyName == sort.Keys.First()
                    ? source.OrderBy(propertyName, sort[propertyName])
                    : source.ThenBy(propertyName, sort[propertyName]);
            }

            return (IOrderedEnumerable<T>)source;
        }

        public static IOrderedEnumerable<T> OrderBy<T>(this IEnumerable<T> source, string propertyName, Order order)
        {
            return Order(source, propertyName, order == Abstractions.Order.Asc ? OrderMethod.OrderBy : OrderMethod.OrderByDescending);
        }

        public static IOrderedEnumerable<T> OrderBy<T>(this IEnumerable<T> source, string propertyName)
        {
            return Order(source, propertyName, OrderMethod.OrderBy);
        }

        public static IOrderedEnumerable<T> OrderByDescending<T>(this IEnumerable<T> source, string propertyName)
        {
            return Order(source, propertyName, OrderMethod.OrderByDescending);
        }

        public static IOrderedEnumerable<T> ThenBy<T>(this IEnumerable<T> source, string propertyName, Order order)
        {
            return Order(source, propertyName, order == Abstractions.Order.Asc ? OrderMethod.ThenBy : OrderMethod.ThenByDescending);
        }

        public static IOrderedEnumerable<T> ThenBy<T>(this IOrderedEnumerable<T> source, string propertyName)
        {
            return Order(source, propertyName, OrderMethod.ThenBy);
        }

        public static IOrderedEnumerable<T> ThenByDescending<T>(this IOrderedEnumerable<T> source, string propertyName)
        {
            return Order(source, propertyName, OrderMethod.ThenByDescending);
        }

        private static IOrderedEnumerable<T> Order<T>(IEnumerable<T> source, string propertyName, OrderMethod method)
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
                {
                    throw new InvalidOperationException($"Nested property with name {propertyName} for type {typeof(T).Name} not found.");
                }
                expr = Expression.Property(expr, propertyInfo);
                type = propertyInfo.PropertyType;
            }

            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            MethodInfo orderingMethod = typeof(Enumerable)
                .GetMethods()
                .SingleOrDefault(m =>
                {
                    var parameters = m.GetParameters();
                    return m.Name == method.ToString()
                        && m.IsGenericMethodDefinition
                        && m.GetGenericArguments().Length == 2
                        && parameters.Length == 2
                        && parameters[0].ParameterType.GetGenericTypeDefinition()
                            .IsAssignableFromGenericType(source.GetType().GetGenericTypeDefinition())
                        && parameters[1].ParameterType.GetGenericTypeDefinition()
                            .IsAssignableFromGenericType(delegateType.GetGenericTypeDefinition());
                })?
                .MakeGenericMethod(typeof(T), type);

            if (orderingMethod == null)
            {
                throw new InvalidOperationException($"Ordering method \"{method}\" not found.");
            }

            object result = orderingMethod.Invoke(null, new object[] { source, lambda.Compile() });
            return (IOrderedEnumerable<T>)result;
        }

        /// <summary>
        /// Gets a random element from provided collection.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source collection.</typeparam>
        /// <param name="source">A sequence of values to pick from.</param>
        /// <returns>Single element picked at random from source collection.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static T PickRandom<T>(this IEnumerable<T> source)
        {
            Ensure.NotNull(source, nameof(source));

            return source.PickRandom(1).SingleOrDefault();
        }

        /// <summary>
        /// Randomly gets <paramref name="count"/> items from <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source collection.</typeparam>
        /// <param name="source">A sequence of values to pick from.</param>
        /// <returns>Random number of first elements from source collection.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
        {
            Ensure.NotNull(source, nameof(source));

            return source.Shuffle().Take(count);
        }

        /// <summary>
        /// Shuffles source collection changing it's elements positions at random.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source collection.</typeparam>
        /// <param name="source">A sequence of values to shuffle.</param>
        /// <returns>Collection of elements from source collection, but shuffeled randomly.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            Ensure.NotNull(source, nameof(source));

            return source.OrderBy(x => Guid.NewGuid());
        }

        /// <summary>
        /// Performs simple foreach-like iteration on source collection while invoking provided method for each element of collection.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source collection.</typeparam>
        /// <param name="source">A sequence of values to shuffle.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(action, nameof(action));

            foreach (var item in source)
            {
                action(item);
            }
        }

        /// <summary>
        /// Performs simple foreach-like iteration on source collection while invoking provided function for every
        /// element of collection. Breaks iteration when first <see langword="true"/> result got from function.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source collection.</typeparam>
        /// <param name="source">A sequence of values to shuffle.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void ForEachOrBreak<T>(this IEnumerable<T> source, Func<T, bool> func)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(func, nameof(func));

            foreach (var item in source)
            {
                bool result = func(item);
                if (result)
                {
                    break;
                }
            }
        }
    }
}
