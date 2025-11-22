using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mobiray.DI
{
    internal static class UnityDI
    {
        private static DIContainer _container;
        internal static DIContainer Container => _container ??= new DIContainer();

        internal static void SetDebugLogs(bool showLogs) => Container.ShowDebugLogs = showLogs;

        internal static void Register<TInterface, TImplementation>() where TImplementation : TInterface
            => Container.Register<TInterface, TImplementation>();

        internal static void Register<TImplementation>()
            => Container.Register<TImplementation>();

        internal static void RegisterInstance<TInterface>(TInterface instance)
            => Container.RegisterInstance(instance);

        internal static T Resolve<T>()
            => Container.Resolve<T>();

        internal static bool Inject(object target)
            => Container.Inject(target);

        internal static void Clear()
            => Container.Clear();

        internal static void InjectAllInScene()
        {
            var scene = SceneManager.GetActiveScene();
            InjectScene(scene);
        }

        internal static void InjectScene(Scene scene)
        {
            var rootObjects = scene.GetRootGameObjects();
            
            foreach (var rootObject in rootObjects)
            {
                InjectGameObject(rootObject);
            }

            if (Container.ShowDebugLogs) Debug.Log($"[DI] Injected {rootObjects.Length} root objects in scene: {scene.name}");
        }

        internal static void InjectGameObject(GameObject gameObject)
        {
            var behaviours = gameObject.GetComponentsInChildren<MonoBehaviour>(true);
            var injectedCount = 0;
            
            foreach (var behaviour in behaviours)
            {
                if (behaviour != null && Inject(behaviour))
                    injectedCount++;
            }

            if (Container.ShowDebugLogs) Debug.Log($"[DI] Injected {injectedCount} behaviours in: {gameObject.name}");
        }
    }
}