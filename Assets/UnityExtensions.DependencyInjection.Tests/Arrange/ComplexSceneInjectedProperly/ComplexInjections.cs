using Microsoft.Extensions.DependencyInjection;

namespace UnityExtensions.DependencyInjection.Tests.Arrange.ComplexSceneInjectedProperly
{
    public static class ComplexInjections
    {
        public static IServiceCollection AddComplexInjection(this IServiceCollection services)
        {
            services.AddTransient<ITestService1, TestService1>();

            return services;
        }
    }
}
