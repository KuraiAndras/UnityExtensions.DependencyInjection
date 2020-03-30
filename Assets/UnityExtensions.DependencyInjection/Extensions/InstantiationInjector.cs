using Microsoft.Extensions.DependencyInjection;
using System;
using Object = UnityEngine.Object;

namespace UnityExtensions.DependencyInjection.Extensions
{
    public static class InstantiationInjector
    {
        public static IServiceCollection AddDefaultServices(this IServiceCollection services)
        {
            if (services is null) throw new ArgumentNullException(nameof(services));

            services.AddSingleton<IGameObjectInjector, SceneInjector>(_ => Object.FindObjectOfType<SceneInjector>());

            return services;
        }
    }
}
