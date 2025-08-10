using UnityEngine;

namespace Collectibles
{
    public class FollowParent : MonoBehaviour
    {
        [SerializeField] private Transform _followTransform;
        [SerializeField] private float swayStrength = 0.5f; 
        [SerializeField] private float swaySpeed = 5f; 
        [SerializeField] private float dampening = 2f; 
        
        private Vector3 _baseLocalPosition; 
        private bool _basePositionSet;
        private Vector3 _currentVelocity;
        private Vector3 _lastParentPosition;
        private float _swayOffset;
        
        private void Start()
        {
            if (_followTransform != null)
            {
                _lastParentPosition = _followTransform.position;
            }
        }

        private void FixedUpdate()
        {
            if (_followTransform == null) return;
            
            if (!_basePositionSet)
            {
                _baseLocalPosition = transform.localPosition;
                _basePositionSet = true;
            }
            
            Vector3 parentMovement = _followTransform.position - _lastParentPosition;
            float horizontalMovement = parentMovement.x;
            
            _swayOffset += -horizontalMovement * swayStrength;
            
            _swayOffset = Mathf.Lerp(_swayOffset, 0f, Time.fixedDeltaTime * dampening);
            
            Vector3 currentPos = transform.localPosition;
            Vector3 targetPosition = new Vector3(
                _baseLocalPosition.x + _swayOffset,
                _baseLocalPosition.y, 
                _baseLocalPosition.z  
            );
            
            transform.localPosition = new Vector3(
                Mathf.Lerp(currentPos.x, targetPosition.x, Time.fixedDeltaTime * swaySpeed),
                _baseLocalPosition.y, 
                _baseLocalPosition.z  
            );
            
            _lastParentPosition = _followTransform.position;
        }
        
        public void SetFollowTransform(Transform followTransform)
        {
            _followTransform = followTransform;
            if (_followTransform != null)
            {
                _lastParentPosition = _followTransform.position;
            }
        }
        
        public void UpdateBasePosition()
        {
            _baseLocalPosition = transform.localPosition;
            _basePositionSet = true;
        }

		public void SetSwayStrength(float strength)
        {
            swayStrength = strength;
        }
        
        public void SetDampening(float value)
        {
            dampening = value;
        }

    }
}