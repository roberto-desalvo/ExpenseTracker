using FluentAssertions;
using Moq;
using RDS.ExpenseTracker.Importer.Parsers.CustomExcelParser;
using RDS.ExpenseTracker.Importer.Parsers.CustomExcelParser.Helpers;
using RDS.ExpenseTracker.Importer.Parsers.CustomExcelParser.Models;
using RDS.ExpenseTracker.Domain.Models;
using System.Data;
using Xunit;
using RDS.ExpenseTracker.Importer.Parsers.Abstractions;

namespace RDS.ExpenseTracker.Importer.Tests.Parsers.CustomExcelParser
{
    public class CustomExcelTransactionDataParserTests
    {
        private readonly Mock<IExcelFileReader> _fileReaderMock;
        private readonly ExcelImporterConfiguration _config;
        private readonly ExcelTransactionDataParser _parser;

        public CustomExcelTransactionDataParserTests()
        {
            _fileReaderMock = new Mock<IExcelFileReader>();
            _config = new ExcelImporterConfiguration
            {
                FilePath = "test.xlsx",
                SheetsToIgnore = new List<string> { "ignore" },
                FirstAccountName = "Account1",
                SecondAccountName = "Account2",
                ThirdAccountName = "Account3",
                TransactionDateIndex = 0,
                TransactionDescriptionIndex = 1,
                TransactionOutflowIndex = 2,
                TransactionInflowIndex = 3,
                TransactionAccountNameIndex = 4,
                TransferDateIndex = 5,
                TransferDescriptionIndex = 6,
                TransferAmountIndex = 7
            };
            _parser = new ExcelTransactionDataParser(_fileReaderMock.Object, _config);
        }

