﻿using FluentAssertions;
using RDS.ExpenseTracker.Importer.Parsers.CustomExcelParser;

namespace RDS.ExpenseTracker.Tests.Business.Helpers
{
    public class UtilitiesTests
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

            var result = CustomExcelParserUtilities.ParseDateFromSheetName(text);
            result.Should().Be(expected);
        }
    }
}
