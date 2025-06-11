using AsyncImage_Fetcher_Service.System.Architectural.Test.Abstractions;

namespace AsyncImage_Fetcher_Service.System.Architectural.Test;

public class NamingConventionTests : BaseArchTest
{
    [Fact]
    public void Controllers_Should_Reside_In_Adapters_Api_Controllers_Namespace()
    {
        var types = Types.InAssembly(AdaptersAssembly)
            .That()
            .Inherit(typeof(Microsoft.AspNetCore.Mvc.ControllerBase))
            .Or().HaveNameEndingWith("Controller");

        var result = types
            .Should()
            .ResideInNamespaceStartingWith($"{AdaptersNamespace}.Api.Controllers")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(because: "Controllers should reside in the *.Api.Controllers namespace" + GetFailingTypes(result));
    }

    [Fact]
    public void Interfaces_Should_Start_With_I()
    {
        var types = Types.InAssemblies(new[] {
            AdaptersAssembly, DriversDataAssembly,
            LogicAssembly, RulesImagesAssembly
        })
        .That()
        .AreInterfaces();

        var result = types
            .Should()
            .HaveNameStartingWith("I")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(because: "Interfaces should start with 'I'" + GetFailingTypes(result));
    }

    [Fact]
    public void Abstractions_Should_Reside_In_Correct_Namespaces()
    {
        var types = Types.InAssemblies(new[] {
            AdaptersAssembly, DriversDataAssembly,
            LogicAssembly, RulesImagesAssembly
        })
        .That()
        .AreInterfaces();

        string[] allowedNamespaces = {
            $"{RulesNamespace}.Images.Abstractions",
            $"{LogicNamespace}.Abstractions",
            $"{DriversNamespace}.Abstractions",
            $"{AdaptersNamespace}.Abstractions"
          };

        var condition = types.Should().ResideInNamespace(allowedNamespaces[0]);
        foreach (var ns in allowedNamespaces.Skip(1))
        {
            condition = condition.Or().ResideInNamespace(ns);
        }
        var result = condition.GetResult();

        result.IsSuccessful.Should().BeTrue(
            because: $"Interfaces (Abstractions) must reside only in *.Abstractions namespaces:\n{string.Join(", ", allowedNamespaces)}" +
                     GetFailingTypes(result));
    }
}