        [Fact]
        public void Constructor_WhenConfigIsNull_ShouldThrowArgumentNullException()
        {
            // Act
            Action act = () => new ExcelTransactionDataParser(_fileReaderMock.Object, null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*config*");
        }

        [Fact]
        public void Constructor_WhenExcelFileReaderIsNull_ShouldThrowArgumentNullException()
        {
            // Act
            Action act = () => new ExcelTransactionDataParser(null, _config);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*excelFileReader*");
        }

        [Fact]
        public void GetTransactionsFromRow_WhenTransactionAmountIsZero_ShouldNotReturnTransaction()
        {
            // Arrange
            var dataTable = new DataTable();
            dataTable.Columns.Add("Date");
            dataTable.Columns.Add("Description");
            dataTable.Columns.Add("Outflow");
            dataTable.Columns.Add("Inflow");
            dataTable.Columns.Add("AccountName");
            dataTable.Columns.Add("TransferDate");
            dataTable.Columns.Add("TransferDescription");
            dataTable.Columns.Add("TransferAmount");

            var dataRow = dataTable.NewRow();
            dataRow["Date"] = "2021-01-01";
            dataRow["Description"] = "Test Transaction";
            dataRow["Outflow"] = "0";
            dataRow["Inflow"] = "0";
            dataRow["AccountName"] = "Account1";
            dataRow["TransferDate"] = "2021-01-01";
            dataRow["TransferDescription"] = "Transfer";
            dataRow["TransferAmount"] = "0";
            dataTable.Rows.Add(dataRow);

            // Act
            var transactions = _parser.GetTransactionsFromRow(dataRow).ToList();

            // Assert
            transactions.Should().BeEmpty();
        }

        [Fact]
        public void GetTransactionsFromRow_WhenTransactionAmountIsNonZero_ShouldReturnTransaction()
        {
            // Arrange
            var dataTable = new DataTable();
            dataTable.Columns.Add("Date");
            dataTable.Columns.Add("Description");
            dataTable.Columns.Add("Outflow");
            dataTable.Columns.Add("Inflow");
            dataTable.Columns.Add("AccountName");
            dataTable.Columns.Add("TransferDate");
            dataTable.Columns.Add("TransferDescription");
            dataTable.Columns.Add("TransferAmount");

            var dataRow = dataTable.NewRow();
            dataRow["Date"] = "2021-01-01";
            dataRow["Description"] = "Test Transaction";
            dataRow["Outflow"] = "100";
            dataRow["Inflow"] = "0";
            dataRow["AccountName"] = "Account1";
            dataRow["TransferDate"] = "2021-01-01";
            dataRow["TransferDescription"] = "Transfer";
            dataRow["TransferAmount"] = "50";
            dataTable.Rows.Add(dataRow);

            // Act
            var transactions = _parser.GetTransactionsFromRow(dataRow).ToList();

            // Assert
            transactions.Should().HaveCount(3);
            transactions[0].Amount.Should().Be(-10000);
            transactions[0].Description.Should().Be("Test Transaction");
            transactions[0].FinancialAccountName.Should().Be("Account1");
            transactions[0].IsTransfer.Should().BeFalse();
        }

        [Fact]
        public void GetTransactionsFromRow_WhenTransferAmountIsNonZero_ShouldReturnTransferTransactions()
        {
            // Arrange
            var dataTable = new DataTable();
            dataTable.Columns.Add("Date");
            dataTable.Columns.Add("Description");
            dataTable.Columns.Add("Outflow");
            dataTable.Columns.Add("Inflow");
            dataTable.Columns.Add("AccountName");
            dataTable.Columns.Add("TransferDate");
            dataTable.Columns.Add("TransferDescription");
            dataTable.Columns.Add("TransferAmount");

            var dataRow = dataTable.NewRow();
            dataRow["Date"] = "2021-01-01";
            dataRow["Description"] = "Test Transaction";
            dataRow["Outflow"] = "0";
            dataRow["Inflow"] = "0";
            dataRow["AccountName"] = "Account1";
            dataRow["TransferDate"] = "2021-01-01";
            dataRow["TransferDescription"] = "Transfer to Account2";
            dataRow["TransferAmount"] = "50";
            dataTable.Rows.Add(dataRow);

            // Act
            var transactions = _parser.GetTransactionsFromRow(dataRow).ToList();

            // Assert
            transactions.Should().HaveCount(2);
            transactions[0].Amount.Should().Be(-5000);
            transactions[0].Description.Should().Be("Transfer to Account2");
            transactions[0].FinancialAccountName.Should().Be("Account1");
            transactions[0].IsTransfer.Should().BeTrue();

            transactions[1].Amount.Should().Be(5000);
            transactions[1].Description.Should().Be("Transfer to Account2");
            transactions[1].FinancialAccountName.Should().Be("Account2");
            transactions[1].IsTransfer.Should().BeTrue();
        }

        [Fact]
        public void GetTransactionsFromDataTable_WhenCalled_ShouldReturnTransactions()
        {
            // Arrange
            var dataTable = new DataTable("gennaio 2021");
            dataTable.Columns.Add("Date");
            dataTable.Columns.Add("Description");
            dataTable.Columns.Add("Outflow");
            dataTable.Columns.Add("Inflow");
            dataTable.Columns.Add("AccountName");
            dataTable.Columns.Add("TransferDate");
            dataTable.Columns.Add("TransferDescription");
            dataTable.Columns.Add("TransferAmount");

            var dataRow = dataTable.NewRow();
            dataRow["Date"] = "2021-01-01";
            dataRow["Description"] = "Test Transaction";
            dataRow["Outflow"] = "100";
            dataRow["Inflow"] = "0";
            dataRow["AccountName"] = "Account1";
            dataRow["TransferDate"] = "2021-01-01";
            dataRow["TransferDescription"] = "Transfer";
            dataRow["TransferAmount"] = "50";
            dataTable.Rows.Add(dataRow);

            // Act
            var transactions = _parser.GetTransactionsFromDataTable(dataTable).ToList();

            // Assert
            transactions.Should().HaveCount(3);
        }

        [Fact]
        public void GetDataRowModel_WhenCalled_ShouldReturnCustomExcelDataRowModel()
        {
            // Arrange
            var dataTable = new DataTable();
            dataTable.Columns.Add("Date");
            dataTable.Columns.Add("Description");
            dataTable.Columns.Add("Outflow");
            dataTable.Columns.Add("Inflow");
            dataTable.Columns.Add("AccountName");
            dataTable.Columns.Add("TransferDate");
            dataTable.Columns.Add("TransferDescription");
            dataTable.Columns.Add("TransferAmount");

            var dataRow = dataTable.NewRow();
            dataRow["Date"] = "2021-01-01";
            dataRow["Description"] = "Test Transaction";
            dataRow["Outflow"] = "100";
            dataRow["Inflow"] = "0";
            dataRow["AccountName"] = "Account1";
            dataRow["TransferDate"] = "2021-01-01";
            dataRow["TransferDescription"] = "Transfer";
            dataRow["TransferAmount"] = "50";
            dataTable.Rows.Add(dataRow);

            // Act
            var model = _parser.GetDataRowModel(dataRow);

            // Assert
            model.TransactionDate.Should().Be(new DateTime(2021, 1, 1));
            model.TransactionDescription.Should().Be("Test Transaction");
            model.TransactionAmount.Should().Be(-10000); // 100 * -100
            model.TransactionAccountName.Should().Be("Account1");
            model.TransferDate.Should().Be(new DateTime(2021, 1, 1));
            model.TransferDescription.Should().Be("Transfer");
            model.TransferAmount.Should().Be(5000);
        }

        [Fact]
        public void ExtractStandardTransaction_WhenCalled_ShouldReturnTransaction()
        {
            // Arrange
            var model = new ExcelDataRowModel
            {
                TransactionDate = new DateTime(2021, 1, 1),
                TransactionDescription = "Test Transaction",
                TransactionAmount = -100,
                TransactionAccountName = "Account1"
            };

            // Act
            var transaction = _parser.ExtractStandardTransaction(model);

            // Assert
            transaction.Amount.Should().Be(-100);
            transaction.Date.Should().Be(new DateTime(2021, 1, 1));
            transaction.Description.Should().Be("Test Transaction");
            transaction.FinancialAccountName.Should().Be("Account1");
            transaction.IsTransfer.Should().BeFalse();
        }

        [Fact]
        public void ExtractOutgoingTransfer_WhenCalled_ShouldReturnTransaction()
        {
            // Arrange
            var model = new ExcelDataRowModel
            {
                TransferDate = new DateTime(2021, 1, 1),
                TransferDescription = "Transfer",
                TransferAmount = 50
            };

            // Act
            var transaction = _parser.ExtractOutgoingTransfer(model);

            // Assert
            transaction.Amount.Should().Be(-50);
            transaction.Date.Should().Be(new DateTime(2021, 1, 1));
            transaction.Description.Should().Be("Transfer");
            transaction.FinancialAccountName.Should().Be(_config.FirstAccountName);
            transaction.IsTransfer.Should().BeTrue();
        }

        [Fact]
        public void ExtractIngoingTransfer_WhenCalled_ShouldReturnTransaction()
        {
            // Arrange
            var model = new ExcelDataRowModel
            {
                TransferDate = new DateTime(2021, 1, 1),
                TransferDescription = "Transfer to Account2",
                TransferAmount = 50
            };

            // Act
            var transaction = _parser.ExtractIngoingTransfer(model);

            // Assert
            transaction.Amount.Should().Be(50);
            transaction.Date.Should().Be(new DateTime(2021, 1, 1));
            transaction.Description.Should().Be("Transfer to Account2");
            transaction.FinancialAccountName.Should().Be(_config.SecondAccountName);
            transaction.IsTransfer.Should().BeTrue();
        }

        [Fact]
        public void ParseTransactions_WhenCalled_ShouldReturnTransactions()
        {
            // Arrange
            var dataTable = new DataTable("gennaio 2021");
            dataTable.Columns.Add("Date");
            dataTable.Columns.Add("Description");
            dataTable.Columns.Add("Outflow");
            dataTable.Columns.Add("Inflow");
            dataTable.Columns.Add("AccountName");
            dataTable.Columns.Add("TransferDate");
            dataTable.Columns.Add("TransferDescription");
            dataTable.Columns.Add("TransferAmount");

            var dataRow = dataTable.NewRow();
            dataRow["Date"] = "2021-01-01";
            dataRow["Description"] = "Test Transaction";
            dataRow["Outflow"] = "100";
            dataRow["Inflow"] = "0";
            dataRow["AccountName"] = "Account1";
            dataRow["TransferDate"] = "2021-01-01";
            dataRow["TransferDescription"] = "Transfer";
            dataRow["TransferAmount"] = "50";
            dataTable.Rows.Add(dataRow);

            var dataSet = new DataSet();
            dataSet.Tables.Add(dataTable);

            _fileReaderMock.Setup(fr => fr.ReadExcelFile(It.IsAny<string>())).Returns(dataSet);

            // Act
            var transactions = _parser.ParseTransactions();

            // Assert
            transactions.Should().HaveCount(3);
        }

        [Fact]
        public void ParseTransactions_WhenExceptionThrown_ShouldReturnEmptyList()
        {
            // Arrange
            _fileReaderMock.Setup(fr => fr.ReadExcelFile(It.IsAny<string>())).Throws(new Exception("Test Exception"));

            // Act
            var transactions = _parser.ParseTransactions();

            // Assert
            transactions.Should().BeEmpty();
        }
    }
}
