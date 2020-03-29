using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityExtensions.DependencyInjection.Tests.Arrange.ComplexSceneInjectedProperly;
using UnityExtensions.DependencyInjection.Tests.Arrange.InjectorAwakeCalledFirst;

namespace UnityExtensions.DependencyInjection.Tests.Injection
{
    public sealed class InjectionTests
    {
        [UnityTest]
        public IEnumerator InjectorAwakeCalledFirst()
        {
            // Arrange
            SceneManager.LoadScene($"UnityExtensions.DependencyInjection.Tests/Scenes/{nameof(InjectorAwakeCalledFirst)}", LoadSceneMode.Single);

            // Act
            yield return null;

            // Assert
            var logger = Object.FindObjectOfType<AwakeLogger>();
            Assert.AreEqual(logger.CallerTypes.Count, 6);
            Assert.AreEqual(logger.CallerTypes[0], typeof(AwakeCalledFirstInjector));
        }

        [UnityTest]
        public IEnumerator ComplexSceneInjectedProperly()
        {
            // Arrange
            SceneManager.LoadScene($"UnityExtensions.DependencyInjection.Tests/Scenes/{nameof(ComplexSceneInjectedProperly)}", LoadSceneMode.Single);

            // Act
            yield return null;

            // Assert
            var sut = Object.FindObjectOfType<ComplexConcrete>();
            sut.AssertBaseInjected();
            sut.AssertConcreteInjected();
        }
    }
}
