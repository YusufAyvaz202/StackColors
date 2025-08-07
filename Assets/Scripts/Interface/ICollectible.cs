using System;
using Misc;
using UnityEngine;

namespace Interface
{
    public interface ICollectible
    {
        void Collect(Action<ColorType, CollectibleType> onCollected);
        Material GetColorMaterial();
    }
}