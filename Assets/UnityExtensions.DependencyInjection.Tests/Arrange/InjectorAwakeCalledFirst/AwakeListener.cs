using UnityEngine;

namespace UnityExtensions.DependencyInjection.Tests.Arrange.InjectorAwakeCalledFirst
{
    public sealed class AwakeListener : MonoBehaviour
    {
        [SerializeField] private AwakeLogger _logger;

        private void Awake() => _logger.CallerTypes.Add(GetType());
    }
}
