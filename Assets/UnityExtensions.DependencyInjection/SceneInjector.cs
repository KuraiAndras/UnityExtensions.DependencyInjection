using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityExtensions.DependencyInjection.Extensions;
using Object = UnityEngine.Object;

namespace UnityExtensions.DependencyInjection
{
    public sealed class SceneInjector : MonoBehaviour, IGameObjectInjector
    {
        private IServiceProvider _serviceProvider;

        public void InitializeScene(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            foreach (var rootGameObject in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                InjectIntoGameObject(rootGameObject);
            }
        }

        public void InjectIntoGameObject(GameObject gameObjectInstance)
        {
            if (gameObjectInstance is null) throw new ArgumentNullException(nameof(gameObjectInstance));

            var componentsToInject = gameObjectInstance
                .GetComponentsInChildren(typeof(MonoBehaviour), true)
                .Select(c => (c.GetType(), (object)c, c.gameObject));

            foreach (var (type, instance, componentGameObject) in componentsToInject)
            {
                var instanceScope = InjectIntoType(type, instance);

                if (instanceScope is null) continue;

                componentGameObject.AddComponent<DestroyDetector>().Disposable = instanceScope;
            }
        }

        public GameObject Instantiate(GameObject original)
        {
            var go = Object.Instantiate(original);

            InjectIntoGameObject(gameObject);

            return go;
        }

        public GameObject Instantiate(GameObject original, Transform parent)
        {
            var go = Object.Instantiate(original, parent);

            InjectIntoGameObject(gameObject);

            return go;
        }

        public GameObject Instantiate(GameObject original, Transform parent, bool instantiateInWorldSpace)
        {
            var go = Object.Instantiate(original, parent, instantiateInWorldSpace);

            InjectIntoGameObject(gameObject);

            return go;
        }

        public GameObject Instantiate(GameObject original, Vector3 position, Quaternion rotation)
        {
            var go = Object.Instantiate(original, position, rotation);

            InjectIntoGameObject(gameObject);

            return go;
        }

        public GameObject Instantiate(GameObject original, Vector3 position, Quaternion rotation, Transform parent)
        {
            var go = Object.Instantiate(original, position, rotation, parent);

            InjectIntoGameObject(gameObject);

            return go;
        }

        private IServiceScope InjectIntoType(Type type, object instance)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));
            if (instance is null) throw new ArgumentNullException(nameof(instance));

            var didInstantiate = false;

            var scope = _serviceProvider.CreateScope();

            var allTypes = type
                .GetParentTypes()
                .Concat(new[] { type })
                .ToList();

            foreach (var field in allTypes
                .SelectMany(t => t.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
                .Where(TypeExtensions.MemberHasInjectAttribute)
                .Distinct())
            {
                field.SetValue(instance, scope.ServiceProvider.GetService(field.FieldType));
                didInstantiate = true;
            }

            foreach (var property in allTypes
                .SelectMany(t => t.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
                .Where(TypeExtensions.MemberHasInjectAttribute)
                .Distinct())
            {
                if (property.CanWrite)
                {
                    property.SetValue(instance, scope.ServiceProvider.GetService(property.PropertyType));
                    didInstantiate = true;
                }
                else if (property.IsAutoProperty())
                {
                    property.GetAutoPropertyBackingField().SetValue(instance, scope.ServiceProvider.GetService(property.PropertyType));
                    didInstantiate = true;
                }
            }

            foreach (var method in allTypes
                .SelectMany(t => t.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
                .Where(TypeExtensions.MemberHasInjectAttribute)
                .Distinct())
            {
                var methodParameters = method.GetParameters();
                var parameters = new object[methodParameters.Length];
                for (var i = 0; i < methodParameters.Length; i++)
                {
                    parameters[i] = scope.ServiceProvider.GetService(methodParameters[i].ParameterType);
                    didInstantiate = true;
                }

                method.Invoke(instance, parameters);
            }

            return didInstantiate ? scope : null;
        }
    }
}
