using Structr.Collections;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AutoMapper
{
    public static class MapperExtensions
    {
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
