using Managers;
using Misc;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("References")] 
        private PlayerInputController _playerInputController;
        private Rigidbody _rigidbody;

        [Header("Movement Settings")]
        [SerializeField] private float _horizontalSpeed = 5f;
        [SerializeField] private float _currentForwardSpeed = 5f;
        private float baseForwardSpeed;

        /// <summary>
        /// Unity lifecycle methods for initialization and updates.
        /// </summary>

        #region Unity Methods

        private void Awake()
        {
            GetComponents();
            baseForwardSpeed = _currentForwardSpeed;
        }

        private void FixedUpdate()
        {
            Move();
        }

        #endregion

        private void Move()
        {
            GameState gameState = GameManager.Instance.GetCurrentGameState(); 
            if (gameState != GameState.Playing && gameState != GameState.FewerMode) return;

            Vector2 moveInput = _playerInputController.MoveInput;
            Vector3 movementDirection = new Vector3(moveInput.x * _horizontalSpeed, 0, _currentForwardSpeed);
            _rigidbody.MovePosition(_rigidbody.position + movementDirection * Time.fixedDeltaTime);
            
            // Clamp the player's position to prevent going out of bounds
            _rigidbody.position = new Vector3(Mathf.Clamp(_rigidbody.position.x, -4, 4), _rigidbody.position.y, _rigidbody.position.z);
        }

        #region Helper Methods

        public void IncreaseForwardSpeed(float speed)
        {
            _currentForwardSpeed += speed;
        }

        public void ResetForwardSpeed()
        {
            _currentForwardSpeed = baseForwardSpeed;
        }

        public void DisableHorizontalMovement()
        {
            _horizontalSpeed = 0f;
        }

        #endregion

        #region Initialize & Cleanup

        private void GetComponents()
        {
            _playerInputController = GetComponent<PlayerInputController>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        #endregion
    }
}