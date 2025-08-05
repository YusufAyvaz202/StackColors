using System;
using Misc;

namespace Managers
{
    public static class EventManager
    {
        // Event triggered when a correct collectible is collected.
        public static Action OnCorrectCollectibleCollected;
        
        public static Action<GameState> OnGameStateChanged;
    }
}