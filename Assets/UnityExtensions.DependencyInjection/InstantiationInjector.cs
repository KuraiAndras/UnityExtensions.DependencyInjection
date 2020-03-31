using Microsoft.Extensions.DependencyInjection;
using System;
using Object = UnityEngine.Object;

namespace UnityExtensions.DependencyInjection
{
    public static class InstantiationInjector
    {
        public static IServiceCollection AddInjectorServices(this IServiceCollection services)
        {
            if (services is null) throw new ArgumentNullException(nameof(services));

            services.AddSingleton<IGameObjectFactory, DefaultGameObjectFactory>();
            services.AddSingleton<ISceneInjector, SceneInjector>(_ => Object.FindObjectOfType<SceneInjector>());

            return services;
        }
    }
}
