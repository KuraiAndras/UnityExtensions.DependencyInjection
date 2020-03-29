using Microsoft.Extensions.DependencyInjection;
using System;
using UnityEngine;
using UnityExtensions.DependencyInjection.Extensions;

namespace UnityExtensions.DependencyInjection
{
    /// <summary>
    /// Has execution order : -999
    /// </summary>
    [DefaultExecutionOrder(-999)]
    [RequireComponent(typeof(SceneInjector))]
    public abstract class Injector : MonoBehaviour
    {
        private static readonly IServiceCollection DefaultServiceCollection = new ServiceCollection().AddInstantiation();

        protected Injector() => ServiceProvider = Services.BuildServiceProvider() ?? DefaultServiceCollection.BuildServiceProvider();

        protected IServiceCollection Services { get; set; } = DefaultServiceCollection;

        /// <summary>
        /// Set to initialize services
        /// </summary>
        protected IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// Starts initializing the scene with the ServiceProvider.
        /// Gets called in Awake.
        /// Injector should be set as the first relevant executing script.
        /// Override this and add services here.
        /// Don't forget to call base when overriden.
        /// </summary>
        protected virtual void Startup() => GetComponent<SceneInjector>().InitializeScene(ServiceProvider);

        /// <summary>
        /// Calls Startup
        /// </summary>
        protected virtual void Awake() => Startup();
    }
}