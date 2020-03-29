using UnityEngine;

namespace UnityExtensions.DependencyInjection
{
    public interface IGameObjectInjector
    {
        void InjectIntoGameObject(GameObject gameObjectInstance);
        GameObject Instantiate(GameObject original);
        GameObject Instantiate(GameObject original, Transform parent);
        GameObject Instantiate(GameObject original, Transform parent, bool instantiateInWorldSpace);
        GameObject Instantiate(GameObject original, Vector3 position, Quaternion rotation);
        GameObject Instantiate(GameObject original, Vector3 position, Quaternion rotation, Transform parent);
    }
}