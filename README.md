# UnityDI - Minimal Dependency Injection Framework

A lightweight DI framework for Unity that supports singleton services with constructor and method injection.

## Installation

1. Add to your `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.mobiray.unity_di": "https://github.com/mobiray/unity-di.git"
  }
}
```

2. Create subclass of GameInstaller and register dependencies
```csharp
public class MyInstaller : GameInstaller
{
    protected override void RegisterDependencies(Container container)
    {
        container.Register<ISomeService, SomeService>();
        container.RegisterSingleton<GameManager>();
    }
}
```

3. Add this Installer to the first Scene

4. Use [Inject] annotation
```csharp
public class Player : MonoBehaviour
{
    private ISomeService _service;
    
    [Inject]
    public void Construct(ISomeService service)
    {
        _service = service;
    }
}
```