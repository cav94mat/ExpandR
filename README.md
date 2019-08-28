# ExpandR
A dependency-injection based plugin-system coded against
[.NET Standard 2.0](https://docs.microsoft.com/dotnet/standard/net-standard) and
[Microsoft.Extensions.DependencyInjection](https://docs.microsoft.com/aspnet/core/fundamentals/dependency-injection).

> :warning: This project is experimental, and as such caution is advised before its use, especially in production environments.

## How does it work?
The ***host*** - in general the main application - declares and _exposes_ a set of service interfaces, which can be
implemented by one or more ***plugin(s)*** (external assemblies), and/or even by the host itself - e.g. in case there is no
plugin implementing it first:

```csharp
using cav94mat.ExpandR;
namespace MyHost {
  public class Program {
    // [...]
    private static void ConfigureServices(IServiceCollection services)
    {    
        // Internal services, not exposed to plugins.
        services.AddSingleton<ISomethingInternal, SomethingInternal>(); 

        services.AddExpandR(pluggableServices =>
        {
            // Expose `IHelpPrinter` as a singleton, accepting only one implementation.
            //  If no plugin implements it, `DefaultHelpPrinter` is used.
            pluggableServices.AddSingleton<IHelpPrinter, DefaultHelpPrinter>();        
            
            // Expose `ICommand` as transient, accepting multiple implementations.
            //  At least two implementations (`EchoCommand` and` HelpCommand`) are always registered before
            //  any plugin is loaded.
            pluggableServices.AddMultiTransient<ICommand>(typeof(EchoCommand), typeof(HelpCommand));

            // Load plugins from the /plugins sub-directory
            var plugDir = new DirectoryInfo(Directory.GetCurrentDirectory()).CreateSubdirectory("plugins");
            pluggableServices.Load(plugDir);
        });
    }
  }
}
```
> :bulb: For the sake of brevity, the declarations of `ISomethingInternal`, `ICommand`, `IHelpPrinter`, and
> the relative built-in implementations `DefaultHelpPrinter`, `EchoCommand` and `HelpCommand` are ommitted, since irrelevant.

Plugins(s) are contained in external assembly and loaded at runtime via reflection, provided they have an **entry-point**
class defined via an assembly-level attribute (see the example below). When a plugin is loaded it's entry-point is
initialized and its `Setup` method is called, so that the host's dependency injection container can be further extended
with the plugin's service implementations.

It's always the host that decides whether a specific service can have multiple implementations, as well as its lifetime
(i.e. _singleton_, _scoped_ or _transient_).

```csharp
using cav94mat.ExpandR;
// ...

// Define the plugin entry-point with an assembly-level attribute:
[assembly: Entrypoint(typeof(MyPlugin.Plugin))]

namespace MyPlugin {
  // ...
  public class Plugin : IEntrypoint
  {
      // The entry-point method:
      public void Setup(IServiceCollectionExtender services)
      {
          // IHelpPrinter is to be implemented by FancyHelpPrinter, if no plugin has provided an
          //  implementation yet. Otherwise, `TryAdd` here simply returns false. On the other hand, if
          //  the alternative `Add` method was used and an implementation already exists, an
          //  exception is thrown.
          services.TryAdd<IHelpPrinter, FancyHelpPrinter>();
          // (Subsequent calls to TryAdd<IHelpPrinter, ...> will always return false.

          // Another implementation for ICommand, residing in the class MyPlugin.ConfCommand, is added.
          //  Since ICommand supports multiple implementation, `TryAdd` always returns true and `Add` is
          //  perfectly safe to be used here.
          services.Add<ICommand, ConfCommand>();
      }
  }
}
```
> :bulb: For the sake of brevity, the declarations of `FancyHelpPrinter` and `ConfCommand` are ommitted, since irrelevant.

> :bulb: The `Entrypoint` attribute and the `IEntrypoint` interface used in this example are part of ExpandR.
> It's possible to derive these types in order to create custom entry-points.
 

