using System.Collections.Generic;
using Interface;
using Misc;
using UnityEngine;

namespace Player
{
    public class PlayerInteractionManager : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private Transform _playerPlateTransform;

        [Header("Settings")] 
        private List<GameObject> _collectedItems;
        private GameObject _selectedItem;
        private ColorType _currentColorType;

        private void Awake()
        {
            _collectedItems = new List<GameObject> { _playerPlateTransform.gameObject };
        }

        private void OnTriggerEnter(Collider other)
        {   
            if (other.TryGetComponent(out ICollectible collectible))
            {
                _selectedItem = other.gameObject;
                collectible.Collect(OnCollectibleCollected);
            }
        }

        private void OnCollectibleCollected(ColorType colorType)
        {
            Debug.Log($"Collected Color: {colorType}");
            if (_currentColorType == colorType)
            {
                _selectedItem.transform.SetParent(_playerPlateTransform);
                
                _selectedItem.transform.localPosition = new Vector3(0, _collectedItems[^1].transform.localPosition.y + 10f, 0);
                _collectedItems.Add(_selectedItem);
            }
            else
            {
                _collectedItems.RemoveAt(_collectedItems.Count);
            }
        }
    }
}