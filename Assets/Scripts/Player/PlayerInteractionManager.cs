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
        private Color _tempColorMaterial;
        private bool _isFirstPick = true;
        private int _fewerModeCount;
        private bool _canCheckTriggers;
        private bool _isFewerModeActive;
        
        [Header("Follow Settings")]
        private float _followStrength = .1f;
        private float _followSpeed = 1f;
        private float _followDamp = 15f;

        /// <summary>
        /// Awake, Start, Update, and other Unity lifecycle methods.
        /// </summary>

        #region Unity Methods

        private void Awake()
        {
            _collectedItems = new List<Collectible> { _playerPlateTransform.GetComponent<Collectible>() };
            GetComponents();
        }

        private void OnEnable()
        {
            EventManager.OnGameStateChanged += OnGameStateChanged;
        }
        
        private void OnDisable()
        {
            EventManager.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnTriggerEnter(Collider other)
        {
            CheckTriggers(other);
        }

        #endregion
        
        private void OnGameStateChanged(GameState currentGameState)
        {
            if (currentGameState == GameState.Playing)
            {
                _canCheckTriggers = true;
                _isFewerModeActive = false;
            }
            else if (currentGameState == GameState.FewerMode)
            {
                _canCheckTriggers = true;
                _isFewerModeActive = true;
            }
            else
            {
                _canCheckTriggers = false;
            }
        }


        private void CheckTriggers(Collider other)
        {
            if (!_canCheckTriggers) return;
            
            if (_selectedItem != null)
            {
                if(_selectedItem.gameObject == other.gameObject) return;
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
                    if (_isFewerModeActive)
                    {
                        FewerModeManager.Instance.SetFewerModeMaterial(_tempColorMaterial, colorType);
                        FewerModeManager.Instance.ChangeFewerModeMaterial();
                    }

                    SetColorAllCollectedItems(_tempColorMaterial);
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

        
        private void CorrectCollectibleCollected()
        {
            _selectedItem.transform.SetParent(_playerPlateTransform);
            _scaleMultiplier *= (_selectedItem.transform.localScale.y + _collectedItems[^1].transform.localScale.y) / 2;

            // Collectible follow settings
            if(_collectedItems.Count == 1)
            {
                _selectedItem.SetFollowSettings(_playerPlateTransform, _followStrength, _followSpeed, _followDamp);
            }
            if (_collectedItems.Count > 1)
            {
                _followStrength = Mathf.Clamp(_followStrength + .1f,0.1f, 3f);
                _followSpeed = Mathf.Clamp(_followSpeed + .1f,0.1f, 3f);
                _selectedItem.SetFollowSettings(_collectedItems[^1].transform, _followStrength, _followSpeed, _followDamp);
            }
            _selectedItem.transform.localPosition = new Vector3(0, _collectedItems[^1].transform.localPosition.y + _scaleMultiplier, 0);
            _selectedItem.UpdateBasePosition();
    
            _collectedItems.Add(_selectedItem);
            _selectedItem.SetIsCollected(true);
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
            
            // Follow settings
            _followStrength = Mathf.Clamp(_followStrength - .1f, 0.1f, 2.5f);
            _followSpeed = Mathf.Clamp(_followSpeed - .1f, 0.1f,2.5f);
        }

        #endregion

        // Sets the color of all collected items to the specified color
        private void SetColorAllCollectedItems(Color colorMaterial)
        {
            if (_collectedItems.Count <= 1) return;

            foreach (var item in _collectedItems)
            {
                item.SetColor(colorMaterial);
            }
        }

        #region Fewer Mode Methods

        private void IncreaseFewerModeCount()
        {
            if (_isFewerModeActive) return;

            _fewerModeCount++;
            EventManager.OnFewerModeChanged?.Invoke(_fewerModeCount);

            if (_fewerModeCount >= Conts.FewerMode.FEWER_MODE_COUNT)
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
            /*Vector3 resetXPosition = new Vector3(0, transform.position.y, transform.position.z);
            transform.position = Vector3.Slerp(transform.position, resetXPosition, 2f);*/

            GameManager.Instance.SetActiveBonusUI(true);
            
            // Disable follow for all collected items
            foreach (var item in _collectedItems)
            {
                item.DisableFollow();
            }
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