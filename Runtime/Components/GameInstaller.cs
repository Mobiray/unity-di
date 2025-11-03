using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mobiray.DI
{
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

            _isInitialized = true;

            RegisterDependencies();
        }

        /// <summary>
        /// Переопредели этот метод для регистрации зависимостей
        /// </summary>
        protected abstract void RegisterDependencies();

        /// <summary>
        /// Инжект всех объектов на сцене
        /// </summary>
        private void InjectAll()
        {
            UnityDI.InjectAllInScene();
        }

        /// <summary>
        /// Вызывается при загрузке новой сцены
        /// </summary>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log($"[GameInstaller] New scene loaded: {scene.name}, injecting dependencies...");
            InjectAll();
        }

        /// <summary>
        /// Регистрация синглтона с интерфейсом
        /// </summary>
        protected void Register<TInterface, TImplementation>() where TImplementation : TInterface
        {
            UnityDI.Register<TInterface, TImplementation>();
        }

        /// <summary>
        /// Регистрация синглтона без интерфейса
        /// </summary>
        protected void RegisterSingleton<TImplementation>()
        {
            UnityDI.Register<TImplementation>();
        }

        /// <summary>
        /// Регистрация готового инстанса
        /// </summary>
        protected void RegisterInstance<TInterface>(TInterface instance)
        {
            UnityDI.RegisterInstance(instance);
        }

        /// <summary>
        /// Ручной инжект конкретной сцены
        /// </summary>
        public void InjectScene(Scene scene)
        {
            UnityDI.InjectScene(scene);
        }

        /// <summary>
        /// Ручной инжект конкретного GameObject
        /// </summary>
        public void InjectGameObject(GameObject gameObject)
        {
            UnityDI.InjectGameObject(gameObject);
        }

        public void InjectCurrentScene()
        {
            InjectAll();
        }
    }
}