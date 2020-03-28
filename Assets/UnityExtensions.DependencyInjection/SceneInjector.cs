using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityExtensions.DependencyInjection.Extensions;

namespace UnityExtensions.DependencyInjection
{
    internal sealed class SceneInjector : MonoBehaviour, IGameObjectInjector
    {
        private IServiceProvider _serviceProvider;

        internal void InitializeScene(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            foreach (var rootGameObject in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                InjectIntoGameObject(rootGameObject);
            }
        }

        private IServiceScope InjectIntoType(Type type, object instance)
        {
            var scope = _serviceProvider.CreateScope();

            foreach (var field in type
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(MemberContainsInjectAttribute))
            {
                field.SetValue(instance, scope.ServiceProvider.GetService(field.FieldType));
            }

            foreach (var property in type
                .GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(MemberContainsInjectAttribute))
            {
                property.SetValue(instance, scope.ServiceProvider.GetService(property.PropertyType));
            }

            foreach (var method in type
                .GetParentTypes()
                .Concat(new[] { type })
                .SelectMany(t => t.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
                .Where(MemberContainsInjectAttribute))
            {
                var methodParameters = method.GetParameters();
                var parameters = new object[methodParameters.Length];
                for (var i = 0; i < methodParameters.Length; i++)
                {
                    parameters[i] = scope.ServiceProvider.GetService(methodParameters[i].ParameterType);
                }

                method.Invoke(instance, parameters);
            }

            return scope;
        }

        public void InjectIntoGameObject(GameObject gameObjectInstance)
        {
            var componentsToInject = gameObjectInstance
                .GetComponentsInChildren(typeof(MonoBehaviour), true)
                .Select(c => (type: c.GetType(), instance: (object)c));

            foreach (var (type, instance) in componentsToInject)
            {
                var instanceScope = InjectIntoType(type, instance);

                gameObjectInstance.AddComponent<DestroyDetector>().Disposable = instanceScope;
            }
        }

        private static bool MemberContainsInjectAttribute<T>(T memberInfo) where T : MemberInfo => memberInfo.GetCustomAttributes<InjectAttribute>().Any();
    }
}
