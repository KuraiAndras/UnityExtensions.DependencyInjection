using UnityEngine;

namespace UnityExtensions.DependencyInjection.Tests.Arrange.DisposeIsCalledOnDestroy
{
    public sealed class ListenToDispose : MonoBehaviour, IListenToDispose
    {
        public bool DisposeCalled { get; set; } = false;
    }
}