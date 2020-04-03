using Injecter;
using UnityEngine;

namespace UnityExtensions.DependencyInjection.Tests.Arrange.DisposeIsCalledOnDestroy
{
    public sealed class DisposingScript : MonoBehaviour
    {
        [Inject] private readonly ITestDisposable _testDisposable;
    }
}