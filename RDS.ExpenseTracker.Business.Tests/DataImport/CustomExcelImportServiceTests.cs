using FluentAssertions;
using Moq;
using RDS.ExpenseTracker.Business.DataImport;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTracker.DataAccess.Entities;
using RDS.ExpenseTracker.Domain.Models;
using RDS.ExpenseTracker.Importer.Parsers.CustomExcelParser;
using RDS.ExpenseTracker.Importer.Parsers.CustomExcelParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RDS.ExpenseTracker.Business.Tests.DataImport
{
    public class CustomExcelImportServiceTests
    {
        private readonly Mock<ExcelTransactionDataParser> _parserMock;
        private readonly Mock<IFinancialAccountService> _accountServiceMock;
        private readonly Mock<ITransactionService> _transactionServiceMock;
        private readonly Mock<ICategoryService> _categoryServiceMock;
        private readonly CustomExcelImportService _service;

        public CustomExcelImportServiceTests()
        {
            _parserMock = new Mock<ExcelTransactionDataParser>();
            _accountServiceMock = new Mock<IFinancialAccountService>();
            _transactionServiceMock = new Mock<ITransactionService>();
            _categoryServiceMock = new Mock<ICategoryService>();
            _service = new CustomExcelImportService(_parserMock.Object, _accountServiceMock.Object, _transactionServiceMock.Object, _categoryServiceMock.Object);
        }

        [Fact]
        public async Task ImportTransactions_WhenCalled_ShouldImportTransactions()
        {
            // Arrange
            var transactions = new List<Transaction>
            {
                new Transaction { Description = "Test transaction" }
            };

            _parserMock.Setup(p => p.ParseTransactions()).Returns(transactions);
            _accountServiceMock.Setup(s => s.GetFinancialAccounts(It.IsAny<Func<IQueryable<EFinancialAccount>, IQueryable<EFinancialAccount>>>()))
                .ReturnsAsync(new List<FinancialAccount>());
            _categoryServiceMock.Setup(s => s.GetCategories()).ReturnsAsync(new List<Category>());
            _categoryServiceMock.Setup(s => s.GetDefaultCategory()).ReturnsAsync(new Category());

            // Act
            await _service.ImportTransactions();

            // Assert
            _transactionServiceMock.Verify(s => s.DeleteAllTransactions(), Times.Once);
            _accountServiceMock.Verify(s => s.GetFinancialAccounts(), Times.Once);
            _transactionServiceMock.Verify(s => s.AddTransactions(transactions), Times.Once);
            _accountServiceMock.Verify(s => s.CalculateAvailabilities(transactions), Times.Once);
        }

        [Fact]
        public async Task ImportTransactions_WhenCalled_ShouldRestoreAccountBaseAmounts()
        {
            // Arrange
            var accounts = new List<FinancialAccount>
            {
                new FinancialAccount { Id = 1, Name = "Account1" }
            };

            var config = new ExcelImporterConfiguration
            {
                AccountInitialAmounts = new Dictionary<string, int>
                {
                    { "Account1", 1000 }
                }
            };

            _parserMock.SetupGet(p => p.Config).Returns(config);
            _accountServiceMock.Setup(s => s.GetFinancialAccounts()).ReturnsAsync(accounts);

            // Act
            await _service.ImportTransactions();

            // Assert
            _accountServiceMock.Verify(s => s.UpdateFinancialAccount(It.Is<FinancialAccount>(a => a.Availability == 1000)), Times.Once);
        }

        [Fact]
        public async Task ImportTransactions_WhenAccountNotFound_ShouldThrowException()
        {
            // Arrange
            var accounts = new List<FinancialAccount>();

            var config = new ExcelImporterConfiguration
            {
                AccountInitialAmounts = new Dictionary<string, int>
                {
                    { "Account1", 1000 }
                }
            };

            _parserMock.SetupGet(p => p.Config).Returns(config);
            _accountServiceMock.Setup(s => s.GetFinancialAccounts()).ReturnsAsync(accounts);

            // Act
            Func<Task> act = async () => await _service.ImportTransactions();

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Account Account1 not found");
        }

        [Fact]
        public async Task RestoreAccountBaseAmounts_WhenCalled_ShouldUpdateAccountAvailability()
        {
            // Arrange
            var accounts = new List<FinancialAccount>
            {
                new FinancialAccount { Id = 1, Name = "Account1" }
            };

            var config = new ExcelImporterConfiguration
            {
                AccountInitialAmounts = new Dictionary<string, int>
                {
                    { "Account1", 1000 }
                }
            };

            _parserMock.SetupGet(p => p.Config).Returns(config);
            _accountServiceMock.Setup(s => s.GetFinancialAccounts()).ReturnsAsync(accounts);

            // Act
            await _service.ImportTransactions();

            // Assert
            _accountServiceMock.Verify(s => s.UpdateFinancialAccount(It.Is<FinancialAccount>(a => a.Availability == 1000)), Times.Once);
        }

        [Fact]
        public async Task RestoreAccountBaseAmounts_WhenAccountNotFound_ShouldThrowException()
        {
            // Arrange
            var accounts = new List<FinancialAccount>();

            var config = new ExcelImporterConfiguration
            {
                AccountInitialAmounts = new Dictionary<string, int>
                {
                    { "Account1", 1000 }
                }
            };

            _parserMock.SetupGet(p => p.Config).Returns(config);
            _accountServiceMock.Setup(s => s.GetFinancialAccounts()).ReturnsAsync(accounts);

            // Act
            Func<Task> act = async () => await _service.ImportTransactions();

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Account Account1 not found");
        }
    }
}

