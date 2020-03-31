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

        protected Injector() => Services = new ServiceCollection().AddInjectorServices();

        /// <summary>
        /// Create service registration here.
        /// Builds <see cref="IServiceProvider"/> from <see cref="Services"/>
        /// </summary>
        protected virtual IServiceProvider CreateServiceProvider() => Services.BuildServiceProvider();

        /// <summary>
        /// Calls <see cref="SceneInjector"/> With <see cref="CreateServiceProvider"/>
        /// Add <see cref="DefaultExecutionOrder"/> to ensure this awake is called first in your scene.
        /// </summary>
        protected virtual void Awake() => SceneInjector.InitializeScene(CreateServiceProvider());
    }
}