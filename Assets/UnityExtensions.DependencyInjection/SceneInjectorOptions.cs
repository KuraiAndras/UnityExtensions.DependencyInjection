﻿namespace UnityExtensions.DependencyInjection
{
    public sealed class SceneInjectorOptions
    {
        public bool UseCaching { get; set; } = true;
        public bool DontDestroyOnLoad { get; set; } = true;
    }
}