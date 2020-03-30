using System;
using UnityEngine;

namespace UnityExtensions.DependencyInjection.Tests.Arrange.InjectorAwakeCalledFirst
{
    [DefaultExecutionOrder(-999)]
    public sealed class AwakeCalledFirstInjector : Injector
    {
        [SerializeField] private AwakeLogger _logger;

        protected override IServiceProvider CreateServiceProvider()
        {
            _logger.CallerTypes.Add(GetType());

            return base.CreateServiceProvider();
        }
    }
}