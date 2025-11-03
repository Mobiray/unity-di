using UnityEngine;

namespace Mobiray.DI
{
    public abstract class GameInstaller : MonoBehaviour
    {
        [Header("Installer Settings")]
        [SerializeField] protected bool _global = true;
        [SerializeField] protected bool _dontDestroyOnLoad = true;
        [SerializeField] protected bool _autoInjectOnStart = true;
        [SerializeField] protected bool _injectExistingScene = true;

        private static bool _isInitialized;

        protected virtual void Awake()
        {
            if (_global && _isInitialized)
            {
                Destroy(gameObject);
                return;
            }

            Initialize();
        }

        protected virtual void Start()
        {
            if (_autoInjectOnStart)
            {
                InjectAll();
            }
        }

        private void Initialize()
        {
            // Настраиваем GameObject
            if (_global)
            {
                if (_dontDestroyOnLoad)
                    DontDestroyOnLoad(gameObject);
                
                _isInitialized = true;
            }

            // Очищаем контейнер (для перезапуска)
            if (!_global)
                UnityDI.Clear();

            // Регистрируем зависимости
            RegisterDependencies();

            // Инжектим существующие объекты на сцене
            if (_injectExistingScene)
                InjectAll();
        }

        /// <summary>
        /// Переопредели этот метод для регистрации зависимостей
        /// </summary>
        protected abstract void RegisterDependencies();

        /// <summary>
        /// Инжект всех объектов на сцене
        /// </summary>
        public void InjectAll()
        {
            if (_global)
            {
                UnityDI.InjectAllInScene();
            }
            else
            {
                UnityDI.InjectGameObject(gameObject);
            }
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
        protected void Register<TImplementation>()
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
        /// Регистрация самого себя как синглтона
        /// </summary>
        protected void RegisterSelf<T>()
        {
            UnityDI.RegisterInstance(GetComponent<T>());
        }

        [ContextMenu("Inject Now")]
        public void InjectNow()
        {
            InjectAll();
        }

        [ContextMenu("Clear Container")]
        public void ClearContainer()
        {
            UnityDI.Clear();
        }
    }
}