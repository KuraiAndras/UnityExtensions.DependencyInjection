using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityExtensions.DependencyInjection.Tests
{
    public static class TestHelpers
    {
        public static (T, SceneInjector) SetUpInjector<T>() where T : Injector
        {
            SceneManager.LoadScene("UnityExtensions.DependencyInjection.Tests/Scenes/TestScene");

            var testsObject = Object.Instantiate(new GameObject("Tests"));

            testsObject.AddComponent<T>();

            var injector = Object.FindObjectOfType<T>();
            Assert.NotNull(injector);

            var sceneInjector = Object.FindObjectOfType<SceneInjector>();
            Assert.NotNull(sceneInjector);

            return (injector, sceneInjector);
        }
    }
}