using System;
using System.Collections.Generic;
using System.Reflection;

namespace Mobiray.DI
{
    public class DIContainer
    {
        private static DIContainer _instance;
        public static DIContainer Instance => _instance ??= new DIContainer();

        private readonly Dictionary<Type, object> _singletons = new();
        private readonly Dictionary<Type, Type> _registrations = new();

        public void Register<TInterface, TImplementation>() where TImplementation : TInterface
        {
            _registrations[typeof(TInterface)] = typeof(TImplementation);
        }

        public void Register<TImplementation>()
        {
            _registrations[typeof(TImplementation)] = typeof(TImplementation);
        }

        public void RegisterInstance<TInterface>(TInterface instance)
        {
            _singletons[typeof(TInterface)] = instance;
        }

        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        public object Resolve(Type type)
        {
            // Если уже есть инстанс - возвращаем его
            if (_singletons.TryGetValue(type, out var instance))
                return instance;

            // Ищем регистрацию
            if (!_registrations.TryGetValue(type, out var implementationType))
                throw new InvalidOperationException($"No registration found for {type}");

            // Создаем инстанс с инжекцией зависимостей
            instance = CreateInstance(implementationType);
            _singletons[type] = instance;
            
            return instance;
        }

        private object CreateInstance(Type type)
        {
            // Пробуем найти конструктор с [Inject]
            var constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            var injectConstructor = Array.Find(constructors, c => c.GetCustomAttribute<InjectAttribute>() != null);

            if (injectConstructor != null)
            {
                // Инжект через конструктор
                var parameters = injectConstructor.GetParameters();
                var args = new object[parameters.Length];
                
                for (int i = 0; i < parameters.Length; i++)
                    args[i] = Resolve(parameters[i].ParameterType);

                return injectConstructor.Invoke(args);
            }
            else
            {
                // Обычный конструктор
                return Activator.CreateInstance(type);
            }
        }

        public void Inject(object target)
        {
            var type = target.GetType();
            
            // Ищем методы с [Inject]
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            
            foreach (var method in methods)
            {
                if (method.GetCustomAttribute<InjectAttribute>() == null) 
                    continue;

                var parameters = method.GetParameters();
                var args = new object[parameters.Length];
                
                for (int i = 0; i < parameters.Length; i++)
                    args[i] = Resolve(parameters[i].ParameterType);

                method.Invoke(target, args);
            }
        }

        public bool Inject(object target)
        {
            try
            {
                var type = target.GetType();
        
                // Ищем методы с [Inject]
                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                var injected = false;
        
                foreach (var method in methods)
                {
                    if (method.GetCustomAttribute<InjectAttribute>() == null) 
                        continue;

                    var parameters = method.GetParameters();
                    var args = new object[parameters.Length];
            
                    for (int i = 0; i < parameters.Length; i++)
                        args[i] = Resolve(parameters[i].ParameterType);

                    method.Invoke(target, args);
                    injected = true;
                }

                return injected;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[DI] Failed to inject {target.GetType()}: {ex.Message}");
                return false;
            }
        }
        
        public void Clear()
        {
            _singletons.Clear();
            _registrations.Clear();
        }
    }
}