# UnityExtensions.DependencyInjection
[![openupm](https://img.shields.io/npm/v/com.unityextensions.dependencyinjection?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.unityextensions.dependencyinjection/)

A simple library to use the Microsoft.Extensions.DependencyInjection library in Unity3D.

The Microsoft.Extensions.DependencyInjection 2.0.0 dlls are included in the package

## Why?

If you use .net standard libraries in your unity project, and they already rely on dependency injection then using them in unity is rather cumbersome.

One option would be to use an already existing DI framework in unity, and translate your injection into that container, but it sometimes is not viable. Also most libraries have different opinions on DI. By relying on the Microsoft provided package all your libraries can have the same abstraction for DI.

## Resources on DI

 - [Microsoft Docs](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.2)
 - [MSDN Magazine article](https://docs.microsoft.com/en-us/archive/msdn-magazine/2016/june/essential-net-dependency-injection-with-net-core)
 - [Wikipedia DI](https://en.wikipedia.org/wiki/Dependency_injection)

## Usage

### Initialize
Create a class that inherits from Injector:
```c#
// Customize script execution order, so Awake is called first in you scene
// Usually -999 works nicely
[DefaultExecutionOrder(-999)]
public sealed class ExampleInjector : Injector
{
    // Override CreateServiceProvider to add service registrations
    protected override IServiceProvider CreateServiceProvider()
    {
        // Use the usual IServiceCollection methods
        Services.AddTransient<IExampleService, ExampleService>();

        // Resolve scripts already in the scene with FindObjectOfType()
        Services.AddSingleton<MonoBehaviourService>(_ => GameObject.FindObjectOfType<MonoBehaviourService>());

        // Either:

        // Return a built ServiceProvider
        return Services.BuildServiceProvider();

        // Or:

        // Call base. Base does: Services.BuildServiceProvider()
        return base.CreateServiceProvider();
    }

    // Override Awake to customize startup
    // Base by default does: GetComponent<SceneInjector>().InitializeScene(CreateServiceProvider())
    protected override void Awake()
    {
        // Put your startup logic here.
        base.Awake();
        // Put startup logic needing initialized scene here
    }
}
```

Add this script to any one GameObject in your scene.

### Usage in MonoBehavior

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

Supported injection methods for InjectAttribute: Field, Property, Method. Injection happens in this order. **Constructor injection does not work.**

### Usage in other classes

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

### Usage in Prefabs

Injecting into prefabs:

```c#
// Get a prefab that contains a script which needs injection.
GameObject prefab = ;
// IGameObjectInjector and ISceneInjector are services added by default to Services
IGameObjectFactory gameObjectFactory = ;
ISceneInjector sceneInjector = ;

// Either:

// Instantiate the usual way
var instance = GameObject.Instantiate(prefab);
// Inject into freshly created GameObject
sceneInjector.InjectIntoGameObject(instance);

// Or:

// Use IGameObjectFactory which wraps GameObject.Instantiate(...) methods
var instance = gameObjectFactory.Instantiate(prefab); // Prefab is created and injected
```
You don't have to call InjectIntoGameObject on prefab children. When InjectIntoGameObject is called all the scripts on the game object and it's children which have the InjectAttribute gets injected.

## Scopes, Disposables

 - An IServiceScope is created for every script found in a GameObject.
 - Thus each MonoBehavior injected has it's own scope (Scoped lifetime services start from here).
 - A DestroyDetector script is added to every GameObject that receives injection. When the game object is destroyed, the DestroyDetector disposes of all the scopes that got created for that specific game object.
 - Thus if you create a prefab, destroy one of it's children then only the scopes associated with that child are disposed.
 - DestroyDetector is internal, and is hidden in the Inspector.
 - Destroying the game object holding the SceneInjector disposes of the ServiceProvider

## Options

You can customize some behavior of the SceneInjector by providing an action to configure the options when calling SceneInjector.InitializeScene
```c#
    [DefaultExecutionOrder(-999)]
    public sealed class ExampleInjector : Injector
    {
        protected override IServiceProvider CreateServiceProvider()
        {
            Services.AddExampleInjection();

            return base.CreateServiceProvider();
        }

        protected override void Awake()
        {
            SceneInjector.InitializeScene(
                CreateServiceProvider(),
                options =>
                {
                    options.DontDestroyOnLoad = true;
                    options.UseCaching = true;
                });
        }
    }
```
Current options:

| Name | Description | Default value|
|---|---|---|
| UseCaching | During injection cache the fields, properties, methods needing injection | True|
| DontDestroyOnLoad | Calls GameObject.DontDestroyOnLoad(SceneInjector) during initialization. This prevents the game object from being destroyed | True|

## Notes
  - To see sample usage check out tests and test scenes
  - Pull requests are welcome!