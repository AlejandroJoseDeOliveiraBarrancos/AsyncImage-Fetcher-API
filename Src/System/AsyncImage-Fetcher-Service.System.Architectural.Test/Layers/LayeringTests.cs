using AsyncImage_Fetcher_Service.System.Architectural.Test.Abstractions;

namespace AsyncImage_Fetcher_Service.System.Architectural.Test.Layers;

public class LayeringTests : BaseArchTest
{
    [Fact]
    public void Rules_Should_Not_Depend_On_Other_Layers()
    {
        var types = Types.InAssembly(RulesImagesAssembly);
        var forbiddenNamespaces = new[] { AdaptersNamespace, DriversNamespace, LogicNamespace };

        var result = types.ShouldNot().HaveDependencyOnAny(forbiddenNamespaces).GetResult();

        result.IsSuccessful.Should().BeTrue(because: "Rules layer should not depend on Adapters, Drivers, or Logic" + GetFailingTypes(result));
    }

    [Fact]
    public void Logic_Should_Not_Depend_On_Adapters_Or_Drivers()
    {
        var types = Types.InAssembly(LogicAssembly);
        var forbiddenNamespaces = new[] { AdaptersNamespace, DriversNamespace };

        var result = types.ShouldNot().HaveDependencyOnAny(forbiddenNamespaces).GetResult();

        result.IsSuccessful.Should().BeTrue(because: "Logic layer should not depend on Adapters or Drivers" + GetFailingTypes(result));
    }

    [Fact]
    public void Drivers_Should_Not_Depend_On_Adapters()
    {
        var types = Types.InAssembly(DriversDataAssembly);
        var forbiddenNamespaces = new[] { AdaptersNamespace };

        var result = types.ShouldNot().HaveDependencyOnAny(forbiddenNamespaces).GetResult();

        result.IsSuccessful.Should().BeTrue(because: "Drivers layer should not depend on Adapters" + GetFailingTypes(result));
    }
}