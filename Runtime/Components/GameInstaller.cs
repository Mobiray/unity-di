using UnityEngine;

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
                _isInitialized = false;
                return;
            }

            Initialize();
        }

        private void Initialize()
        {
            if (_isInitialized)
                UnityDI.Clear();

            if (_dontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);

            _isInitialized = true;

            RegisterDependencies();
            InjectAll();
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
    }
}