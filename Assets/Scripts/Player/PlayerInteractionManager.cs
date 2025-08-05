using System.Collections.Generic;
using ColorGates;
using Interface;
using Managers;
using Misc;
using UnityEngine;

namespace Player
{
    public class PlayerInteractionManager : MonoBehaviour
    {
        [Header("References")] 
        private PlayerMovementController _playerMovementController;
        [SerializeField] private Transform _playerPlateTransform;
        [SerializeField] private float _scaleMultiplier = 1f;
        
        [Header("Settings")] 
        private List<GameObject> _collectedItems;
        private GameObject _selectedItem;
        private ColorType _currentColorType;
        private bool _isFirstPick = true;

        /// <summary>
        /// Awake, Start, Update, and other Unity lifecycle methods.
        /// </summary>
        #region Unity Methods

        private void Awake()
        {
            _collectedItems = new List<GameObject> { _playerPlateTransform.gameObject };
            _playerMovementController = GetComponent<PlayerMovementController>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(GameManager.Instance.GetCurrentGameState() != GameState.Playing) return;
            if(_selectedItem == other.gameObject) return;
            
            if (other.TryGetComponent(out ICollectible collectible))
            {
                _selectedItem = other.gameObject;
                collectible.Collect(OnCollectibleCollected);
            }
            else if (other.TryGetComponent(out ColorGate colorGate))
            {
                SetColorCollectedItems(colorGate.GetColorMaterial());
                _currentColorType = colorGate.GetColorType();
            }
        }

        #endregion
        
        private void SetColorCollectedItems(Material color)
        {
            if (_collectedItems.Count <= 1) return;

            foreach (var item in _collectedItems)
            {
                if (item.TryGetComponent(out MeshRenderer meshRenderer))
                {
                    meshRenderer.material = color;
                }
            }
        }

        private void OnCollectibleCollected(ColorType colorType)
        {
            Debug.Log($"Collected Color: {colorType}");

            // If this is the first collectible, set the current color type
            if (_isFirstPick)
            {
                _currentColorType = colorType;
                _isFirstPick = false;
            }
            
            if (_currentColorType == colorType)
            {
                CorrectCollectCollectible();
                EventManager.OnCorrectCollectibleCollected?.Invoke();
            }
            else
            {
                DropCollectible();
            }
        }

        private void CorrectCollectCollectible()
        {
            _selectedItem.transform.SetParent(_playerPlateTransform);
            _scaleMultiplier *= (_selectedItem.transform.localScale.y + _collectedItems[^1].transform.localScale.y) / 2;
                
            _selectedItem.transform.localPosition = new Vector3(0, _collectedItems[^1].transform.localPosition.y + _scaleMultiplier, 0);
            _collectedItems.Add(_selectedItem);

            _scaleMultiplier = 1f;
            
            _playerMovementController.IncreaseForwardSpeed(1f);
        }
        
        private void DropCollectible()
        {
            if (_collectedItems.Count <= 1)
            {
                GameManager.Instance.ChangeGameState(GameState.GameOver);
                GameManager.Instance.GameOver();
                return;
            }
            
            var collectible = _collectedItems[^1];
                
            collectible.transform.SetParent(null);
            _collectedItems.Remove(collectible);
            Destroy(collectible.gameObject);
            
            _playerMovementController.ResetForwardSpeed();
        }
    }
}