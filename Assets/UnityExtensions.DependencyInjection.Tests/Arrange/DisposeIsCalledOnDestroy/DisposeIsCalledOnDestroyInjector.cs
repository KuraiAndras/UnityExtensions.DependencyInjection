using Microsoft.Extensions.DependencyInjection;

namespace UnityExtensions.DependencyInjection.Tests.Arrange.DisposeIsCalledOnDestroy
{
    public sealed class DisposeIsCalledOnDestroyInjector : Injector
    {
        protected override void Startup()
        {
            Services.AddSingleton<IListenToDispose, ListenToDispose>(_ => FindObjectOfType<ListenToDispose>());
            Services.AddTransient<ITestDisposable, TestDisposable>();
            ServiceProvider = Services.BuildServiceProvider();

            base.Startup();
        }
    }
}
