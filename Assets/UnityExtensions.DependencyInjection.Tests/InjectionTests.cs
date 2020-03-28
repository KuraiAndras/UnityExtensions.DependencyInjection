using System.Collections;
using UnityEngine.TestTools;
using UnityExtensions.DependencyInjection.Tests.Setup;

namespace UnityExtensions.DependencyInjection.Tests
{
    public sealed class InjectionTests
    {
        [UnityTest]
        public IEnumerator InjectorAwakeCalledFirst()
        {
            // TODO: Implement test

            TestHelpers.SetUpInjector<TestInjector>();

            yield return null;
        }
    }
}
