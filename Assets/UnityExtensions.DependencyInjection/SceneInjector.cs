using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
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
        private readonly ConcurrentDictionary<Type, (FieldInfo[] fieldInfos, PropertyInfo[] propertyInfos, MethodInfo[] methodInfos)> _resolveDictionary =
            new ConcurrentDictionary<Type, (FieldInfo[], PropertyInfo[], MethodInfo[])>();

        private IServiceProvider _serviceProvider;

        public void InitializeScene(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            foreach (var rootGameObject in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                InjectIntoGameObject(rootGameObject);
            }
        }

        public GameObject InjectIntoGameObject(GameObject gameObjectInstance)
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

            return gameObjectInstance;
        }

        public GameObject Instantiate(GameObject original) =>
            InjectIntoGameObject(Object.Instantiate(original));

        public GameObject Instantiate(GameObject original, Transform parent) =>
            InjectIntoGameObject(Object.Instantiate(original, parent));

        public GameObject Instantiate(GameObject original, Transform parent, bool instantiateInWorldSpace) =>
            InjectIntoGameObject(Object.Instantiate(original, parent, instantiateInWorldSpace));

        public GameObject Instantiate(GameObject original, Vector3 position, Quaternion rotation) =>
            InjectIntoGameObject(Object.Instantiate(original, position, rotation));

        public GameObject Instantiate(GameObject original, Vector3 position, Quaternion rotation, Transform parent) =>
            InjectIntoGameObject(Object.Instantiate(original, position, rotation, parent));

        private IServiceScope InjectIntoType(Type type, object instance)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));
            if (instance is null) throw new ArgumentNullException(nameof(instance));

            var didInstantiate = false;

            if (!_resolveDictionary.ContainsKey(type))
            {
                var allTypes = type
                    .GetAllTypes()
                    .ToList();

                var fieldsToInject = allTypes
                    .SelectMany(t => t.GetFields(InstanceBindingFlags))
                    .FilterMembersToArray();

                var propertiesToInject = allTypes
                    .SelectMany(t => t.GetProperties(InstanceBindingFlags))
                    .FilterMembersToArray();

                var methodsToInject = allTypes
                    .SelectMany(t => t.GetMethods(InstanceBindingFlags))
                    .FilterMembersToArray();

                _resolveDictionary.TryAdd(type, (fieldsToInject, propertiesToInject, methodsToInject));
            }

            var scope = _serviceProvider.CreateScope();

            _resolveDictionary.TryGetValue(type, out var members);

            members.fieldInfos.ForEach(f => didInstantiate = Inject(instance, scope, f));
            members.propertyInfos.ForEach(p => didInstantiate = Inject(instance, scope, p));
            members.methodInfos.ForEach(m => didInstantiate = Inject(instance, scope, m));

            return didInstantiate ? scope : null;
        }

        private static bool Inject(object instance, IServiceScope scope, PropertyInfo property)
        {
            if (property.CanWrite)
            {
                property.SetValue(instance, GetService(scope, property.PropertyType));
                return true;
            }

            if (!property.IsAutoProperty()) return false;

            property.GetAutoPropertyBackingField().SetValue(instance, GetService(scope, property.PropertyType));

            return true;
        }

        private static bool Inject(object instance, IServiceScope scope, FieldInfo field)
        {
            field.SetValue(instance, GetService(scope, field.FieldType));

            return true;
        }

        private static bool Inject(object instance, IServiceScope scope, MethodBase method)
        {
            var methodParameters = method.GetParameters();
            var parameters = new object[methodParameters.Length];
            for (var i = 0; i < methodParameters.Length; i++)
            {
                parameters[i] = GetService(scope, methodParameters[i].ParameterType);
            }

            method.Invoke(instance, parameters);

            return true;
        }

        private static object GetService(IServiceScope scope, Type memberType) => scope.ServiceProvider.GetService(memberType);

        private static BindingFlags InstanceBindingFlags { get; } = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
    }
}
