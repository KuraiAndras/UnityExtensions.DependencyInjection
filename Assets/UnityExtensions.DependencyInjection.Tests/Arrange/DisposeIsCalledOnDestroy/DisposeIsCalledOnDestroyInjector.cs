using Microsoft.Extensions.DependencyInjection;
using System;
using UnityEngine;

namespace UnityExtensions.DependencyInjection.Tests.Arrange.DisposeIsCalledOnDestroy
{
    [DefaultExecutionOrder(-999)]
    public sealed class DisposeIsCalledOnDestroyInjector : Injector
    {
        protected override IServiceProvider CreateServiceProvider()
        {
            Services.AddSingleton<IListenToDispose, ListenToDispose>(_ => FindObjectOfType<ListenToDispose>());
            Services.AddTransient<ITestDisposable, TestDisposable>();

            return base.CreateServiceProvider();
        }
    }
}
