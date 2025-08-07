using System;
using Misc;

namespace Managers
{
    public static class EventManager
    {
        // Event triggered when a correct collectible is collected.
        public static Action OnCorrectCollectibleCollected;
        public static Action OnGoldCollected;
        
        // Event triggered when the game state changes.
        public static Action<GameState> OnGameStateChanged;
        
        // Event triggered when a bonus action is performed.
        public static Action OnBonusActionPerformed;
    }
}