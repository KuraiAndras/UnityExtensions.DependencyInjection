# UnityExtensions.DependencyInjection

A simple library to use the Microsoft.Extensions.DependencyInjection library.

Unity supports this package up to 2.2.0. The dlls are included in the package

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

Supported injection methods: Field, Property, Method. Injection happens in this order.

The Injector.Startup is the first Awake that is called when the scene starts up.

Injecting into prefabs:
```c#
// Get a prefab that contains a script which needs injection.
GameObject prefab = ;
// IGameObjectInjector is a service added by default to Services
IGameObjectInjector gameObjectInjector = ;

var instance = GameObject.Instantiate(prefab);

gameObjectInjector.InjectIntoGameObject(instance);

```

## Disposables

 - An IServiceScope is created for every script found in a GameObject.
 - A DestoryDetector script is added to the containing GameObject every time when a script gets injected. When the GameObject containing the script is destroyed the scope associated with the injection target is disposed.