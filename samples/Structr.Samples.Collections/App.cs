using AutoMapper;
using Structr.Collections;
using Structr.Samples.Collections.Dto;
using Structr.Samples.Collections.Entities;
using Structr.Samples.Collections.Infrastructure;
using Structr.Samples.IO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Structr.Samples.Collections
{
    public class App : IApp
    {
        private readonly IMapper _mapper;
        private readonly IStringWriter _writer;

        public App(IMapper mapper, IStringWriter writer)
        {
            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            _mapper = mapper;
            _writer = writer;
        }

        public async Task RunAsync()
        {
            var items = new List<Foo>
            {
                new Foo("Foo1", Currency.Rub, 10),
                new Foo("Foo2", Currency.Rub, 20),
                new Foo("Foo3", Currency.Rub, 30)
            };
            var pagedListEntities = items.ToPagedList(1, 5);
            await WriteAsync(pagedListEntities);

            await _writer.WriteLineAsync("-------------------");

            var itemsDto = _mapper.MapList<FooDto>(items);
            await WriteAsync(itemsDto);

            await _writer.WriteLineAsync("-------------------");

            var pagedListDto = _mapper.MapPagedList<FooDto>(pagedListEntities);
            await WriteAsync(pagedListDto);
        }

        private async Task WriteAsync<T>(IEnumerable<T> collection) where T : Writable
        {
            foreach (var item in collection)
            {
                await _writer.WriteLineAsync(item.ToString());
            }
        }
    }
}
