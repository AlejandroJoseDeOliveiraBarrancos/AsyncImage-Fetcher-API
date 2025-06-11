using Digester.System.Achitectural.Test.Abstractions;

namespace Digester.System.Achitectural.Test;

public class FrameworkDependencyTests : BaseArchTest
{
    [Fact]
    public void Rules_Layer_Should_Not_Depend_On_Frameworks()
    {
        var types = Types.InAssembly(RulesUtilitiesAssembly);
        var forbiddenFrameworkNamespaces = new[] {
            "Microsoft.EntityFrameworkCore",
            "Microsoft.AspNetCore",
            "Serilog",
            "AutoMapper"
        };

        var result = types.ShouldNot().HaveDependencyOnAny(forbiddenFrameworkNamespaces).GetResult();

        result.IsSuccessful.Should().BeTrue(
            because: $"Rules layer should not depend on frameworks like {string.Join(", ", forbiddenFrameworkNamespaces)}" + GetFailingTypes(result));
    }

    [Fact]
    public void Logic_Layer_Should_Not_Depend_On_Infrastructure_Frameworks()
    {
        var types = Types.InAssembly(LogicUtilitiesAssembly);
        var forbiddenFrameworkNamespaces = new[] {
            "Microsoft.EntityFrameworkCore",
            "Microsoft.AspNetCore.Mvc"
        };

        var result = types.ShouldNot().HaveDependencyOnAny(forbiddenFrameworkNamespaces).GetResult();

        result.IsSuccessful.Should().BeTrue(
            because: $"Logic layer should not depend on specific infrastructure/web frameworks like {string.Join(", ", forbiddenFrameworkNamespaces)}" + GetFailingTypes(result));
    }
}