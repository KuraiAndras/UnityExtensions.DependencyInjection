using UnityEngine;

namespace UnityExtensions.DependencyInjection
{
    public interface IGameObjectInjector
    {
        /// <summary>
        /// Sets up usages of the InjectAttribute
        /// </summary>
        /// <param name="gameObjectInstance">GameObject instance to inject into</param>
        /// <returns>Original instance</returns>
        GameObject InjectIntoGameObject(GameObject gameObjectInstance);

        /// <summary>
        /// Calls Object.Instantiate with the appropriate parameters
        /// Injects into created instance.
        /// </summary>
        /// <returns>Instantiated GameObject instance</returns>
        GameObject Instantiate(GameObject original);

        /// <summary>
        /// Calls Object.Instantiate with the appropriate parameters
        /// Injects into created instance.
        /// </summary>
        /// <returns>Instantiated GameObject instance</returns>
        GameObject Instantiate(GameObject original, Transform parent);

        /// <summary>
        /// Calls Object.Instantiate with the appropriate parameters
        /// Injects into created instance.
        /// </summary>
        /// <returns>Instantiated GameObject instance</returns>
        GameObject Instantiate(GameObject original, Transform parent, bool instantiateInWorldSpace);

        /// <summary>
        /// Calls Object.Instantiate with the appropriate parameters
        /// Injects into created instance.
        /// </summary>
        /// <returns>Instantiated GameObject instance</returns>
        GameObject Instantiate(GameObject original, Vector3 position, Quaternion rotation);

        /// <summary>
        /// Calls Object.Instantiate with the appropriate parameters
        /// Injects into created instance.
        /// </summary>
        /// <returns>Instantiated GameObject instance</returns>
        GameObject Instantiate(GameObject original, Vector3 position, Quaternion rotation, Transform parent);
    }
}