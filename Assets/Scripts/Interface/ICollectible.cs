using System;
using Misc;

namespace Interface
{
    public interface ICollectible
    {
        void Collect(Action<ColorType> onCollected);
    }
}