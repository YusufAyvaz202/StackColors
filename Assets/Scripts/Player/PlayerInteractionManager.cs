using Interface;
using Misc;
using UnityEngine;

namespace Player
{
    public class PlayerInteractionManager : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ICollectible collectible))
            {
                collectible.Collect(OnCollectibleCollected);
            }
        }

        private void OnCollectibleCollected(ColorType colorType)
        {
            // Handle the collected color type, e.g., update UI, play sound, etc.
            Debug.Log($"Collected Color: {colorType}");
            switch (colorType)
            {
                case ColorType.Blue:
                    break;
                case ColorType.Green:
                    break;  
                case ColorType.Red:
                    break;
                case ColorType.Yellow:
                    break;
            }
        }
    }
}