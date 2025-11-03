namespace Mobiray.DI
{
    public static class UnityDI
    {
        public static void Register<TInterface, TImplementation>() where TImplementation : TInterface
            => DIContainer.Instance.Register<TInterface, TImplementation>();

        public static void Register<TImplementation>()
            => DIContainer.Instance.Register<TImplementation>();

        public static void RegisterInstance<TInterface>(TInterface instance)
            => DIContainer.Instance.RegisterInstance(instance);

        public static T Resolve<T>()
            => DIContainer.Instance.Resolve<T>();

        public static void Inject(object target)
            => DIContainer.Instance.Inject(target);

        public static void Clear()
            => DIContainer.Instance.Clear();
    }
}