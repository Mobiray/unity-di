using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mobiray.DI
{
    public static class UnityDI
    {
        // ... предыдущие методы ...

        public static void InjectAllInScene()
        {
            var scene = SceneManager.GetActiveScene();
            InjectScene(scene);
        }

        public static void InjectScene(Scene scene)
        {
            var rootObjects = scene.GetRootGameObjects();
            
            foreach (var rootObject in rootObjects)
            {
                InjectGameObject(rootObject);
            }

            Debug.Log($"[UnityDI] Injected {rootObjects.Length} root objects in scene: {scene.name}");
        }

        public static void InjectGameObject(GameObject gameObject)
        {
            var behaviours = gameObject.GetComponentsInChildren<MonoBehaviour>(true);
            var injectedCount = 0;
            
            foreach (var behaviour in behaviours)
            {
                if (behaviour != null && Instance.Inject(behaviour))
                    injectedCount++;
            }

            Debug.Log($"[UnityDI] Injected {injectedCount} behaviours in: {gameObject.name}");
        }
    }
}