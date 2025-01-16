using FluentAssertions;
using RDS.ExpenseTracker.Domain.Models;
using RDS.ExpenseTracker.Importer.Parsers.CustomExcelParser;
using RDS.ExpenseTracker.Importer.Parsers.CustomExcelParser.Helpers;

namespace RDS.ExpenseTracker.Importer.Tests.Parsers.CustomExcelParser.Helpers
{
    public class ParserHelperTests
    {
        public static IEnumerable<object[]> GetTestData()
        {
            yield return new object[] { "gennaio 2021 ", new DateTime(2021, 1, 1) };
            yield return new object[] { " Febbraio2021", new DateTime(2021, 2, 1) };
            yield return new object[] { "Marzo 2022", new DateTime(2022, 3, 1) };
            yield return new object[] { "aprile2023  ", new DateTime(2023, 4, 1) };
            yield return new object[] { "Maggio 2021 ", new DateTime(2021, 5, 1) };
            yield return new object[] { "  giugno 2021 ", new DateTime(2021, 6, 1) };
            yield return new object[] { "luglio  2022 ", new DateTime(2022, 7, 1) };
            yield return new object[] { " agosto 2024 ", new DateTime(2024, 8, 1) };
            yield return new object[] { "settembre2024 ", new DateTime(2024, 9, 1) };
            yield return new object[] { " ottobre 2024 ", new DateTime(2024, 10, 1) };
            yield return new object[] { " novembre2021 ", new DateTime(2021, 11, 1) };
            yield return new object[] { " dicembre 2021 ", new DateTime(2021, 12, 1) };
        }

        [Theory]
        [MemberData(nameof(GetTestData))]
        public static void ParseDateFromSheetName_WhenCalled_ShoulParseCorrectly(string text, DateTime expected)
        {
            var result = ParserHelper.ParseDateFromSheetName(text);
            result.Should().Be(expected);
        }

        [Fact]
        public void CheckAndAssignDate_WhenTransactionDateIsNull_ShouldAssignDefaultDate()
        {
            var transaction = new Transaction { Date = null };
            var defaultDate = new DateTime(2021, 1, 1);

            ParserHelper.CheckAndAssignDate(transaction, defaultDate);

            transaction.Date.Should().Be(defaultDate);
        }

        [Fact]
        public void CheckAndAssignDate_WhenTransactionDateIsNotNull_ShoulNotAssignDefaultDate()
        {
            var originalDate = new DateTime(2021, 2, 2);
            var transaction = new Transaction { Date =  originalDate };
            var defaultDate = new DateTime(2021, 1, 1);

            ParserHelper.CheckAndAssignDate(transaction, defaultDate);

            transaction.Date.Should().NotBe(defaultDate);
        }

        [Fact]
        public void CheckAndAssignDate_WhenTransactionDateIsNotNull_ShouldAssignDefaultDateYear()
        {
            var originalDate = new DateTime(2021, 2, 2);
            var transaction = new Transaction { Date = originalDate };
            var defaultDate = new DateTime(2022, 1, 1);

            ParserHelper.CheckAndAssignDate(transaction, defaultDate);

            transaction.Date.Value.Year.Should().Be(defaultDate.Year);
        }
}
