using System;
using System.Collections.Generic;
using System.Reflection;

namespace UnityDI
{
    /// <summary>
    /// Simple dependency injection container for Unity.
    /// </summary>
    public class Injector
    {
        private readonly Dictionary<Type, Func<object>> _bindings = new();

        public void Bind<T>(Func<T> factory)
        {
            _bindings[typeof(T)] = () => factory();
        }

        public void BindSingleton<T>(T instance)
        {
            _bindings[typeof(T)] = () => instance!;
        }

        public T Resolve<T>()
        {
            if (_bindings.TryGetValue(typeof(T), out var factory))
                return (T)factory();

            throw new InvalidOperationException($"Type {typeof(T)} not bound in DI container.");
        }

        public void Inject(object target)
        {
            var fields = target.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                var attr = Attribute.GetCustomAttribute(field, typeof(InjectAttribute));
                if (attr != null)
                {
                    var method = typeof(Injector).GetMethod(nameof(Resolve))!;
                    var generic = method.MakeGenericMethod(field.FieldType);
                    var dependency = generic.Invoke(this, null);
                    field.SetValue(target, dependency);
                }
            }
        }
    }

    /// <summary>
    /// Marks a field for dependency injection.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class InjectAttribute : Attribute { }
}

