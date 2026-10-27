using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mobiray.DI
{
    [DefaultExecutionOrder(-999)]
    public abstract class GameInstaller : MonoBehaviour
    {
        #region Fields

        [Header("Installer Settings")] [SerializeField]
        private bool _showDebugLogs = true;

        private static bool _isInitialized;

        #endregion

        #region Unity Lifecycle

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

        #endregion

        #region Registration

        /// <summary>
        /// Override this method to register dependencies
        /// </summary>
        protected abstract void RegisterDependencies();

        /// <summary>
        /// Register a singleton with an interface
        /// </summary>
        protected void Register<TInterface, TImplementation>() where TImplementation : TInterface
        {
            UnityDI.Register<TInterface, TImplementation>();
        }

        /// <summary>
        /// Register a pre-created instance
        /// </summary>
        protected void RegisterInstance<TInterface>(TInterface instance)
        {
            UnityDI.RegisterInstance(instance);
        }

        #endregion

        #region Private Methods

        private void Initialize()
        {
            DontDestroyOnLoad(gameObject);

            UnityDI.SetDebugLogs(_showDebugLogs);
            RegisterDependencies();

            _isInitialized = true;
        }

        /// <summary>
        /// Called when a new scene is loaded
        /// </summary>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if(_showDebugLogs) Debug.Log($"[GameInstaller] New scene loaded: {scene.name}, injecting dependencies...");
            UnityDI.InjectAllInScene();
        }

        #endregion
    }
}
