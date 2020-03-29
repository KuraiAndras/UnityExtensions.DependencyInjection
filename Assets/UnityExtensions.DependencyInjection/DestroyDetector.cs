using System;
using UnityEngine;

namespace UnityExtensions.DependencyInjection
{
    internal sealed class DestroyDetector : MonoBehaviour
    {
        private IDisposable _disposable;

        internal IDisposable Disposable
        {
            private get { return _disposable; }
            set
            {
                _disposable?.Dispose();

                _disposable = value;
            }
        }


        private void Awake() => hideFlags = HideFlags.HideInInspector;
        private void OnDestroy() => Disposable?.Dispose();
    }
}