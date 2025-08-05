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
        [SerializeField] private float _horizontalSpeed= 5f;
        [SerializeField] private float _forwardSpeed = 5f;

        private void Awake()
        {
            GetComponents();
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            Vector2 moveInput = _playerInputController.MoveInput;
            Vector3 movementDirection = new Vector3(moveInput.x * _horizontalSpeed, 0, _forwardSpeed);
            _rigidbody.MovePosition(_rigidbody.position + movementDirection * Time.fixedDeltaTime);
        }

        #region Initialize & Cleanup

        private void GetComponents()
        {
            _playerInputController = GetComponent<PlayerInputController>();
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        #endregion
    }
}