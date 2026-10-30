using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mobiray.DI
{
    internal static class UnityDI
    {
        #region Fields

        private static DIContainer _container;

        #endregion

        #region Properties

        private static DIContainer Container => _container ??= new DIContainer();

        #endregion

        #region Lifecycle

        internal static void Reset() => _container = null;

        #endregion

        #region Settings

        internal static void SetDebugLogs(bool showLogs) => Container.ShowDebugLogs = showLogs;

        #endregion

        #region Registration

        internal static void Register<TInterface, TImplementation>() where TImplementation : TInterface
            => Container.Register<TInterface, TImplementation>();

        internal static void RegisterInstance<TInterface>(TInterface instance)
            => Container.RegisterInstance(instance);

        #endregion

        #region Injection

        internal static bool Inject(object target)
            => Container.Inject(target);

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

        #endregion
    }
}
