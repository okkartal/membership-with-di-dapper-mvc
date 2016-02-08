using Autofac;

namespace IoC
{
    public interface IScopeProvider
    {
        ILifetimeScope Scope();
    }
}
