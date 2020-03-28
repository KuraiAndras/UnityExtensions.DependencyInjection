using Microsoft.Extensions.DependencyInjection;

namespace UnityExtensions.DependencyInjection.Tests.Setup
{
    public class TestInjector : Injector
    {
        protected override void Startup()
        {
            ServiceProvider = Services.BuildServiceProvider();

            base.Startup();
        }
    }
}
