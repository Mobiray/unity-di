# UnityDI - Minimal Dependency Injection Framework

A lightweight DI tool for Unity that supports singleton services with constructor injection (for registered services) and method injection (for MonoBehaviours).

## Installation

1. Add to your `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.mobiray.unity_di": "https://github.com/mobiray/unity-di.git"
  }
}
```

## Usage

1. Create a subclass of `GameInstaller` and register dependencies:

```csharp
using Mobiray.DI;

public class MyInstaller : GameInstaller
{
    protected override void RegisterDependencies()
    {
        // Interface -> implementation
        Register<ISomeService, SomeService>();

        // Pre-created instance
        RegisterInstance<IConfig>(myConfig);
    }
}
```

2. Add the installer to a GameObject in **the first scene only** (e.g. a boot/loading scene). The installer survives scene changes (`DontDestroyOnLoad`), keeps the registrations, and injects every scene that is loaded afterwards. You do not need to add it to other scenes.

   Every service is a lazy singleton: it is created on first request and reused afterwards.

   > If you press Play from a scene that is not the first one, that scene needs an installer too. Extra installers in later scenes are harmless: they destroy themselves on startup.

3. Mark a method on a MonoBehaviour with `[Inject]`. It is called with the resolved dependencies each time the scene containing that MonoBehaviour is loaded:

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

4. Registered services can use constructor injection. Mark a public constructor with `[Inject]`; otherwise a parameterless constructor is required:

```csharp
public class SomeService : ISomeService
{
    private readonly IConfig _config;

    [Inject]
    public SomeService(IConfig config)
    {
        _config = config;
    }
}
```

## Installer settings

- **Show Debug Logs** (default: on) - toggles DI log output.

## Notes

- Injection happens when a scene finishes loading, after `Awake`/`OnEnable` and before `Start`. Use injected dependencies in `Start` or later, not in `Awake`.
- Only the active scene is injected on load. Additively loaded scenes and objects instantiated at runtime are not injected automatically.
- Objects moved to `DontDestroyOnLoad` are not in the active scene, so they are not injected on later scene loads.
