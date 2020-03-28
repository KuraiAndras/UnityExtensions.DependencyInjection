using Microsoft.Extensions.DependencyInjection;
using UnityEngine;

namespace UnityExtensions.DependencyInjection
{
    internal sealed class DestroyDetector : MonoBehaviour
    {
        private void Awake() => hideFlags = HideFlags.HideInInspector;

        internal IServiceScope Scope { private get; set; }

        private void OnDestroy() => Scope?.Dispose();
    }
}