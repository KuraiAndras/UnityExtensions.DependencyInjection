# UnityExtensions.DependencyInjection

A simple library to use the Microsoft.Extensions.DependencyInjection library in Unity3D.

Unity supports this package up to 2.2.0. The dlls are included in the package

## Why?

If you use .net standard libraries in your unity project, and they already rely on dependency injection then using them in unity is rather cumbersome.

One option would be to use an already existing DI framework in unity, and translate your injection into that container, but it sometimes is not viable. Also most libraries have different opinions on DI. By relying on the Microsoft provided package all your libraries can have the same abstraction for DI.

## Resources on DI

 - [Microsoft Docs](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.2)
 - [MSDN Magazine article](https://docs.microsoft.com/en-us/archive/msdn-magazine/2016/june/essential-net-dependency-injection-with-net-core)
 - [Wikipedia DI](https://en.wikipedia.org/wiki/Dependency_injection)

## Usage

Create a class that inherits from Injector:
```c#
public sealed class ExampleInjector : Injector
{
    protected override void Startup()
    {
        // Use the usual IServiceCollection methods
        Services.AddTransient<IExampleService, ExampleService>();

        // Resolve scripts already in the scene with FindObjectOfType()
        Services.AddSingleton<MonoBehaviourService>(_ => GameObject.FindObjectOfType<MonoBehaviourService>());

        // Set the ServiceProvider
        ServiceProvider = Services.BuildServiceProvider();

        // Don't forget to call base at some point
        base.Startup();
    }
}
```

Add this script to any GameObject in your scene.

Use the InjectAttribute to inject into a MonoBehavior:

```c#
public class ExampleScript : MonoBehaviour
{
    [Inject] private readonly IExampleService1 _exampleService1;
    [Inject] private IExampleService2 ExampleService2 { get; }

    private IExampleService3 _exampleService3;

    [Inject]
    private void Construct(IExampleService3 exampleService3)
    {
        _exampleService3 = exampleService3;
    }
}
```

Supported injection methods for InjectAttribute: Field, Property, Method. Injection happens in this order.

Regular classes (not inheriting from MonoBehavior) can't use the InjectAttribute. For those cases use constructor injection.

```c#
public class ExampleService : IExampleService
{
    public ExampleService(IOtherService service)
    {
        // Implementation
    }
}
```

The Injector class has a Script Execution Order of: -999 making it (and its subtypes) the first (unless other script is set bellow -999) one to be called in lifecycle events. Injector.Startup is called in the implementation of Injector.Awake.

Injecting into prefabs:
```c#
// Get a prefab that contains a script which needs injection.
GameObject prefab = ;
// IGameObjectInjector is a service added by default to Services
IGameObjectInjector gameObjectInjector = ;

// Either:

// Instantiate the usual way
var instance = GameObject.Instantiate(prefab);
// Inject into freshly created GameObject
gameObjectInjector.InjectIntoGameObject(instance);

// Or:

// Use IGameObjectInjector which wraps GameObject.Instantiate(...) methods
var instance = gameObjectInjector.Instantiate(prefab); // Prefab is created and injected
```
You don't have to call InjectIntoGameObject on prefab children. When InjectIntoGameObject is called all the scripts on the game object and it's children which have the InjectAttribute gets injected.


## Scopes, Disposables

 - An IServiceScope is created for every script found in a GameObject.
 - Thus each MonoBehavior injected has it's own scope (Scoped lifetime services start from here)
 - A DestroyDetector script is added to the containing GameObject every time when a script gets injected. When the GameObject containing the script is destroyed the scope associated with the injection target is disposed.
 - DestroyDetector is internal, and is hidden in the Inspector.
 - Example: Player GameObject has 3 scripts which all have some dependencies implementing IDisposable. 3 DestroyDetector script gets added to Player (for each injected script). When Player is destroyed all scoped and transient services which implement IDisposable are disposed.