using System;
using UnityEngine;

namespace UnityExtensions.DependencyInjection.Tests.Arrange.ComplexSceneInjectedProperly
{
    [DefaultExecutionOrder(-999)]
    public sealed class ComplexInjector : Injector
    {
        protected override IServiceProvider CreateServiceProvider()
        {
            Services.AddComplexInjection();

            return base.CreateServiceProvider();
        }
    }
}
