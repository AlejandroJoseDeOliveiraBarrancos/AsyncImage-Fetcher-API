namespace AsyncImage_Fetcher_Service.System.Architectural.Test.Abstractions;

/// <summary>
/// Base class for architectural tests, providing common assembly references,
/// namespace constants, and helper methods.
/// </summary>
public abstract class BaseArchTest
{
    /// <summary>Assembly for Adapters layer (Api project).</summary>
    protected static readonly Assembly AdaptersAssembly = typeof(AsyncImage_Fetcher_Service.Adapters.DependencyInjection).Assembly;

    /// <summary>Assembly for Drivers layer (Data project).</summary>
    protected static readonly Assembly DriversDataAssembly = typeof(AsyncImage_Fetcher_Service.Drivers.DependencyInjection).Assembly;

    /// <summary>Assembly for Logic layer (Logic project).</summary>
    protected static readonly Assembly LogicAssembly = typeof(AsyncImage_Fetcher_Service.Logic.DependencyInjection).Assembly;

    /// <summary>Assembly for Rules layer (Images project).</summary>
    protected static readonly Assembly RulesImagesAssembly = typeof(AsyncImage_Fetcher_Service.Rules.Images.Image).Assembly;

    protected const string AdaptersNamespace = "AsyncImage_Fetcher_Service.Adapters";
    protected const string DriversNamespace = "AsyncImage_Fetcher_Service.Drivers";
    protected const string LogicNamespace = "AsyncImage_Fetcher_Service.Logic";
    protected const string RulesNamespace = "AsyncImage_Fetcher_Service.Rules";

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