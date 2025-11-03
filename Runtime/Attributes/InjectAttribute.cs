using System;

namespace Mobiray.DI
{
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method)]
    public class InjectAttribute : Attribute { }
}