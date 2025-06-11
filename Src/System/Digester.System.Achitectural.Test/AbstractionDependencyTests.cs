using Microsoft.AspNetCore.Mvc;

namespace Digester.System.Achitectural.Test;

public class AbstractionDependencyTests : BaseArchTest
{
    [Fact(Skip = "Temporarily skipped due to pipeline reasons")]
    public void Adapters_Should_Not_Depend_On_Concrete_Driver_Or_Logic_Classes()
    {
        var adapterTypes = Types.InAssembly(AdaptersAssembly);
        var forbiddenDependencies = Types.InAssemblies(new[] { DriversDataAssembly, DriversUtilitiesAssembly, LogicUtilitiesAssembly })
            .That()
            .AreClasses().And().AreNotAbstract()
            .And().DoNotHaveNameEndingWith("Exception")
            .And().DoNotResideInNamespaceContaining(".Models")
            .And().DoNotResideInNamespaceContaining(".Events")
            .GetTypes();

        var forbiddenTypeNames = forbiddenDependencies.Select(t => t.FullName).Where(n => n != null).ToArray();
        if (!forbiddenTypeNames.Any()) { Assert.Fail("Setup failure: No forbidden concrete types found to check against in Drivers/Logic."); return; }

        var result = adapterTypes.ShouldNot().HaveDependencyOnAny(forbiddenTypeNames!).GetResult();

        result.IsSuccessful.Should().BeTrue(because: "Adapters should only depend on abstractions from lower layers" + GetFailingTypes(result));
    }

    [Fact]
    public void Drivers_Should_Not_Depend_On_Concrete_Logic_Classes()
    {
        var driverTypes = Types.InAssemblies(new[] { DriversDataAssembly, DriversUtilitiesAssembly });
        var forbiddenLogicDependencies = Types.InAssembly(LogicUtilitiesAssembly)
            .That()
            .AreClasses().And().AreNotAbstract()
            .And().DoNotHaveNameEndingWith("Exception")
            .And().DoNotResideInNamespaceContaining(".Models")
            .GetTypes();

        var forbiddenTypeNames = forbiddenLogicDependencies.Select(t => t.FullName).Where(n => n != null).ToArray();
        if (!forbiddenTypeNames.Any()) { return; }

        var result = driverTypes.ShouldNot().HaveDependencyOnAny(forbiddenTypeNames!).GetResult();

        result.IsSuccessful.Should().BeTrue(because: "Drivers should only depend on abstractions from Logic layer" + GetFailingTypes(result));
    }

    [Fact]
    public void Controllers_Should_Not_Depend_Directly_On_Domain_Models()
    {
        var controllerTypes = Types.InAssembly(AdaptersAssembly)
            .That()
            .HaveNameEndingWith("Controller")
            .Or().Inherit(typeof(ControllerBase));

        var domainModelNamespace = "Digester.Rules.Models";

        var result = controllerTypes.ShouldNot().HaveDependencyOn(domainModelNamespace).GetResult();

        result.IsSuccessful.Should().BeTrue(
            because: $"Controllers should use DTOs/ViewModels, not depend directly on Domain Models ({domainModelNamespace})" + GetFailingTypes(result));
    }
}