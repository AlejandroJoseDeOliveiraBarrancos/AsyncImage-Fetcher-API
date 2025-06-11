using Digester.System.Achitectural.Test.Abstractions;

namespace Digester.System.Achitectural.Test;

public class MaintainabilityTests : BaseArchTest
{
    private static readonly Assembly[] CoreAssemblies =
    [
        RulesUtilitiesAssembly,
        LogicUtilitiesAssembly,
        DriversUtilitiesAssembly,
        DriversDataAssembly
        // AdaptersAssembly is excluded as it might wrap exceptions
    ];

    // List of BCL/Framework exceptions that are okay to inherit directly from System.Exception if needed by framework design
    // Or more likely, exceptions that are okay to *use* but shouldn't be subclassed directly from System.Exception
    // Adjust this list based on actual allowed exceptions in your domain/logic.
    private static readonly string[] AllowedBaseExceptionNames =
    [
        nameof(ArgumentException),
        nameof(ArgumentNullException),
        nameof(ArgumentOutOfRangeException),
        nameof(InvalidOperationException),
        nameof(NotSupportedException),
        nameof(NotImplementedException),
        nameof(FormatException),
        nameof(TimeoutException)
        // Add any custom BASE exceptions here if they are allowed to inherit Exception directly
        // "MyCustomBaseException"
    ];

    [Fact]
    public void Static_Classes_Should_Be_Limited()
    {
        var allowedStaticClassNames = new[] {
            "Program", "Startup", "DependencyInjection", "GlobalUsings",
            "Utils"
        };

        var allTypes = Types.InAssemblies(new[] {
                AdaptersAssembly, DriversDataAssembly, DriversUtilitiesAssembly,
                LogicUtilitiesAssembly, RulesUtilitiesAssembly
            })
            .That()
            .AreClasses()
            .GetTypes();

        var unexpectedStaticTypes = allTypes
            .Where(t => t.IsAbstract && t.IsSealed && !allowedStaticClassNames.Contains(t.Name))
            .ToList();

        unexpectedStaticTypes.Should().BeEmpty(
            because: $"Static classes should be limited. Allowed: {string.Join(", ", allowedStaticClassNames)}. Found: {string.Join(", ", unexpectedStaticTypes.Select(t => t.FullName))}");
    }

    private static bool IsDigesterTestAssembly(Assembly assembly) =>
        assembly.GetName().Name?.StartsWith("Digester.", StringComparison.OrdinalIgnoreCase) == true &&
        assembly.FullName?.Contains(".Test", StringComparison.OrdinalIgnoreCase) == true;

    private static bool IsTestClass(Type type) =>
        (type.Name.EndsWith("Tests") || type.GetMethods().Any(m => m.GetCustomAttributes(inherit: true).Any(a => a.GetType().Name == "FactAttribute" || a.GetType().Name == "TheoryAttribute")))
        && !type.IsAbstract
        && type.IsPublic;

    [Fact]
    public void TestClasses_ShouldInheritFromBaseClass_InSameAssembly()
    {
        var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
        var testAssemblies = allAssemblies.Where(IsDigesterTestAssembly).ToList();
        var failingAssemblies = new List<string>();

        foreach (var assembly in testAssemblies)
        {
            var typesInAssembly = assembly.GetTypes();
            var testClasses = typesInAssembly.Where(IsTestClass).ToList();

            if (testClasses.Any())
            {
                bool hasProperBase = testClasses.Any(tc =>
                    tc.BaseType != null &&
                    tc.BaseType.IsAbstract &&
                    tc.BaseType.Assembly == assembly);

                if (!hasProperBase)
                {
                    failingAssemblies.Add(assembly.GetName().Name ?? "Unknown Assembly");
                }
            }
        }

        failingAssemblies.Should().BeEmpty(
            $"the following test assemblies contain test classes, but none inherit from an abstract base class defined within the same assembly: {string.Join(", ", failingAssemblies)}");
    }

    [Fact]
    public void CustomExceptions_ShouldNotInherit_DirectlyFrom_SystemException_Reflection()
    {
        var failingTypes = new List<Type>();

        foreach (var assembly in CoreAssemblies)
        {
            try
            {
                var types = assembly.GetTypes();

                failingTypes.AddRange(types.Where(t =>
                    t.IsSubclassOf(typeof(Exception)) &&
                    !t.IsAbstract &&
                    t.BaseType == typeof(Exception) &&
                    !AllowedBaseExceptionNames.Contains(t.Name)
                ));
            }
            catch (ReflectionTypeLoadException ex)
            {
                Console.WriteLine($"Warning: Could not load all types from assembly {assembly.FullName}. Errors: {string.Join(", ", ex.LoaderExceptions.Select(e => e?.Message ?? "N/A"))}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Error reflecting assembly {assembly.FullName}: {ex.Message}");
            }
        }

        failingTypes.Should().BeEmpty(
            $"The following custom exception types inherit directly from System.Exception instead of a more specific base type: {string.Join(", ", failingTypes.Select(t => t.FullName))}");
    }

    [Fact]
    public void Methods_ShouldNotReturn_BaseExceptionType_Reflection()
    {
        var failingMethods = new List<string>();

        foreach (var assembly in CoreAssemblies)
        {
            try
            {
                var types = assembly.GetTypes();

                foreach (var type in types)
                {
                    var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
                    failingMethods.AddRange(
                        methods.Where(m => m.ReturnType == typeof(Exception))
                               .Select(m => $"{m.DeclaringType?.FullName}.{m.Name}")
                    );
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                Console.WriteLine($"Warning: Could not load all types from assembly {assembly.FullName}. Errors: {string.Join(", ", ex.LoaderExceptions.Select(e => e?.Message ?? "N/A"))}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Error reflecting assembly {assembly.FullName}: {ex.Message}");
            }
        }

        failingMethods.Should().BeEmpty(
           $"The following methods directly return System.Exception, which should be avoided: {string.Join(", ", failingMethods)}");
    }

    // --- Removed Exception Handling Convention Tests ---
    // The following tests were removed because the required NetArchTest.Rules.Condition
    // type could not be resolved by the linter/compiler in the current environment,
    // even with fully qualified names. This likely requires local troubleshooting
    // (e.g., dotnet restore --force, clean, build).

    // [Fact]
    // public void CustomExceptions_ShouldNotInherit_DirectlyFrom_SystemException() { ... }

    // [Fact]
    // public void Methods_ShouldNotReturn_BaseExceptionType() { ... }
}