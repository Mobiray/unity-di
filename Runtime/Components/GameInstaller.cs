using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mobiray.DI
{
    [DefaultExecutionOrder(-999)]
    public abstract class GameInstaller : MonoBehaviour
    {
        [Header("Installer Settings")] [SerializeField]
        protected bool _dontDestroyOnLoad = true;

        private static bool _isInitialized;

        protected virtual void Awake()
        {
            if (_isInitialized)
            {
                Destroy(gameObject);
                return;
            }

            Initialize();
        }

        protected virtual void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        protected virtual void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void Initialize()
        {
            if (_dontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);

            RegisterDependencies();

            _isInitialized = true;
        }

        /// <summary>
        /// Override this method to register dependencies
        /// </summary>
        protected abstract void RegisterDependencies();

        /// <summary>
        /// Inject all objects in the current scene
        /// </summary>
        internal void InjectAll()
        {
            UnityDI.InjectAllInScene();
        }

        /// <summary>
        /// Called when a new scene is loaded
        /// </summary>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log($"[GameInstaller] New scene loaded: {scene.name}, injecting dependencies...");
            InjectAll();
        }

        /// <summary>
        /// Register a singleton with an interface
        /// </summary>
        protected void Register<TInterface, TImplementation>() where TImplementation : TInterface
        {
            UnityDI.Register<TInterface, TImplementation>();
        }

        /// <summary>
        /// Register a singleton without an interface
        /// </summary>
        protected void RegisterSingleton<TImplementation>()
        {
            UnityDI.Register<TImplementation>();
        }

        /// <summary>
        /// Register a pre-created instance
        /// </summary>
        protected void RegisterInstance<TInterface>(TInterface instance)
        {
            UnityDI.RegisterInstance(instance);
        }

        /// <summary>
        /// Manual injection for a specific scene
        /// </summary>
        public void InjectScene(Scene scene)
        {
            UnityDI.InjectScene(scene);
        }

        /// <summary>
        /// Manual injection for a specific GameObject
        /// </summary>
        public void InjectGameObject(GameObject gameObject)
        {
            UnityDI.InjectGameObject(gameObject);
        }

        /// <summary>
        /// Manual injection for the current scene
        /// </summary>
        public void InjectCurrentScene()
        {
            InjectAll();
        }
    }
}