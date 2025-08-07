using System.Collections.Generic;
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
        private PlayerBonusController _playerBonusController;
        [SerializeField] private Transform _playerPlateTransform;
        [SerializeField] private float _scaleMultiplier = 1f;
        
        [Header("Settings")] 
        private List<GameObject> _collectedItems;
        private GameObject _selectedItem;
        private ColorType _currentColorType;
        private bool _isFirstPick = true;
        
        private Material _tempColorMaterial;

        /// <summary>
        /// Awake, Start, Update, and other Unity lifecycle methods.
        /// </summary>
        #region Unity Methods

        private void Awake()
        {
            _collectedItems = new List<GameObject> { _playerPlateTransform.gameObject };
            GetComponents();
        }

        private void OnTriggerEnter(Collider other)
        {
            CheckTriggers(other);
        }

        #endregion
        
        private void CheckTriggers(Collider other)
        {
            if(GameManager.Instance.GetCurrentGameState() != GameState.Playing) return;
            if(_selectedItem == other.gameObject) return;
            
            if (other.TryGetComponent(out ICollectible collectible))
            {
                _selectedItem = other.gameObject;
                _tempColorMaterial = collectible.GetColorMaterial();
                collectible.Collect(OnCollectibleCollected);
            }
        }
        
        // This method is called when a collectible is collected
        private void OnCollectibleCollected(ColorType colorType, CollectibleType collectibleType)
        {
            switch (collectibleType)
            {
                case CollectibleType.Color:
                    ColorCollectibleCollected(colorType);
                    break;
                case CollectibleType.ColorChanger:
                    SetColorCollectedItems(_tempColorMaterial);
                    _currentColorType = colorType;
                    break;
                case CollectibleType.BonusCollectorStart:
                    BonusCollectorStart();
                    break;
                case CollectibleType.BonusCollectorEnd:
                    BonusCollectorEnd();
                    break;
            }
        }
        
        // This method is called when the bonus collector starts
        private void BonusCollectorStart()
        {
            // Stacking is over. Bonus running is started.
            _playerBonusController.enabled = true;
            _playerMovementController.ResetForwardSpeed();
            _playerMovementController.DisableHorizontalMovement();
            
            GameManager.Instance.SetActiveBonusUI(true);
        }
        
        private void BonusCollectorEnd()
        {
            // Bonus run is over. Calculate bonus.
            _playerBonusController.enabled = false;
            GameManager.Instance.ChangeGameState(GameState.BonusCalculation);
        }
        
        // Sets the color of all collected items to the specified color
        private void SetColorCollectedItems(Material colorMaterial)
        {
            if (_collectedItems.Count <= 1) return;

            foreach (var item in _collectedItems)
            {
                if (item.TryGetComponent(out MeshRenderer meshRenderer))
                {
                    meshRenderer.material = colorMaterial;
                }
            }
        }

        #region Color Collectible Methods

        private void ColorCollectibleCollected(ColorType colorType)
        {
            // If this is the first collectible, set the current color type
            if (_isFirstPick)
            {
                _currentColorType = colorType;
                _isFirstPick = false;
            }
            
            if (_currentColorType == colorType)
            {
                CorrectCollectibleCollected();
                EventManager.OnCorrectCollectibleCollected?.Invoke();
            }
            else
            {
                WrongCollectCollectible();
            }
        }

        // This method is called when a collectible is collected correctly
        private void CorrectCollectibleCollected()
        {
            _selectedItem.transform.SetParent(_playerPlateTransform);
            _scaleMultiplier *= (_selectedItem.transform.localScale.y + _collectedItems[^1].transform.localScale.y) / 2;
                
            _selectedItem.transform.localPosition = new Vector3(0, _collectedItems[^1].transform.localPosition.y + _scaleMultiplier, 0);
            _collectedItems.Add(_selectedItem);

            _scaleMultiplier = 1f;
            
            _playerMovementController.IncreaseForwardSpeed(.25f);
        }
        
        // This method is called when a collectible is collected incorrectly
        private void WrongCollectCollectible()
        {
            if (_collectedItems.Count <= 1)
            {
                GameManager.Instance.ChangeGameState(GameState.GameOver);
                return;
            }
            
            var collectible = _collectedItems[^1];
                
            collectible.transform.SetParent(null);
            _collectedItems.Remove(collectible);
            Destroy(collectible.gameObject);
            
            _playerMovementController.ResetForwardSpeed();
        }

        #endregion


        #region Initialize & Cleanup
        
        private void GetComponents()
        {
            _playerMovementController = GetComponent<PlayerMovementController>();
            _playerBonusController = GetComponent<PlayerBonusController>();
        }

        #endregion
    }
}