using UnityEngine;

namespace UnityExtensions.DependencyInjection.Tests.Arrange.InjectorAwakeCalledFirst
{
    public sealed class AwakeCalledFirstInjector : Injector
    {
        [SerializeField] private AwakeLogger _logger;

        protected override void Startup()
        {
            _logger.CallerTypes.Add(GetType());
            base.Startup();
        }
    }
}