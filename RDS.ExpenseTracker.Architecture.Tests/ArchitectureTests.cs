using FluentAssertions;
using NetArchTest.Rules;
using RDS.ExpenseTracker.Api.Architecture;
using RDS.ExpenseTracker.Business.Architecture;
using RDS.ExpenseTracker.DataAccess.Architecture;

namespace RDS.ExpenseTracker.Architecture.Tests
{
    public class ArchitectureTests
    {

        [Fact]
        public void Presentation_ShouldNotReference_Data()
        {
            var result = Types
                .InAssembly(typeof(PresentationAssemblyMarker).Assembly)
                .ShouldNot()
                .HaveDependencyOn(typeof(DataAccessAssemblyMarker).Assembly.GetName().Name)
                .GetResult();

            result.IsSuccessful.Should().BeTrue("Presentation should not have direct dependencies on Data");
        }

        [Theory]
        [InlineData(typeof(PresentationAssemblyMarker))]
        [InlineData(typeof(DataAccessAssemblyMarker))]
        [InlineData(typeof(BusinessAssemblyMarker))]
        public void All_Classes_Should_Use_Correct_Namespace_Prefix(object assemblyMarker)
        {
            var assembly = assemblyMarker.GetType().Assembly;
            var expectedNamespacePrefix = assembly.GetName().Name;

            var failingTypes = Types.InAssembly(assembly)
                .That()
                .AreClasses()
                .GetTypes()
                .Where(t => t.Namespace != null && !t.Namespace.StartsWith(expectedNamespacePrefix))
                .ToList();

            failingTypes.Should().BeEmpty(
                $"All classes in {expectedNamespacePrefix} should have a namespace which starts with '{expectedNamespacePrefix}', but the followings have not:\n" +
                string.Join("\n", failingTypes.Select(t => t.FullName))
            );
        }
    }
}
