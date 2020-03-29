using Microsoft.Extensions.DependencyInjection;

namespace UnityExtensions.DependencyInjection.Tests.Arrange.ComplexSceneInjectedProperly
{
    public sealed class ComplexInjector : Injector
    {
        protected override void Startup()
        {
            Services.AddComplexInjection();
            ServiceProvider = Services.BuildServiceProvider();

            base.Startup();
        }
    }
}
