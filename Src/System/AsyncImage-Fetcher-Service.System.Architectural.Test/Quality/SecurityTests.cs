using AsyncImage_Fetcher_Service.System.Architectural.Test.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;

namespace AsyncImage_Fetcher_Service.System.Architectural.Test.Quality;

public class SecurityTests : BaseArchTest
{
    [Fact]
    public void Controllers_Should_Have_Authorize_Or_AllowAnonymous_Attribute()
    {
        var controllerTypes = Types.InAssembly(AdaptersAssembly)
            .That()
            .Inherit(typeof(Microsoft.AspNetCore.Mvc.ControllerBase))
            .And().AreNotAbstract()
            .GetTypes();

        var failingTypes = new List<string>();

        foreach (var type in controllerTypes)
        {
            var hasAuthorizeAttribute =
                type.GetCustomAttributes(typeof(AuthorizeAttribute), true).Any() ||
                type.Assembly.GetCustomAttributes(typeof(AuthorizeAttribute), true).Any();

            var hasAllowAnonymousAttribute =
                type.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any();

            if (!hasAuthorizeAttribute && !hasAllowAnonymousAttribute)
            {
                failingTypes.Add(type.FullName ?? type.Name);
            }
        }

        failingTypes.Should().BeEmpty(because: "Controllers should have [Authorize] or [AllowAnonymous] attribute");
    }

    [Fact]
    public void Validators_Should_Reside_In_Logic_Or_Adapters()
    {
        var forbiddenAssemblies = new[] { RulesImagesAssembly, DriversDataAssembly };
        var validatorTypesInForbiddenLayers = Types.InAssemblies(forbiddenAssemblies)
            .That()
            .Inherit(typeof(AbstractValidator<>))
            .GetTypes();

        validatorTypesInForbiddenLayers.Should().BeEmpty(
            because: "Validators should reside in Logic or Adapters layers, not Rules or Drivers");
    }
}