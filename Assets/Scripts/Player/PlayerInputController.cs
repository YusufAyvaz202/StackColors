using Managers;
using Misc;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInputController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private InputActionAsset _inputActionAsset;
        private PlayerAnimationController _playerAnimationController;
    
        [Header("Input Actions")]
        private InputAction _moveAction;
        private InputAction _bonusAction;
    
        [Header("Input Action Callbacks")]
        private Vector2 _moveInput;
        public Vector2 MoveInput => _moveInput;
        
        [Header("Settings")]
        private bool isStarted;

        
        #region Unity Methods
        private void Awake()
        {
            GetComponents();
        }
        
        private void OnEnable()
        {
            StartActions();
        }

        private void OnDisable()
        {
            StopActions();
        }
        
        #endregion
    
        private void MoveActionOnStarted(InputAction.CallbackContext context)
        {
            if (!isStarted)
            {
                isStarted = true;
                GameManager.Instance.ChangeGameState(GameState.Playing);
                _playerAnimationController.SetRunAnimation();
            }
            
            if (GameManager.Instance.GetCurrentGameState() != GameState.Playing) return;
            _moveInput = context.ReadValue<Vector2>();
        }
        
        private void BonusActionOnStarted(InputAction.CallbackContext obj)
        {
            EventManager.OnBonusActionPerformed?.Invoke();
        }

        #region Initialize & Cleanup
        
        private void GetComponents()
        {
            _moveAction = _inputActionAsset.FindAction(Conts.InputAction.MOVE_ACTION);
            _bonusAction = _inputActionAsset.FindAction(Conts.InputAction.BONUS_ACTION);
            _playerAnimationController = GetComponent<PlayerAnimationController>();
        }
        
        private void StartActions()
        {
            _moveAction.Enable();
            _moveAction.started += MoveActionOnStarted;
            _moveAction.performed += MoveActionOnStarted;
            _moveAction.canceled += MoveActionOnStarted;
            
            _bonusAction.Enable();
            _bonusAction.started += BonusActionOnStarted;
            _bonusAction.performed += BonusActionOnStarted;
            _bonusAction.canceled += BonusActionOnStarted;
        }

        private void StopActions()
        {
            _moveAction.started -= MoveActionOnStarted;
            _moveAction.performed -= MoveActionOnStarted;
            _moveAction.canceled -= MoveActionOnStarted;
            _moveAction.Disable();
            
            _bonusAction.started -= BonusActionOnStarted;
            _bonusAction.performed -= BonusActionOnStarted;
            _bonusAction.canceled -= BonusActionOnStarted;
        }
        
        #endregion
    }
}
