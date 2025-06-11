namespace Digester.System.Achitectural.Test.Abstractions;

/// <summary>
/// Base class for architectural tests, providing common assembly references,
/// namespace constants, and helper methods.
/// </summary>
public abstract class BaseArchTest
{
    /// <summary>Assembly for Adapters layer (Api project).</summary>
    protected static readonly Assembly AdaptersAssembly = typeof(Digester.Adapters.Api.Startup).Assembly;

    /// <summary>Assembly for Drivers layer (Data project).</summary>
    protected static readonly Assembly DriversDataAssembly = typeof(Digester.Drivers.Data.DependencyInjection).Assembly;

    /// <summary>Assembly for Drivers layer (Utilities project).</summary>
    protected static readonly Assembly DriversUtilitiesAssembly = typeof(Digester.Drivers.Utilities.Logging.SerilogAppLogger<>).Assembly;

    /// <summary>Assembly for Logic layer (Utilities project).</summary>
    protected static readonly Assembly LogicUtilitiesAssembly = typeof(Digester.Logic.Utilities.DependencyInjection).Assembly;

    /// <summary>Assembly for Rules layer (Utilities project).</summary>
    protected static readonly Assembly RulesUtilitiesAssembly = typeof(RuleProcessingSettings).Assembly;

    protected const string AdaptersNamespace = "Digester.Adapters";
    protected const string DriversNamespace = "Digester.Drivers";
    protected const string LogicNamespace = "Digester.Logic";
    protected const string RulesNamespace = "Digester.Rules";

    /// <summary>
    /// Helper method to format the assertion message with failing types for FluentAssertions.
    /// </summary>
    /// <param name="result">The result of the NetArchTest rule.</param>
    /// <returns>A formatted string of failing types, or an empty string if successful.</returns>
    protected static string GetFailingTypes(TestResult result)
    {
        return result.IsSuccessful ? string.Empty :
               $"\nFailing types: {string.Join("\n", result.FailingTypes?.Select(t => t.FullName) ?? Enumerable.Empty<string>())}";
    }
}