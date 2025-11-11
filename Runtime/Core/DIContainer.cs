using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Mobiray.DI
{
    internal class DIContainer
    {
        private static DIContainer _instance;
        internal static DIContainer Instance => _instance ??= new DIContainer();

        internal readonly Dictionary<Type, object> _singletons = new();
        internal readonly Dictionary<Type, Type> _registrations = new();

        internal void Register<TInterface, TImplementation>() where TImplementation : TInterface
        {
            if (_registrations.ContainsKey(typeof(TInterface)))
            {
                Debug.LogError($"[DI] Type {typeof(TInterface)} already registered");
                return;
            }

            _registrations[typeof(TInterface)] = typeof(TImplementation);
        }

        internal void Register<TImplementation>()
        {
            if (_registrations.ContainsKey(typeof(TImplementation)))
            {
                Debug.LogError($"[DI] Type {typeof(TImplementation)} already registered");
                return;
            }

            _registrations[typeof(TImplementation)] = typeof(TImplementation);
        }


        internal void RegisterInstance<TInterface>(TInterface instance)
        {
            if (_singletons.ContainsKey(typeof(TInterface)))
            {
                Debug.LogError($"[DI] Type {typeof(TInterface)} already registered");
                return;
            }

            _singletons[typeof(TInterface)] = instance;
        }

        internal T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        internal object Resolve(Type type)
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

        internal bool Inject(object target)
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

        internal void Clear()
        {
            _singletons.Clear();
            _registrations.Clear();
        }
    }
}