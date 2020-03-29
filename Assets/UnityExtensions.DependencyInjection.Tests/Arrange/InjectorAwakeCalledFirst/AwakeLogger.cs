using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityExtensions.DependencyInjection.Tests.Arrange.InjectorAwakeCalledFirst
{
    public sealed class AwakeLogger : MonoBehaviour
    {
        public List<Type> CallerTypes { get; } = new List<Type>();
    }
}
