using UnityEngine;

namespace UnityExtensions.DependencyInjection
{
    public interface IGameObjectInjector
    {
        void InjectIntoGameObject(GameObject gameObjectInstance);
    }
}