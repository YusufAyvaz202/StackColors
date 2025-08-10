using System.Collections.Generic;
using Collectibles;
using Interface;
using Managers;
using Misc;
using UnityEngine;

namespace Player
{
    public class PlayerInteractionManager : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private Transform _playerPlateTransform;
        private PlayerMovementController _playerMovementController;
        private PlayerBonusController _playerBonusController;

        [Header("Settings")] 
        [SerializeField] private float _scaleMultiplier = 1f;
        private List<Collectible> _collectedItems;
        private Collectible _selectedItem;
        private ColorType _currentColorType;
        private bool _isFirstPick = true;
        private int _fewerModeCount;

        private Material _tempColorMaterial;

        /// <summary>
        /// Awake, Start, Update, and other Unity lifecycle methods.
        /// </summary>

        #region Unity Methods

        private void Awake()
        {
            _collectedItems = new List<Collectible> { _playerPlateTransform.GetComponent<Collectible>() };
            GetComponents();
        }

        private void OnTriggerEnter(Collider other)
        {
            CheckTriggers(other);
        }

        #endregion

        private void CheckTriggers(Collider other)
        {
            GameState gameState = GameManager.Instance.GetCurrentGameState();
            if (gameState != GameState.Playing && gameState != GameState.FewerMode) return;
            if (_selectedItem != null)
            {
                if (_selectedItem.gameObject == other.gameObject) return;
            }

            if (other.TryGetComponent(out ICollectible collectible))
            {
                _selectedItem = collectible as Collectible;
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
                    // If the game is in fewer mode & we pass a color changer, we set the fewer mode material.
                    if (GameManager.Instance.GetCurrentGameState() == GameState.FewerMode)
                    {
                        FewerModeManager.Instance.SetFewerModeMaterial(_tempColorMaterial, colorType);
                        FewerModeManager.Instance.ChangeFewerModeMaterial();
                    }

                    SetColorCollectedItems(_tempColorMaterial);
                    _currentColorType = colorType;
                    break;
                case CollectibleType.BonusCollectorStart:
                    GameManager.Instance.ChangeGameState(GameState.Playing);
                    BonusCollectorStart();
                    break;
                case CollectibleType.BonusCollectorEnd:
                    BonusCollectorEnd();
                    break;
                case CollectibleType.Gold:
                    CollectGold();
                    break;
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

            if(_collectedItems.Count == 1)
            {
                _selectedItem.GetComponent<FollowParent>().SetFollowTransform(_playerPlateTransform);
            }
            if (_collectedItems.Count > 1)
            {
                _selectedItem.GetComponent<FollowParent>().SetFollowTransform(_collectedItems[^1].transform);
            }

            _selectedItem.transform.localPosition = new Vector3(0, _collectedItems[^1].transform.localPosition.y + _scaleMultiplier, 0);
    
            _selectedItem.GetComponent<FollowParent>().UpdateBasePosition();
    
            _collectedItems.Add(_selectedItem);

            _scaleMultiplier = 1f;
            IncreaseFewerModeCount();

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
            ResetFewerMode();
        }

        #endregion

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

        #region Fewer Mode Methods

        private void IncreaseFewerModeCount()
        {
            if (GameManager.Instance.GetCurrentGameState() == GameState.FewerMode) return;

            _fewerModeCount++;
            EventManager.OnFewerModeChanged?.Invoke(_fewerModeCount);

            if (_fewerModeCount >= Conts.FewerMode.FewerModeCount)
            {
                ResetFewerModeCount();
                FewerModeManager.Instance.SetFewerModeMaterial(_tempColorMaterial, _currentColorType);
                GameManager.Instance.ChangeGameState(GameState.FewerMode);
            }
        }

        private void ResetFewerModeCount()
        {
            _fewerModeCount = 0;
        }

        private void ResetFewerMode()
        {
            _fewerModeCount = 0;
            EventManager.OnFewerModeChanged?.Invoke(_fewerModeCount);
        }

        #endregion

        #region Bonus Collector Methods

        // This method is called when the bonus collector starts
        private void BonusCollectorStart()
        {
            // Stacking is over. Bonus running is started.
            _playerBonusController.enabled = true;
            _playerMovementController.ResetForwardSpeed();
            _playerMovementController.DisableHorizontalMovement();

            // When the bonus collector starts, the player plate is moved to the center of the screen.
            Vector3 resetXPosition = new Vector3(0, transform.position.y, transform.position.z);
            transform.position = Vector3.Slerp(transform.position, resetXPosition, 2f);

            GameManager.Instance.SetActiveBonusUI(true);
        }

        private void BonusCollectorEnd()
        {
            EventManager.ChangeCameraTarget?.Invoke(_collectedItems[^1].transform);
            
            float bonus = _playerBonusController.GetTotalBonus();
            for (int i = 0; i < _collectedItems.Count; i++)
            {
                float power = i / 2f + bonus/10f;
                _collectedItems[i].KickCollectible(power * Vector3.forward + Vector3.up);
            }

            // Bonus run is over. Calculate bonus.
            _playerBonusController.enabled = false;
            _playerMovementController.enabled = false;
            Invoke(nameof(WaitBonusAnimationEnd), 5f);
        }

        private void WaitBonusAnimationEnd()
        {
            GameManager.Instance.ChangeGameState(GameState.BonusCalculation);
        }

        #endregion

        private void CollectGold()
        {
            EventManager.OnGoldCollected?.Invoke();
        }

        #region Initialize & Cleanup

        private void GetComponents()
        {
            _playerMovementController = GetComponent<PlayerMovementController>();
            _playerBonusController = GetComponent<PlayerBonusController>();
        }

        #endregion
    }
}