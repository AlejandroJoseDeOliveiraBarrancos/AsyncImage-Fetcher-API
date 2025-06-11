namespace Digester.System.Achitectural.Test;

public class LayeringTests : BaseArchTest
{
    [Fact]
    public void Rules_Should_Not_Depend_On_Other_Layers()
    {
        var types = Types.InAssembly(RulesUtilitiesAssembly);
        var forbiddenNamespaces = new[] { AdaptersNamespace, DriversNamespace, LogicNamespace };

        var result = types.ShouldNot().HaveDependencyOnAny(forbiddenNamespaces).GetResult();

        result.IsSuccessful.Should().BeTrue(because: "Rules layer should not depend on Adapters, Drivers, or Logic" + GetFailingTypes(result));
    }

    [Fact]
    public void Logic_Should_Not_Depend_On_Adapters_Or_Drivers()
    {
        var types = Types.InAssembly(LogicUtilitiesAssembly);
        var forbiddenNamespaces = new[] { AdaptersNamespace, DriversNamespace };

        var result = types.ShouldNot().HaveDependencyOnAny(forbiddenNamespaces).GetResult();

        result.IsSuccessful.Should().BeTrue(because: "Logic layer should not depend on Adapters or Drivers" + GetFailingTypes(result));
    }

    [Fact]
    public void Drivers_Should_Not_Depend_On_Adapters()
    {
        var types = Types.InAssemblies(new[] { DriversDataAssembly, DriversUtilitiesAssembly });
        var forbiddenNamespaces = new[] { AdaptersNamespace };

        var result = types.ShouldNot().HaveDependencyOnAny(forbiddenNamespaces).GetResult();

        result.IsSuccessful.Should().BeTrue(because: "Drivers layer should not depend on Adapters" + GetFailingTypes(result));
    }
}