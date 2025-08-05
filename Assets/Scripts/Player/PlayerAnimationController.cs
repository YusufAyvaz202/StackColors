using UnityEngine;

namespace Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [Header("References")]
        private Animator _playerAnimator;
        
        #region Unity Methods

        private void Awake()
        {
            _playerAnimator = GetComponent<Animator>();
        }
        
        #endregion
        
        public void SetRunAnimation()
        {
            _playerAnimator.SetTrigger(Misc.Conts.PlayerAnimations.RUN);
        }
    }
}