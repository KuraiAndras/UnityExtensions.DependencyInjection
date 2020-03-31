using System;
using UnityEngine;

namespace UnityExtensions.DependencyInjection.Tests.Arrange.InjectorAwakeCalledFirst
{
    [DefaultExecutionOrder(-999)]
    public sealed class AwakeCalledFirstInjector : Injector
    {
#pragma warning disable 649
        [SerializeField] private AwakeLogger _logger;
#pragma warning restore 649

        protected override IServiceProvider CreateServiceProvider()
        {
            _logger.CallerTypes.Add(GetType());

            return base.CreateServiceProvider();
        }
    }
}