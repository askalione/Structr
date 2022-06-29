//using FluentAssertions;
//using Structr.Abstractions;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;

//namespace Structr.Tests.Abstractions
//{
//    public class SequentialGuidTests
//    {
//        [InlineData(SequentialGuidType.String)]
//        [InlineData(SequentialGuidType.Binary)]
//        [InlineData(SequentialGuidType.Ending)]
//        [Theory]
//        public void NewGuid(SequentialGuidType type)
//        {
//            // Act
//            var result = SequentialGuid.NewGuid(type);

//            // Assert
//            result.Should().NotBeEmpty();
//        }

//        private Guid MaxGuid() =>
//            new Guid(int.MaxValue,
//                short.MaxValue,
//                short.MaxValue,
//                byte.MaxValue,
//                byte.MaxValue,
//                byte.MaxValue,
//                byte.MaxValue,
//                byte.MaxValue,
//                byte.MaxValue,
//                byte.MaxValue,
//                byte.MaxValue);

//        [InlineData(SequentialGuidType.String)]
//        [InlineData(SequentialGuidType.Binary)]
//        [InlineData(SequentialGuidType.Ending)]
//        [Theory]
//        public void NewGuid_is_sequential(SequentialGuidType type)
//        {
//            // Act
//            var guid1 = SequentialGuid.NewGuid(type);
//            var guid2 = SequentialGuid.NewGuid(type);

//            // Assert
//            Assert.Equal(-1, guid1.CompareTo(guid2));
//        }
//    }
//}
