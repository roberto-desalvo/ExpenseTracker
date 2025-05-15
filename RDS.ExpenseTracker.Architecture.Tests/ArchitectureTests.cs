namespace RDS.ExpenseTracker.Architecture.Tests
{
    public class ArchitectureTests
    {

        [Fact]
        public void Presentation_ShouldNotReference_Data()
        {
            var result = Types
                .InAssembly(typeof(PresentationNamespaceMarker).Assembly)
                .ShouldNot()
                .HaveDependencyOn("YourApp.Data")
                .GetResult();

            Assert.True(result.IsSuccessful, "Presentation layer should not depend on Data layer.");
        }
    }
}
