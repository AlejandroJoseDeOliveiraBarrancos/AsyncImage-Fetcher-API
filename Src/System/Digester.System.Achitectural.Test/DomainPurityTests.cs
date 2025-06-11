using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digester.System.Achitectural.Test;

public class DomainPurityTests : BaseArchTest
{
    [Fact]
    public void DomainModels_Should_Not_Have_Persistence_Attributes()
    {
        var domainModelTypes = Types.InAssembly(RulesUtilitiesAssembly)
            .That().ResideInNamespace("Digester.Rules.Models").GetTypes();

        var forbiddenAttributeTypes = new[] { typeof(TableAttribute), typeof(KeyAttribute) };
        var failingTypesMessages = new List<string>();

        foreach (var modelType in domainModelTypes)
        {
            foreach (var attrType in forbiddenAttributeTypes)
            {
                if (modelType.GetCustomAttributes(attrType, true).Any())
                {
                    failingTypesMessages.Add($"{modelType.FullName} has [{attrType.Name}]");
                    break;
                }
            }
        }

        failingTypesMessages.Should().BeEmpty(because: "Domain models should not have persistence attributes");
    }

    [Fact]
    public void DomainModels_Should_Be_Immutable()
    {
        var domainModelTypes = Types.InAssembly(RulesUtilitiesAssembly)
            .That().ResideInNamespace("Digester.Rules.Models")
            .And().AreClasses().GetTypes();

        var failingTypesMessages = new List<string>();

        foreach (var modelType in domainModelTypes)
        {
            var mutableFields = modelType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(f => (f.IsPublic || f.IsFamily || f.IsFamilyOrAssembly) && !f.IsInitOnly && !f.IsLiteral)
                .ToList();

            if (mutableFields.Any())
            {
                failingTypesMessages.Add($"{modelType.FullName} has mutable fields: {string.Join(", ", mutableFields.Select(f => f.Name))}");
            }
        }

        failingTypesMessages.Should().BeEmpty(because: "Domain model fields should be readonly (immutable)");
    }
}