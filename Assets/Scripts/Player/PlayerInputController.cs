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
    
        [Header("Input Action Callbacks")]
        private Vector2 _moveInput;
        public Vector2 MoveInput => _moveInput;
        
        [Header("Settings")]
        private bool isStarted;

        private void Awake()
        {
            _moveAction = _inputActionAsset.FindAction(Conts.InputAction.MOVE_ACTION);
            _playerAnimationController = GetComponent<PlayerAnimationController>();
        }
    
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

        #region Initialize & Cleanup

        private void OnEnable()
        {
            _moveAction.Enable();
            _moveAction.started += MoveActionOnStarted;
            _moveAction.performed += MoveActionOnStarted;
            _moveAction.canceled += MoveActionOnStarted;
        }

        private void OnDisable()
        {
            _moveAction.started -= MoveActionOnStarted;
            _moveAction.performed -= MoveActionOnStarted;
            _moveAction.canceled -= MoveActionOnStarted;
            _moveAction.Disable();
        }

        #endregion
    }
}
