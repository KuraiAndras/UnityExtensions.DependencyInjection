﻿using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityExtensions.DependencyInjection.Tests.Arrange.ComplexSceneInjectedProperly;
using UnityExtensions.DependencyInjection.Tests.Arrange.DisposeIsCalledOnDestroy;
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
            var sut = Object.FindObjectOfType<AwakeLogger>();
            Assert.AreEqual(sut.CallerTypes.Count, 6);
            Assert.AreEqual(sut.CallerTypes[0], typeof(AwakeCalledFirstInjector));
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

        [UnityTest]
        public IEnumerator DisposeIsCalledOnDestroy()
        {
            // Arrange
            SceneManager.LoadScene($"UnityExtensions.DependencyInjection.Tests/Scenes/{nameof(DisposeIsCalledOnDestroy)}", LoadSceneMode.Single);

            // Act
            yield return null;

            var sut = GameObject.Find("SUT");
            Object.Destroy(sut);

            yield return null;

            // Assert
            var listener = Object.FindObjectOfType<ListenToDispose>();
            Assert.True(listener.DisposeCalled);
        }
    }
}
