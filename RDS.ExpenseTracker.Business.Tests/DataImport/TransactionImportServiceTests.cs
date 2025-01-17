using FluentAssertions;
using Moq;
using RDS.ExpenseTracker.Business.DataImport;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTracker.DataAccess.Entities;
using RDS.ExpenseTracker.Domain.Models;
using RDS.ExpenseTracker.Importer.Parsers.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RDS.ExpenseTracker.Business.Tests.DataImport
{
    public class TransactionImportServiceTests
    {
        private readonly Mock<ITransactionDataParser> _parserMock;
        private readonly Mock<IFinancialAccountService> _accountServiceMock;
        private readonly Mock<ITransactionService> _transactionServiceMock;
        private readonly Mock<ICategoryService> _categoryServiceMock;
        private readonly TransactionImportService _service;

        public TransactionImportServiceTests()
        {
            _parserMock = new Mock<ITransactionDataParser>();
            _accountServiceMock = new Mock<IFinancialAccountService>();
            _transactionServiceMock = new Mock<ITransactionService>();
            _categoryServiceMock = new Mock<ICategoryService>();
            _service = new TransactionImportService(_parserMock.Object, _accountServiceMock.Object, _transactionServiceMock.Object, _categoryServiceMock.Object);
        }

        [Fact]
        public void Constructor_WhenParserIsNull_ShouldThrowArgumentNullException()
        {
            // Act
            Action act = () => new TransactionImportService(null, _accountServiceMock.Object, _transactionServiceMock.Object, _categoryServiceMock.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*parser*");
        }

        [Fact]
        public void Constructor_WhenAccountServiceIsNull_ShouldThrowArgumentNullException()
        {
            // Act
            Action act = () => new TransactionImportService(_parserMock.Object, null, _transactionServiceMock.Object, _categoryServiceMock.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*accountService*");
        }

        [Fact]
        public void Constructor_WhenTransactionServiceIsNull_ShouldThrowArgumentNullException()
        {
            // Act
            Action act = () => new TransactionImportService(_parserMock.Object, _accountServiceMock.Object, null, _categoryServiceMock.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*transactionService*");
        }

        [Fact]
        public void Constructor_WhenCategoryServiceIsNull_ShouldThrowArgumentNullException()
        {
            // Act
            Action act = () => new TransactionImportService(_parserMock.Object, _accountServiceMock.Object, _transactionServiceMock.Object, null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*categoryService*");
        }

        [Fact]
        public async Task AssignAccounts_WhenCalled_ShouldAssignAccounts()
        {
            // Arrange
            var transactions = new List<Transaction>
            {
                new Transaction { FinancialAccountName = "Account1" },
                new Transaction { FinancialAccountName = "Account2" }
            };

            var accounts = new List<FinancialAccount>
            {
                new FinancialAccount { Id = 1, Name = "Account1" },
                new FinancialAccount { Id = 2, Name = "Account2" }
            };

            _accountServiceMock.Setup(s => s.GetFinancialAccounts(It.IsAny<Func<IQueryable<EFinancialAccount>, IQueryable<EFinancialAccount>>>()))
                .ReturnsAsync(accounts);

            // Act
            await _service.AssignAccounts(transactions);

            // Assert
            transactions[0].FinancialAccountId.Should().Be(1);
            transactions[1].FinancialAccountId.Should().Be(2);
        }

        [Fact]
        public async Task AssignAccount_WhenCreateIfMissingIsTrue_ShouldCreateAccountIfMissing()
        {
            // Arrange
            var transactions = new List<Transaction>
            {
                new Transaction { FinancialAccountName = "Account1" }
            };

            _accountServiceMock.Setup(s => s.GetFinancialAccounts(It.IsAny<Func<IQueryable<EFinancialAccount>, IQueryable<EFinancialAccount>>>()))
                .ReturnsAsync(new List<FinancialAccount>());

            _accountServiceMock.Setup(s => s.AddFinancialAccount(It.IsAny<FinancialAccount>()))
                .ReturnsAsync(1);

            // Act
            await _service.AssignAccount(transactions, true);

            // Assert
            transactions[0].FinancialAccountId.Should().Be(1);
        }

        [Fact]
        public async Task AssignCategories_WhenCalled_ShouldAssignCategories()
        {
            // Arrange
            var transactions = new List<Transaction>
            {
                new Transaction { Description = "Test transaction" }
            };

            var categories = new List<Category>
            {
                new Category { Id = 1, Priority = 1, Tags = new List<string> { "test" } }
            };

            var defaultCategory = new Category { Id = 2 };

            _categoryServiceMock.Setup(s => s.GetCategories()).ReturnsAsync(categories);
            _categoryServiceMock.Setup(s => s.GetDefaultCategory()).ReturnsAsync(defaultCategory);

            // Act
            await _service.AssignCategories(transactions);

            // Assert
            transactions[0].CategoryId.Should().Be(1);
        }

        [Fact]
        public async Task AssignCategories_WhenNoMatchingCategory_ShouldAssignDefaultCategory()
        {
            // Arrange
            var transactions = new List<Transaction>
            {
                new Transaction { Description = "Test transaction" }
            };

            var categories = new List<Category>
            {
                new Category { Id = 1, Priority = 1, Tags = new List<string> { "nomatch" } }
            };

            var defaultCategory = new Category { Id = 2 };

            _categoryServiceMock.Setup(s => s.GetCategories()).ReturnsAsync(categories);
            _categoryServiceMock.Setup(s => s.GetDefaultCategory()).ReturnsAsync(defaultCategory);

            // Act
            await _service.AssignCategories(transactions);

            // Assert
            transactions[0].CategoryId.Should().Be(2);
        }

        [Fact]
        public void GetTransactions_WhenCalled_ShouldReturnTransactions()
        {
            // Arrange
            var transactions = new List<Transaction>
            {
                new Transaction { Description = "Test transaction" }
            };

            _parserMock.Setup(p => p.ParseTransactions()).Returns(transactions);

            // Act
            var result = _service.GetTransactions();

            // Assert
            result.Should().BeEquivalentTo(transactions);
        }

        [Fact]
        public async Task UpdateAvailabilities_WhenCalled_ShouldUpdateAvailabilities()
        {
            // Arrange
            var transactions = new List<Transaction>
            {
                new Transaction { Description = "Test transaction" }
            };

            // Act
            await _service.UpdateAvailabilities(transactions);

            // Assert
            _accountServiceMock.Verify(s => s.CalculateAvailabilities(transactions), Times.Once);
        }

        [Fact]
        public async Task SaveTransactions_WhenCalled_ShouldSaveTransactions()
        {
            // Arrange
            var transactions = new List<Transaction>
            {
                new Transaction { Description = "Test transaction" }
            };

            // Act
            await _service.SaveTransactions(transactions);

            // Assert
            _transactionServiceMock.Verify(s => s.AddTransactions(transactions), Times.Once);
        }
    }
}

