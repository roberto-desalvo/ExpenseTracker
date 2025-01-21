using FluentAssertions;
using RDS.ExpenseTracker.Importer.Utilities;
using System;
using Xunit;

namespace RDS.ExpenseTracker.Importer.Tests.Utilities
{
    public class UtilsTests
    {
        [Fact]
        public void ContainsOne_WhenStringContainsOneOfTheValues_ShouldReturnTrue()
        {
            // Arrange
            var str = "Hello World";
            var values = new[] { "World", "Universe" };

            // Act
            var result = str.ContainsOne(values);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void ContainsOne_WhenStringDoesNotContainAnyOfTheValues_ShouldReturnFalse()
        {
            // Arrange
            var str = "Hello World";
            var values = new[] { "Universe", "Galaxy" };

            // Act
            var result = str.ContainsOne(values);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void ContainsOne_WhenIgnoreCaseIsTrue_ShouldReturnTrue()
        {
            // Arrange
            var str = "Hello World";
            var values = new[] { "world", "universe" };

            // Act
            var result = str.ContainsOne(true, values);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void ParseToDecimal_WhenObjectIsNull_ShouldReturnNull()
        {
            // Arrange
            object obj = null;

            // Act
            var result = obj.ParseToDecimal();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ParseToDecimal_WhenObjectIsValidDecimal_ShouldReturnDecimal()
        {
            // Arrange
            object obj = "123.45";

            // Act
            var result = obj.ParseToDecimal();

            // Assert
            result.Should().Be(123.45m);
        }

        [Fact]
        public void ParseToDecimal_WhenObjectIsInvalidDecimal_ShouldReturnNull()
        {
            // Arrange
            object obj = "invalid";

            // Act
            var result = obj.ParseToDecimal();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ParseToDateTime_WhenObjectIsNull_ShouldReturnNull()
        {
            // Arrange
            object obj = null;

            // Act
            var result = obj.ParseToDateTime();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ParseToDateTime_WhenObjectIsValidDateTime_ShouldReturnDateTime()
        {
            // Arrange
            object obj = "2021-01-01";

            // Act
            var result = obj.ParseToDateTime();

            // Assert
            result.Should().Be(new DateTime(2021, 1, 1));
        }

        [Fact]
        public void ParseToDateTime_WhenObjectIsInvalidDateTime_ShouldReturnNull()
        {
            // Arrange
            object obj = "invalid";

            // Act
            var result = obj.ParseToDateTime();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void IsNullOrZero_WhenNumberIsNull_ShouldReturnTrue()
        {
            // Arrange
            int? num = null;

            // Act
            var result = num.IsNullOrZero();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsNullOrZero_WhenNumberIsZero_ShouldReturnTrue()
        {
            // Arrange
            int? num = 0;

            // Act
            var result = num.IsNullOrZero();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsNullOrZero_WhenNumberIsNotZero_ShouldReturnFalse()
        {
            // Arrange
            int? num = 1;

            // Act
            var result = num.IsNullOrZero();

            // Assert
            result.Should().BeFalse();
        }
    }
}
