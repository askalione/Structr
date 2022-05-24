using Structr.Collections;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AutoMapper
{
    public static class MapperExtensions
    {
        /// <summary>
        /// Executes a mapping from the source list to a new destination list.
        /// </summary>
        /// <typeparam name="TDestination">Destination elements type to create.</typeparam>
        /// <param name="mapper">Automaper instance to perform mapping.</param>
        /// <param name="collection">Source collection to map from.</param>
        /// <returns>Result collection with elements got by mapping elements from source collection.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<TDestination> MapList<TDestination>(this IMapper mapper, IEnumerable collection)
        {
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            return mapper.Map<IEnumerable<TDestination>>(collection);
        }

        /// <summary>
        /// Executes a mapping from the source <see cref="PagedList{T}"></see> to a new destination list.
        /// </summary>
        /// <typeparam name="TDestination">Type of destination elements to create.</typeparam>
        /// <param name="mapper">Automaper instance to perform mapping.</param>
        /// <param name="collection">Source collection to map from.</param>
        /// <returns>Result collection with elements got by mapping elements from source collection.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IPagedList<TDestination> MapPagedList<TDestination>(this IMapper mapper, IPagedList collection)
        {
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            return collection.ToPagedList(mapper.Map<IEnumerable<TDestination>>(collection));
        }
    }
}
