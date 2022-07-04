using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Structr.Abstractions.Providers.Timestamp;
using System;
using System.Collections.Generic;
using Xunit;

namespace Structr.Tests.Abstractions
{
    public class ServiceCollectionExtensionsTests
    {
        private enum TicketType
        {
            ClientTicket,
            EmployeeTicket,
            AdminTicket,
            BotTicket
        }

        private class Ticket
        {
            public TicketType Type { get; set; }
            /* some other data */
        }

        private interface ITicketLifecycleManager
        {
            public string ProcessTicket(Ticket ticket);
        }

        private class ClientTicketLifecycleManager : ITicketLifecycleManager
        {
            public string ProcessTicket(Ticket ticket)
            {
                /* some client-specific ticket proccessing stuff */
                return "ClientTicketProcessed";
            }
        }

        private class EmployeeTicketLifecycleManager : ITicketLifecycleManager
        {
            public string ProcessTicket(Ticket ticket)
            {
                /* some employee-specific ticket proccessing stuff */
                return "EmployeeTicketProcessed";
            }
        }

        private class SomeUnknownLifecycleManager
        {
            public string ProcessSomethingElse(object somethingElse)
            {
                /* some employee-specific ticket proccessing stuff */
                return "SomethingElseProcessed";
            }
        }

        [Fact]
        public void AddFactory()
        {
            // Act
            var servicesProvider = new ServiceCollection()
                .AddFactory<TicketType, ITicketLifecycleManager>(new Dictionary<TicketType, Type> {
                    { TicketType.ClientTicket, typeof(ClientTicketLifecycleManager) },
                    { TicketType.EmployeeTicket, typeof(EmployeeTicketLifecycleManager) },
                })
                .BuildServiceProvider();

            // Assert
            var result = servicesProvider.GetService<Func<TicketType, ITicketLifecycleManager>>();
            result(TicketType.ClientTicket).ProcessTicket(new Ticket()).Should().Be("ClientTicketProcessed");
            result(TicketType.EmployeeTicket).ProcessTicket(new Ticket()).Should().Be("EmployeeTicketProcessed");
        }

        [Fact]
        public void AddFactory_throws_when_no_implementations_provided()
        {
            // Act
            Action act = () => new ServiceCollection()
                .AddFactory<TicketType, ITicketLifecycleManager>(new Dictionary<TicketType, Type> { })
                .BuildServiceProvider();

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Items on index not found. At least one item for configured factory is required.");
        }

        [Fact]
        public void AddFactory_throws_when_adding_wrong_implementation()
        {
            // Act
            Action act = () => new ServiceCollection()
                .AddFactory<TicketType, ITicketLifecycleManager>(new Dictionary<TicketType, Type> {
                    { TicketType.ClientTicket, typeof(SomeUnknownLifecycleManager) },
                })
                .BuildServiceProvider();

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("Value must be inherited from*ITicketLifecycleManager*");
        }

        [Fact]
        public void AddTimestampProvider()
        {
            // Act
            var result = new ServiceCollection()
                .AddTimestampProvider<LocalTimestampProvider>()
                .BuildServiceProvider();

            // Assert
            result.GetService<ITimestampProvider>().Should().BeOfType<LocalTimestampProvider>();
        }
    }
}
