using Microsoft.Extensions.DependencyInjection;
using System;
using UnityEngine;
using UnityExtensions.DependencyInjection.Extensions;

namespace UnityExtensions.DependencyInjection
{
    [RequireComponent(typeof(SceneInjector))]
    public abstract class Injector : MonoBehaviour
    {
        protected SceneInjector SceneInjector => GetComponent<SceneInjector>();
        protected IServiceCollection Services { get; set; }

        protected Injector() => Services = new ServiceCollection().AddInstantiation();

        protected virtual IServiceProvider CreateServiceProvider() => Services.BuildServiceProvider();

        protected virtual void Awake() => SceneInjector.InitializeScene(CreateServiceProvider());
    }
}