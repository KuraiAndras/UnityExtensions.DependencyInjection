using Microsoft.Extensions.DependencyInjection;
using UnityEngine;

namespace UnityExtensions.DependencyInjection
{
    public abstract class Injector : MonoBehaviour
    {
        public abstract void AddInjections(IServiceCollection services);
    }
}