using AsyncImage_Fetcher_Service.System.Architectural.Test.Abstractions;

namespace AsyncImage_Fetcher_Service.System.Architectural.Test;

public class SealedClassTests : BaseArchTest
{
    [Fact]
    public void Controllers_Should_Be_Sealed()
    {
        var types = Types.InAssembly(AdaptersAssembly)
            .That()
            .Inherit(typeof(Microsoft.AspNetCore.Mvc.ControllerBase))
            .Or().HaveNameEndingWith("Controller")
            .And().AreNotAbstract();

        var result = types.Should().BeSealed().GetResult();

        result.IsSuccessful.Should().BeTrue(because: "Controllers should be sealed" + GetFailingTypes(result));
    }

    [Fact]
    public void CommandQueryHandlers_Should_Be_Sealed()
    {
        var types = Types.InAssembly(LogicAssembly)
            .That()
            .HaveNameEndingWith("Handler")
            .And().AreClasses()
            .And().AreNotAbstract();

        var result = types.Should().BeSealed().GetResult();

        result.IsSuccessful.Should().BeTrue(because: "Command/Query Handlers should be sealed" + GetFailingTypes(result));
    }
}