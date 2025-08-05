using Managers;
using Misc;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInputController : MonoBehaviour
    {
        [Header("Input Action Asset")]
        [SerializeField] private InputActionAsset _inputActionAsset;
    
        [Header("Input Actions")]
        private InputAction _moveAction;
    
        [Header("Input Action Callbacks")]
        private Vector2 _moveInput;
        public Vector2 MoveInput => _moveInput;

        private void Awake()
        {
            _moveAction = _inputActionAsset.FindAction(Conts.InputAction.MOVE_ACTION);
        }
    
        private void MoveActionOnStarted(InputAction.CallbackContext context)
        {
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
