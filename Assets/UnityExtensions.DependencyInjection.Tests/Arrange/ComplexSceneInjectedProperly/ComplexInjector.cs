namespace UnityExtensions.DependencyInjection.Tests.Arrange.ComplexSceneInjectedProperly
{
    public sealed class ComplexInjector : Injector
    {
        protected override void Startup()
        {
            Services.AddComplexInjection();

            base.Startup();
        }
    }
}
