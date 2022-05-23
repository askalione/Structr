using FluentAssertions;
using Structr.Abstractions.Attributes;
using Structr.Abstractions.Helpers;
using System;
using System.Collections.Generic;
using Xunit;

namespace Structr.Tests.Abstractions.Helpers
{
    public class BindHelperTests
    {
        public class Flower
        {
            public FlowerId Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public FlowerColor Color { get; set; }
            public DateTime? DateCreated { get; set; }
        }

        public enum FlowerColor
        {
            White,
            Yellow,
            Red
        }

        public enum FlowerId
        {
            [BindProperty("Description", "Gazania description")]
            [BindProperty("Color", FlowerColor.Yellow)]
            [BindProperty("DateCreated", null)]
            Gazania,

            [BindProperty("Description", "Windflower description")]
            [BindProperty("Color", FlowerColor.Red)]
            [BindProperty("DateCreated", "2020-04-20")]
            Windflower,
        }

        [Fact]
        public void Bind()
        {
            // Act
            var result = BindHelper.Bind<Flower, FlowerId>((o, e) =>
            {
                o.Id = e;
                o.Name = e.ToString();
            });

            // Assert
            var expected = new List<Flower>
            {
                new Flower
                {
                    Id = FlowerId.Gazania,
                    Name = "Gazania",
                    Description = "Gazania description",
                    Color = FlowerColor.Yellow,
                    DateCreated = null
                },
                new Flower
                {
                    Id = FlowerId.Windflower,
                    Name = "Windflower",
                    Description = "Windflower description",
                    Color = FlowerColor.Red,
                    DateCreated = DateTime.Parse("2020-04-20")
                }
            };
            result.Should().BeEquivalentTo(expected);
        }
    }
}